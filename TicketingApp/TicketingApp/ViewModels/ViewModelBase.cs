using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Realms;
using SpevoCore.Services.Sharepoint_API;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected ISharepointAPI SharepointAPI { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }
        protected Realm realm { get; private set; }
        protected bool connected { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public ViewModelBase(INavigationService navigationService, ISharepointAPI sharepointAPI,
                             IPageDialogService pageDialogService, IEventAggregator eventAggregator)
        {
            RealmConfiguration.DefaultConfiguration.SchemaVersion = 2;

            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            EventAggregator = eventAggregator;

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
