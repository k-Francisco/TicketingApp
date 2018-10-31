using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.LaborUsed;
using TicketingApp.Models.MaterialUsed;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;

namespace TicketingApp.ViewModels
{
	public class InvoicePageViewModel : ViewModelBase
	{
        private ObservableCollection<LaborUsed>  _laborUsed;
        public ObservableCollection<LaborUsed> LaborUsed
        {
            get { return _laborUsed; }
            set { SetProperty(ref _laborUsed, value); }
        }

        private ObservableCollection<MaterialUsed> _materialUsed;
        public ObservableCollection<MaterialUsed> MaterialUsed
        {
            get { return _materialUsed; }
            set { SetProperty(ref _materialUsed, value); }
        }

        private ObservableCollection<EquipmentUsed> _equipmentUsed;
        public ObservableCollection<EquipmentUsed> EquipmentUsed
        {
            get { return _equipmentUsed; }
            set { SetProperty(ref _equipmentUsed, value); }
        }

        private ObservableCollection<ThirdPartyUsed> _thirdPartyUsed;
        public ObservableCollection<ThirdPartyUsed> ThirdPartyUsed
        {
            get { return _thirdPartyUsed; }
            set { SetProperty(ref _thirdPartyUsed, value); }
        }

        private string _costCode;
        public string CostCode
        {
            get { return _costCode; }
            set { SetProperty(ref _costCode, value); }
        }

        private Ticket _ticket;
        public Ticket Ticket
        {
            get { return _ticket; }
            set { SetProperty(ref _ticket, value); }
        }

        public Func<Task<byte[]>> SignatureFromStream { get; set; }
        public byte[] Signature { get; set; }


        public InvoicePageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI)
            :base (navigationService, sharepointAPI)
        {
            
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.New)
            {
                Ticket = parameters.GetValue<Ticket>("ticket");
                var job = parameters.GetValue<Job>("job");

                CostCode = job.CostCode;

                var savedLaborUsed = realm.All<LaborUsed>()
                                    .Where(l => l.TicketId == Ticket.ID)
                                    .ToList();

                LaborUsed = new ObservableCollection<LaborUsed>(savedLaborUsed);

                var savedMaterialUsed = realm.All<MaterialUsed>()
                                        .Where(m => m.TicketId == Ticket.ID)
                                        .ToList();

                MaterialUsed = new ObservableCollection<MaterialUsed>(savedMaterialUsed);

                var savedEquipmentUsed = realm.All<EquipmentUsed>()
                                        .Where(e => e.TicketId == Ticket.ID)
                                        .ToList();

                EquipmentUsed = new ObservableCollection<EquipmentUsed>(savedEquipmentUsed);

                var savedThirdPartyUsed = realm.All<ThirdPartyUsed>()
                                          .Where(t => t.TicketId == Ticket.ID)
                                          .ToList();

                ThirdPartyUsed = new ObservableCollection<ThirdPartyUsed>(savedThirdPartyUsed);

            }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand Save
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(async () =>
                    {
                        Signature = await SignatureFromStream();
                        System.Diagnostics.Debug.WriteLine("signature", Convert.ToBase64String(Signature));
                    });
                }

                return _saveCommand;
            }
        }
    }
}
