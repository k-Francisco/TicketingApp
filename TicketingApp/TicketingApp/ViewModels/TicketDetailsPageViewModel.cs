using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services.Sharepoint_API;
using TicketingApp.Models.Customers;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.Tickets;

namespace TicketingApp.ViewModels
{
    public class TicketDetailsPageViewModel : ViewModelBase
    {
        private Ticket _ticket;
        public Ticket Ticket
        {
            get { return _ticket; }
            set { SetProperty(ref _ticket, value); }
        }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }

        private Job _job;
        public Job Job
        {
            get { return _job; }
            set { SetProperty(ref _job, value); }
        }


        public TicketDetailsPageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI)
            : base(navigationService, sharepointAPI)
        {
            Title = "Details";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.New)
            {
                Ticket = parameters.GetValue<Ticket>("ticket");
                Customer = parameters.GetValue<Customer>("customer");
                Job = parameters.GetValue<Job>("job");
            }
        }

        private DelegateCommand _previewInvoiceCommand;
        public DelegateCommand PreviewInvoiceCommand
        {
            get
            {
                if (_previewInvoiceCommand == null)
                {
                    _previewInvoiceCommand = new DelegateCommand(async () =>
                    {

                        var navParams = new NavigationParameters();
                        navParams.Add("ticket", Ticket);
                        navParams.Add("job", Job);
                        await NavigationService.NavigateAsync(nameof(Views.InvoicePage),
                                                        navParams,
                                                        false,
                                                        false);
                    });
                }

                return _previewInvoiceCommand;
            }
        }
    }
}