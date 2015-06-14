using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.Model
{
    public class HomeModel
    {
        public HomeModel(String title, String imagePath)
        {
            this.ImagePath = imagePath;
            this.Title = title;
        }

        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
