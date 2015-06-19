using ICICIMerchant.DBHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ICICIMerchant.Helper
{
    public static class EncryptionUtility
    {
        public static string AES_Encrypt(string input, string pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string encrypted = "";
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(input));
                encrypted = CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(AES, Buffer, null));

                return encrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string AES_Decrypt(string input, string pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            string decrypted = "";
            try
            {
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                IBuffer Buffer = CryptographicBuffer.DecodeFromBase64String(input);
                byte[] Decrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(AES, Buffer, null), out Decrypted);
                decrypted = System.Text.Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);

                return decrypted;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Returns an encrypted text.
        /// </summary>
        /// <param name="toEncrypt">String to be encrypted</param>
        /// <param name="key">Unique key for encryption/decryption</param>m>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be encrypted.
                var toDecryptBuffer = CryptographicBuffer.ConvertStringToBinary(toEncrypt, BinaryStringEncoding.Utf8);

                // Open a symmetric algorithm provider for the specified algorithm.
                var aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                // The input key must be securely shared between the sender of the cryptic message
                // and the recipient. The initialization vector must also be shared but does not
                // need to be shared in a secure manner. If the sender encodes a message string
                // to a buffer, the binary encoding method must also be shared with the recipient.
                var buffEncrypted = CryptographicEngine.Encrypt(symetricKey, toDecryptBuffer, null);

                // Convert the encrypted buffer to a string (for display).
                // We are using Base64 to convert bytes to string since you might get unmatched characters
                // in the encrypted buffer that we cannot convert to string with UTF8.
                var strEncrypted = CryptographicBuffer.EncodeToBase64String(buffEncrypted);

                return strEncrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                return "";
            }
        }

        private static IBuffer GetMD5Hash(string key)
        {
            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

            // Hash the message.
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }

            return buffHash;
        }

        /// <summary>
        /// Decrypt a string using dual encryption method. Return a Decrypted clear string
        /// </summary>
        /// <param name="cipherString">Encrypted string</param>
        /// <param name="key">Unique key for encryption/decryption</param>
        /// <returns>Returns decrypted text.</returns>
        public static string Decrypt(string cipherString, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be decrypted.
                IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(cipherString);

                // Open a symmetric algorithm provider for the specified algorithm.
                SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);

                string strDecrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);

                return strDecrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                //throw;
                return "";
            }
        }
        public static string Encrypt(string dataToEncrypt, string password, string salt)
        {
            // Generate a key and IV from the password and salt
            IBuffer aesKeyMaterial;
            IBuffer iv;
            uint iterationCount = 10000;
            GenerateKeyMaterial(password, salt, iterationCount, out aesKeyMaterial, out iv);

            IBuffer plainText = CryptographicBuffer.ConvertStringToBinary(dataToEncrypt, BinaryStringEncoding.Utf8);

            // Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
            SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey aesKey = aesProvider.CreateSymmetricKey(aesKeyMaterial);

            // Encrypt the data and convert it to a Base64 string
            IBuffer encrypted = CryptographicEngine.Encrypt(aesKey, plainText, iv);
            return CryptographicBuffer.EncodeToBase64String(encrypted);
        }

        private static void GenerateKeyMaterial(string password, string salt, uint iterationCount, out IBuffer keyMaterial, out IBuffer iv)
        {
            // Setup KDF parameters for the desired salt and iteration count
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf8);
            KeyDerivationParameters kdfParameters = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, iterationCount);

            // Get a KDF provider for PBKDF2, and store the source password in a Cryptographic Key
            KeyDerivationAlgorithmProvider kdf = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha256);
            IBuffer passwordBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            CryptographicKey passwordSourceKey = kdf.CreateKey(passwordBuffer);

            // Generate key material from the source password, salt, and iteration count.  Only call DeriveKeyMaterial once,
            // since calling it twice will generate the same data for the key and IV.
            int keySize = 128 / 8;
            int ivSize = 128 / 8;
            uint totalDataNeeded = (uint)(keySize + ivSize);
            IBuffer keyAndIv = CryptographicEngine.DeriveKeyMaterial(passwordSourceKey, kdfParameters, totalDataNeeded);

            // Split the derived bytes into a seperate key and IV
            byte[] keyMaterialBytes = keyAndIv.ToArray();
            keyMaterial = WindowsRuntimeBuffer.Create(keyMaterialBytes, 0, keySize, keySize);
            iv = WindowsRuntimeBuffer.Create(keyMaterialBytes, keySize, ivSize, ivSize);
        }

        public static string Decrypt(string dataToDecrypt, string password, string salt)
        {
            // Generate a key and IV from the password and salt
            IBuffer aesKeyMaterial;
            IBuffer iv;
            uint iterationCount = 10000;
            GenerateKeyMaterial(password, salt, iterationCount, out aesKeyMaterial, out iv);

            // Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
            SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey aesKey = aesProvider.CreateSymmetricKey(aesKeyMaterial);

            // Convert the base64 input to an IBuffer for decryption
            IBuffer ciphertext = CryptographicBuffer.DecodeFromBase64String(dataToDecrypt);

            // Decrypt the data and convert it back to a string
            IBuffer decrypted = CryptographicEngine.Decrypt(aesKey, ciphertext, iv);
            byte[] decryptedArray = decrypted.ToArray();
            return Encoding.UTF8.GetString(decryptedArray, 0, decryptedArray.Length);
        }

    }

    public static class EncryptionProvider
    {
      

        /// <summary> 
        /// Encrypt a string 
        /// </summary> 
        /// <param name="input">String to encrypt</param> 
        /// <param name="password">Password to use for encryption</param> 
        /// <returns>Encrypted string</returns> 
        public static string Encrypt(string input, string password)
        {
            //make sure we have data to work with 
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("input cannot be empty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password cannot be empty");
            // get IV, key and encrypt 
            var iv = CreateInitializationVector(DBHandler.ivKey);
            var key = CreateKey(DBHandler.key1);
            var encryptedBuffer = CryptographicEngine.Encrypt(
              key, CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8), iv);
            return CryptographicBuffer.EncodeToBase64String(encryptedBuffer);
        }
        /// <summary> 
        /// Decrypt a string previously ecnrypted with Encrypt method and the same password 
        /// </summary> 
        /// <param name="input">String to decrypt</param> 
        /// <param name="password">Password to use for decryption</param> 
        /// <returns>Decrypted string</returns> 
        public static string Decrypt(string input, string password)
        {
            //make sure we have data to work with 
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("input cannot be empty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password cannot be empty");
            // get IV, key and decrypt 
            var iv = CreateInitializationVector(password);
            var key = CreateKey(password);
            var decryptedBuffer = CryptographicEngine.Decrypt(
              key, CryptographicBuffer.DecodeFromBase64String(input), iv);
            return CryptographicBuffer.ConvertBinaryToString(
             BinaryStringEncoding.Utf8, decryptedBuffer);
        }
        /// <summary> 
        /// Create initialization vector IV 
        /// </summary> 
        /// <param name="password">Password is used for random vector generation</param> 
        /// <returns>Vector</returns> 
        private static IBuffer CreateInitializationVector(string password)
        {
            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");

            var newPassword = password;
            // make sure we satify minimum length requirements 
            while (newPassword.Length < provider.BlockLength)
            {
                newPassword = newPassword + password;
            }
            //create vecotr 
            var iv = CryptographicBuffer.CreateFromByteArray(
              UTF8Encoding.UTF8.GetBytes(newPassword));
            return iv;
        }
        /// <summary> 
        /// Create encryption key 
        /// </summary> 
        /// <param name="password">Password is used for random key generation</param> 
        /// <returns></returns> 
        private static CryptographicKey CreateKey(string password)
        {
            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            var newPassword = password;
            // make sure we satify minimum length requirements 
            while (newPassword.Length < provider.BlockLength)
            {
                newPassword = newPassword + password;
            }
            var buffer = CryptographicBuffer.ConvertStringToBinary(
              newPassword, BinaryStringEncoding.Utf8);
            buffer.Length = provider.BlockLength;
            var key = provider.CreateSymmetricKey(buffer);
            return key;
        }
    }

    public class EncryptionHelper
    {
        /// <summary> 
        /// Encrypt a string 
        /// </summary> 
        /// <param name="input">String to encrypt</param> 
        /// <param name="password">Password to use for encryption</param> 
        /// <returns>Encrypted string</returns> 
        public string Encrypt(string input, string keyValue, string ivKey)
        {
            IBuffer encrypted = null;
            IBuffer decrypted;
            IBuffer iv = null;
            IBuffer data;
            //IBuffer nonce;
            String algName = SymmetricAlgorithmNames.AesCbcPkcs7;

            CryptographicKey key = null;
            key = GenerateSymmetricKey();
            data = GenearetData(keyValue);

            // CBC mode needs Initialization vector, here just random data.
            // IV property will be set on "Encrypted".
            if (algName.Contains("CBC"))
            {
                SymmetricKeyAlgorithmProvider algorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algName);
                //iv = CryptographicBuffer.GenerateRandom(algorithm.BlockLength); //Original...
                iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(ivKey)); //New added for ivkey
            }

            // Encrypt the data.
            try
            {
                encrypted = CryptographicEngine.Encrypt(key, data, iv);
            }
            catch (ArgumentException ex)
            {
               // return;
            }

            // Decrypt the data.
            decrypted = CryptographicEngine.Decrypt(key, encrypted, iv);

            if (!CryptographicBuffer.Compare(decrypted, data))
            {
               // return;
            }
            return CryptographicBuffer.EncodeToBase64String(encrypted);
        }
        private CryptographicKey GenerateSymmetricKey()
        {
            String algName = SymmetricAlgorithmNames.AesCbcPkcs7;
            UInt32 keySize = 128 / 8;

            CryptographicKey key;
            // Create an SymmetricKeyAlgorithmProvider object for the algorithm specified on input.
            SymmetricKeyAlgorithmProvider Algorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algName);

            // Generate a symmetric key.
            IBuffer keymaterial = CryptographicBuffer.GenerateRandom(keySize);
            try
            {
                key = Algorithm.CreateSymmetricKey(keymaterial);
            }
            catch (ArgumentException ex)
            {
                return null;
            }
            return key;
        }

        private IBuffer GenearetData(string keyValue)
        {
            IBuffer data;
            //String cookie = "Data to encrypt "; // Original..
            String cookie = keyValue;
            data = CryptographicBuffer.ConvertStringToBinary(cookie, BinaryStringEncoding.Utf8);
            return data;
        }
    }
}
