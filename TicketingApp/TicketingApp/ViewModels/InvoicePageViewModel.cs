using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketingApp.ViewModels
{
	public class InvoicePageViewModel : ViewModelBase
	{
        public InvoicePageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI)
            :base (navigationService, sharepointAPI)
        {

        }
	}
}
