using ICICIMerchant.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.ViewModel
{
    public class HomeViewModel
    {
        private ObservableCollection<HomeModel> _items = new ObservableCollection<HomeModel>();

        public ObservableCollection<HomeModel> Items
        {
            get { return this._items; }
        }

        public HomeViewModel()
        {
            Items.Add(new HomeModel("PAPER ROLL REQUEST", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("STATEMENT REQUEST", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("TERMINAL QUERY", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("STATUS OF PREVIOUS TICKET", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("TALK TO RELATIONSHIP MANAGER", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("CUSTOMER SUPPORT", "ms-appx:///Assets/HubBackground.theme-light.png"));
            Items.Add(new HomeModel("REGISTER CONTACT NUMBER", "ms-appx:///Assets/HubBackground.theme-light.png"));
        }
    }
}
