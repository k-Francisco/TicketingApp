using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Models.EquipmentUnit;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.InvoicedTickets;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.LaborUsed;
using TicketingApp.Models.Material;
using TicketingApp.Models.MaterialUsed;
using TicketingApp.Models.SavedRequests;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using TicketingApp.Models.Users;

namespace TicketingApp.ViewModels
{
    public class InvoicePageViewModel : ViewModelBase
    {
        private ObservableCollection<LaborUsed> _laborUsed = new ObservableCollection<LaborUsed>();
        public ObservableCollection<LaborUsed> LaborUsed
        {
            get { return _laborUsed; }
            set { SetProperty(ref _laborUsed, value); }
        }

        private ObservableCollection<MaterialUsed> _materialUsed = new ObservableCollection<MaterialUsed>();
        public ObservableCollection<MaterialUsed> MaterialUsed
        {
            get { return _materialUsed; }
            set { SetProperty(ref _materialUsed, value); }
        }

        private ObservableCollection<EquipmentUsed> _equipmentUsed = new ObservableCollection<EquipmentUsed>();
        public ObservableCollection<EquipmentUsed> EquipmentUsed
        {
            get { return _equipmentUsed; }
            set { SetProperty(ref _equipmentUsed, value); }
        }

        private ObservableCollection<ThirdPartyUsed> _thirdPartyUsed = new ObservableCollection<ThirdPartyUsed>();
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

        private int _laborUsedHeight;
        public int LaborUsedHeight
        {
            get { return _laborUsedHeight; }
            set { SetProperty(ref _laborUsedHeight, value); }
        }

        private int _equipmentUsedHeight;
        public int EquipmentUsedHeight
        {
            get { return _equipmentUsedHeight; }
            set { SetProperty(ref _equipmentUsedHeight, value); }
        }

        private int _materialUsedHeight;
        public int MaterialUsedHeight
        {
            get { return _materialUsedHeight; }
            set { SetProperty(ref _materialUsedHeight, value); }
        }

        private int _thirdPTUsedHeight;
        public int ThirdPTUsedHeight
        {
            get { return _thirdPTUsedHeight; }
            set { SetProperty(ref _thirdPTUsedHeight, value); }
        }

        private double _laborUsedTotal;
        public double LaborUsedTotal
        {
            get { return _laborUsedTotal; }
            set { SetProperty(ref _laborUsedTotal, value); }
        }

        private double _equipmentUsedTotal;
        public double EquipmentUsedTotal
        {
            get { return _equipmentUsedTotal; }
            set { SetProperty(ref _equipmentUsedTotal, value); }
        }

        private double _materialUsedTotal;
        public double MaterialUsedTotal
        {
            get { return _materialUsedTotal; }
            set { SetProperty(ref _materialUsedTotal, value); }
        }

        private double _thirdPTUsedTotal;
        public double ThirdPTUsedTotal
        {
            get { return _thirdPTUsedTotal; }
            set { SetProperty(ref _thirdPTUsedTotal, value); }
        }

        public InvoicePageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI,
                                    IPageDialogService pageDialogService, IEventAggregator eventAggregator)
            : base(navigationService, sharepointAPI, pageDialogService, eventAggregator)
        {
            Title = "Invoice";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                Ticket = parameters.GetValue<Ticket>("ticket");
                var job = parameters.GetValue<Job>("job");

                CostCode = job.CostCode;

                var savedLaborUsed = realm.All<LaborUsed>()
                                    .Where(l => l.TicketId == Ticket.ID)
                                    .ToList();
                foreach (var item in savedLaborUsed)
                {
                    LaborUsed.Add(item);
                    LaborUsedHeight += 170;
                    //todo: labor used total
                }

                var savedMaterialUsed = realm.All<MaterialUsed>()
                                        .Where(m => m.TicketId == Ticket.ID)
                                        .ToList();

                foreach (var item in savedMaterialUsed)
                {
                    MaterialUsed.Add(item);
                    MaterialUsedHeight += 250;
                    //todo: material used total
                }

                var savedEquipmentUsed = realm.All<EquipmentUsed>()
                                        .Where(e => e.TicketId == Ticket.ID)
                                        .ToList();

                foreach (var item in savedEquipmentUsed)
                {
                    EquipmentUsed.Add(item);
                    EquipmentUsedHeight += 200;
                    //todo: equipment used total
                }

                var savedThirdPartyUsed = realm.All<ThirdPartyUsed>()
                                          .Where(t => t.TicketId == Ticket.ID)
                                          .ToList();

                foreach (var item in savedThirdPartyUsed)
                {
                    ThirdPartyUsed.Add(item);
                    ThirdPTUsedHeight += 180;
                    //todo: third party used total
                }
            }
        }

        public Func<Task<byte[]>> SignatureFromStream { get; set; }

        private DelegateCommand _saveCommand;

        public DelegateCommand Save
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(async () =>
                    {
                        try
                        {
                            var user = realm.All<User>().FirstOrDefault();

                            string signature = "";
                            var signatureBytes = await SignatureFromStream();
                            if (signatureBytes != null)
                                signature = "data:image/png;base64," + Convert.ToBase64String(signatureBytes);

                            var laborUsed = new List<LaborUsed>(LaborUsed);
                            var equipmentUsed = new List<EquipmentUsed>(EquipmentUsed);
                            var materialUsed = new List<MaterialUsed>(MaterialUsed);
                            var thirdPartyUsed = new List<ThirdPartyUsed>(ThirdPartyUsed);
                            var totals = new List<double>() { LaborUsedTotal, EquipmentUsedTotal, MaterialUsedTotal, ThirdPTUsedTotal };

                            var request = new SavedRequests()
                            {
                                CostCode = this.CostCode,
                                TicketNumber = this.Ticket.Title,
                                SavedTicket = this.Ticket,
                                LaborUsedCollection = JsonConvert.SerializeObject(laborUsed),
                                EquipmentUsedCollection = JsonConvert.SerializeObject(equipmentUsed),
                                MaterialUsedCollection = JsonConvert.SerializeObject(materialUsed),
                                ThirdPartyUsedCollection = JsonConvert.SerializeObject(thirdPartyUsed),
                                TotalCollection = JsonConvert.SerializeObject(totals),
                                Signature = signature,
                                UserId = user.UserId.ToString(),
                            };

                            if (connected)
                            {
                                var invoices = await SharepointAPI.GetListItemsByListTitle("Invoiced Tickets");

                                if (invoices.IsSuccessStatusCode)
                                {
                                    var invoicesString = await invoices.Content.ReadAsStringAsync();
                                    var invoicedTicketsResults = JsonConvert.DeserializeObject<Models.InvoicedTickets.RootObject>(invoicesString,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore
                                        });

                                    var tixNumber = Ticket.Title;
                                    var invoicedTickets = invoicedTicketsResults.D.Results
                                                          .Where(i => i.TicketNumber.Equals(tixNumber))
                                                          .ToList();

                                    int invoiceCount = 0;
                                    if (invoicedTickets != null && invoicedTickets.Count != 0)
                                    {
                                        invoiceCount = invoicedTickets.Count + 1;
                                    }
                                    request.InvoiceCount = invoiceCount.ToString();

                                    var builder = new StringBuilder();
                                    var invoiceBody = GetInvoiceBody(request);
                                    foreach (var metadata in invoiceBody)
                                    {
                                        builder.Append(metadata);
                                    }

                                    var item = new StringContent(builder.ToString());
                                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                                    var formDigest = await SharepointAPI.GetFormDigest();

                                    var addInvoice = await SharepointAPI.AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                                           "Invoiced Tickets", item);

                                    var ensure = addInvoice.EnsureSuccessStatusCode();

                                    if (ensure.IsSuccessStatusCode)
                                    {
                                        //TODO: prompt for successful request
                                        System.Diagnostics.Debug.WriteLine("Success!");

                                        var response = JsonConvert.DeserializeObject<RootObject2>(await addInvoice.Content.ReadAsStringAsync());
                                        var invoice = response.invoice;
                                        realm.Write(() =>
                                        {
                                            realm.Add(invoice);
                                        });

                                        UpdateTicket(formDigest.D.GetContextWebInformation.FormDigestValue, signature, Ticket.ID.ToString());
                                    }
                                    else
                                    {
                                        //TODO: prompt for unsuccessful request
                                        System.Diagnostics.Debug.WriteLine("not success!");
                                    }
                                }
                                else
                                {
                                    //TODO: prompt for failure 
                                }
                            }
                            else
                            {
                                //TODO: prompt the user nga na ma upload ang request the next time online ang gago
                                realm.Write(() =>
                                {
                                    realm.Add<SavedRequests>(request);
                                });
                            }
                           
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("saveerror", e.Message);
                        }
                    });
                }

                return _saveCommand;
            }
        }
    }
}