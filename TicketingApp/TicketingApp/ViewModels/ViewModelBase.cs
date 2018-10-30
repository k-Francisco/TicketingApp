using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Realms;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Models.Customers;
using TicketingApp.Models.EquipmentUnit;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.InvoicedTickets;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.LaborUsed;
using TicketingApp.Models.Material;
using TicketingApp.Models.MaterialUsed;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using TicketingApp.Services;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected ISharepointAPI SharepointAPI { get; private set; }
        protected Realm realm { get; private set; }
        protected bool connected { get; private set; }

        public ViewModelBase(INavigationService navigationService, ISharepointAPI sharepointAPI)
        {
            NavigationService = navigationService;
            SharepointAPI = sharepointAPI;
            realm = Realm.GetInstance();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                connected = true;

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    connected = true;
                else
                    connected = false;
            };
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {

        }
    }
}
