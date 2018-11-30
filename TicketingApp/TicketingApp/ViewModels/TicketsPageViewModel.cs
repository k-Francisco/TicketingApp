using Fusillade;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Realms;
using SpevoCore.Services;
using SpevoCore.Services.API_Service;
using SpevoCore.Services.Token_Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using TicketingApp.Models.SavedRequests;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using TicketingApp.Models.Users;
using TicketingApp.Services;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class TicketsPageViewModel : ViewModelBase
    {
        private ObservableCollection<Ticket> _ticketCollection = new ObservableCollection<Ticket>();
        public ObservableCollection<Ticket> TicketCollection
        {
            get { return _ticketCollection; }
        }

        private bool _refreshing;
        public bool Refreshing
        {
            get { return _refreshing; }
            set { SetProperty(ref _refreshing, value); }
        }

        private string _syncStage;
        public string SyncStage
        {
            get { return _syncStage; }
            set { SetProperty(ref _syncStage, value); }
        }

        private readonly IClearCookies _clearCookieService;
        private readonly ITokenService _tokenService;

        public TicketsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IApiManager apiManager,
            IClearCookies clearCookieService, ITokenService tokenService)
            : base(navigationService, eventAggregator, apiManager)
        {
            Title = "Tickets";
            _clearCookieService = clearCookieService;
            _tokenService = tokenService;

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    System.Diagnostics.Debug.WriteLine("connect", "yeah");
                    CheckSavedRequests();
                }
            };
            SaveUser();
            SyncData();
        }

        private async void SaveUser()
        {
            try
            {
                if (IsConnected)
                {
                    var savedUser = realm.All<User>().FirstOrDefault();
                    if (savedUser == null)
                    {
                        var userResponse = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetCurrentUser());
                        var userString = await userResponse.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<SpevoCore.Models.User.UserModel>(userString);
                        realm.Write(() =>
                        {
                            realm.Add(new User()
                            {
                                IsSiteAdmin = user.D.IsSiteAdmin,
                                UserEmail = user.D.Email,
                                UserId = user.D.Id,
                                UserName = user.D.Title
                            });
                        });
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SaveUser", e.Message);
            }
        }

        private async void SyncData()
        {
            try
            {
                if (IsConnected)
                {
                    System.Diagnostics.Debug.WriteLine("rtFa", TokenService.GetInstance.ExtractRtFa());

                    Refreshing = true;

                    SyncStage = "Sending pending requests...";

                    //to upload all the items that were saved during offline situations
                    CheckSavedRequests();

                    var laborUsedSelect = "ID,Title,WorkType,STHours,OTHours,PerDiem,Billable,TicketId,Created,Modified,GUID,Employee/Title";
                    var laborUsedExpand = "Employee";
                    var thirdPartyUsedSelect = "ID,RequestDate,Description,Amount,MarkUp,Billable,ChargeType,TicketId,Created,Modified,Vendor/Title";
                    var thirdPartyUsedExpand = "Vendor";

                    var batchRequests = new List<Task<HttpResponseMessage>>() {
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Tickets")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Customers")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Equipment Unit")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("EquipmentUsed")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Invoiced Tickets")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Jobs")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Labor Used", laborUsedSelect, laborUsedExpand)),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Material")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Material Used")),
                        ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetListItemsByListTitle("Third Party Used", thirdPartyUsedSelect, thirdPartyUsedExpand)),
                    };

                    SyncStage = "Getting data from server...";

                    var responses = await Task.WhenAll(batchRequests.ToArray());

                    var batchConversion = new List<Task<string>>()
                    {
                        responses[0].Content.ReadAsStringAsync(),
                        responses[1].Content.ReadAsStringAsync(),
                        responses[2].Content.ReadAsStringAsync(),
                        responses[3].Content.ReadAsStringAsync(),
                        responses[4].Content.ReadAsStringAsync(),
                        responses[5].Content.ReadAsStringAsync(),
                        responses[6].Content.ReadAsStringAsync(),
                        responses[7].Content.ReadAsStringAsync(),
                        responses[8].Content.ReadAsStringAsync(),
                        responses[9].Content.ReadAsStringAsync(),
                    };

                    var batchConversionResults = await Task.WhenAll(batchConversion.ToArray());

                    #region saved data

                    var savedTickets = realm.All<Ticket>().ToList();
                    var savedCustomers = realm.All<Customer>().ToList();
                    var savedEquipmentUnit = realm.All<EquipmentUnit>().ToList();
                    var savedEquipmentUsed = realm.All<EquipmentUsed>().ToList();
                    var savedInvoicedTickets = realm.All<InvoicedTickets>().ToList();
                    var savedJobs = realm.All<Job>().ToList();
                    var savedLaborUsed = realm.All<LaborUsed>().ToList();
                    var savedMaterial = realm.All<Material>().ToList();
                    var savedMaterialUsed = realm.All<MaterialUsed>().ToList();
                    var savedThirdPartyUsed = realm.All<ThirdPartyUsed>().ToList();

                    #endregion saved data

                    #region response conversion

                    var tickets = JsonConvert.DeserializeObject<Models.Tickets.RootObject>(batchConversionResults[0],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting tix");
                    var customers = JsonConvert.DeserializeObject<Models.Customers.RootObject>(batchConversionResults[1],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting customers");
                    var equipmentUnit = JsonConvert.DeserializeObject<Models.EquipmentUnit.RootObject>(batchConversionResults[2],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting eq unit");
                    var equipmentUsed = JsonConvert.DeserializeObject<Models.EquipmentUsed.RootObject>(batchConversionResults[3],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting eq used");
                    var invoicedTickets = JsonConvert.DeserializeObject<Models.InvoicedTickets.RootObject>(batchConversionResults[4],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting inv tix");
                    var jobs = JsonConvert.DeserializeObject<Models.Jobs.RootObject>(batchConversionResults[5],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting jobs");
                    var laborUsed = JsonConvert.DeserializeObject<Models.LaborUsed.RootObject>(batchConversionResults[6],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting laborused");
                    var material = JsonConvert.DeserializeObject<Models.Material.RootObject>(batchConversionResults[7],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting material");
                    var materialUsed = JsonConvert.DeserializeObject<Models.MaterialUsed.RootObject>(batchConversionResults[8],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting material used");
                    var thirdPartyUsed = JsonConvert.DeserializeObject<Models.ThirdPartyUsed.RootObject>(batchConversionResults[9],
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    System.Diagnostics.Debug.WriteLine("tix", "Done converting tpu");

                    #endregion response conversion

                    SyncStage = "Syncing...";

                    SyncData(savedTickets, tickets.D.Results);
                    SyncData(savedCustomers, customers.D.Results);
                    SyncData(savedEquipmentUnit, equipmentUnit.D.Results);
                    SyncData(savedEquipmentUsed, equipmentUsed.D.Results);
                    SyncData(savedInvoicedTickets, invoicedTickets.D.Results);
                    SyncData(savedJobs, jobs.D.Results);
                    SyncData(savedLaborUsed, laborUsed.D.Results);
                    SyncData(savedMaterial, material.D.Results);
                    SyncData(savedMaterialUsed, materialUsed.D.Results);
                    SyncData(savedThirdPartyUsed, thirdPartyUsed.D.Results);
                }

                SyncStage = "";
                Refreshing = false;

                GetTickets();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncData", e.Message);
            }
        }

        private void GetTickets()
        {
            var tickets = realm.All<Ticket>();
            if (tickets != null || tickets.Any())
            {
                foreach (var item in tickets)
                {
                    TicketCollection.Add(item);
                }
            }
            else
                SyncStage = "No tickets to display";
        }

        public async void CheckSavedRequests()
        {
            try
            {
                var savedRequests = realm.All<SavedRequests>()
                                    .ToList();

                Dictionary<string, int> invoiceCounts = new Dictionary<string, int>();

                if (savedRequests != null || savedRequests.Any())
                {
                    if (IsConnected)
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

                            var ticketNumbers = new List<string>();

                            foreach (var item in invoicedTicketsResults.D.Results)
                            {
                                if (!ticketNumbers.Contains(item.TicketNumber))
                                    ticketNumbers.Add(item.TicketNumber);
                            }

                            foreach (var item in ticketNumbers)
                            {
                                var count = 0;
                                foreach (var item2 in invoicedTicketsResults.D.Results)
                                {
                                    if (item.Equals(item2.TicketNumber))
                                        count++;
                                }

                                invoiceCounts.Add(item, count);
                            }
                        }

                        var formDigestResponse = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).GetFormDigest());
                        var formDigestString = await formDigestResponse.Content.ReadAsStringAsync();
                        var formDigest = JsonConvert.DeserializeObject<SpevoCore.Models.FormDigest.FormDigestModel>(formDigestString);

                        var batch = new List<Task<HttpResponseMessage>>();

                        foreach (var request in savedRequests)
                        {
                            realm.Write(() =>
                            {
                                var count = invoiceCounts.Where(s => s.Key.Equals(request.TicketNumber)).FirstOrDefault();
                                request.InvoiceCount = Convert.ToString(count.Value + 1);
                                invoiceCounts[count.Key] = Convert.ToInt32(request.InvoiceCount);
                            });

                            var builder = new StringBuilder();
                            var invoiceBody = GetInvoiceBody(request);
                            foreach (var metadata in invoiceBody)
                            {
                                builder.Append(metadata);
                            }

                            var item = new StringContent(builder.ToString());
                            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                            batch.Add(ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated)
                                .AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,"Invoiced Tickets", item)));
                        }

                        var results = await Task.WhenAll(batch);

                        realm.Write(()=> {
                            for (int i = 0; i < results.Length; i++)
                            {
                                if (results[i].IsSuccessStatusCode)
                                {
                                    UpdateTicket(formDigest.D.GetContextWebInformation.FormDigestValue,
                                        savedRequests[i].Signature,
                                        savedRequests[i].SavedTicket.ID.ToString());
                                    realm.Remove(savedRequests[i]);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("check saved requests", e.Message);
            }
        }

        private DelegateCommand _refreshCommand;
        public DelegateCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(() =>
                    {
                        TicketCollection.Clear();
                        SyncData();
                    });
                }

                return _refreshCommand;
            }
        }

        private DelegateCommand<Ticket> _itemTappepdCommand;
        public DelegateCommand<Ticket> ItemTappedCommand
        {
            get
            {
                if (_itemTappepdCommand == null)
                {
                    _itemTappepdCommand = new DelegateCommand<Ticket>(async (tix) =>
                    {
                        var job = realm.All<Job>()
                                   .Where(j => j.Title.Equals(tix.JobName))
                                   .FirstOrDefault();

                        var customer = realm.All<Customer>()
                                       .Where(c => c.CustReferenceNo.Equals(tix.CustRefNo))
                                       .FirstOrDefault();

                        var navParams = new NavigationParameters();
                        navParams.Add("customer", customer);
                        navParams.Add("job", job);
                        navParams.Add("ticket", tix);
                        await NavigationService.NavigateAsync(nameof(Views.TicketDetailsPage),
                                                        navParams,
                                                        false,
                                                        false);
                    });
                }

                return _itemTappepdCommand;
            }
        }

        private DelegateCommand _accountCommand;
        public DelegateCommand AccountCommand
        {
            get
            {
                if(_accountCommand == null)
                {
                    _accountCommand = new DelegateCommand(() => {
                        var user = realm.All<User>().FirstOrDefault();

                        PageDialog.ActionSheet(new Acr.UserDialogs.ActionSheetConfig()
                                    .SetTitle(user.UserName)
                                    .SetMessage(user.UserEmail)
                                    .SetCancel()
                                    .SetDestructive("Logout", () => {
                                        PageDialog.ActionSheet(new Acr.UserDialogs.ActionSheetConfig()
                                            .SetTitle("Warning")
                                            .SetMessage("All of the requests that were saved would be deleted. Continue logging out?")
                                            .SetCancel()
                                            .SetDestructive("Logout",async ()=> {
                                                _clearCookieService.ClearAllCookies();
                                                _tokenService.Clear();
                                                realm.Write(() => {
                                                    realm.RemoveAll();
                                                });
                                                System.Diagnostics.Debug.WriteLine("Logout");

                                                await NavigationService.NavigateAsync(new Uri("app:///LoginPage", UriKind.Absolute));
                                            }));
                                    }));
                    });
                }

                return _accountCommand;
            }
        }
    }
}