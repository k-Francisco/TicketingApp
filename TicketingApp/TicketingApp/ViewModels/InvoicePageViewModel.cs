using Fusillade;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using SpevoCore.Models.FormDigest;
using SpevoCore.Services.API_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.InvoicedTickets;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.LaborUsed;
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

        public InvoicePageViewModel(INavigationService navigationService,  IEventAggregator eventAggregator, IApiManager apiManager)
            : base(navigationService, eventAggregator, apiManager)
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
                    _saveCommand = new DelegateCommand(async () => await SendInvoiceItem());
                }

                return _saveCommand;
            }
        }

        private async Task SendInvoiceItem()
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
                    var invoices = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Invoiced Tickets"));

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

                        var formDigestResponse = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetFormDigest());
                        var formDigestString = await formDigestResponse.Content.ReadAsStringAsync();
                        var formDigest = JsonConvert.DeserializeObject<FormDigestModel>(formDigestString);

                        var addInvoice = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                               "Invoiced Tickets", item));

                        var ensure = addInvoice.EnsureSuccessStatusCode();

                        if (ensure.IsSuccessStatusCode)
                        {
                            PageDialog.Alert("Success!");

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
                            PageDialog.Alert("Something went wrong! Please check your internet connection and try again");
                        }
                    }
                    else
                    {
                        PageDialog.Alert("Something went wrong! Please check your internet connection and try again");
                    }
                }
                else
                {
                    PageDialog.Alert("The request will be sent the next time your device is connected to the internet");
                    realm.Write(() =>
                    {
                        var savedRequests = realm.All<SavedRequests>().ToList();
                        if(!savedRequests.Contains(request))
                            realm.Add<SavedRequests>(request);
                    });
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("saveerror", e.Message);
            }
        }
    }
}