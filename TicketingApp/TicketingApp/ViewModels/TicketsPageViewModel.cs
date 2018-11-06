﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services;
using SpevoCore.Services.Sharepoint_API;
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
using Xamarin.Forms;

namespace TicketingApp.ViewModels
{
    public class TicketsPageViewModel : ViewModelBase
    {
        private ObservableCollection<Ticket> _ticketCollection = new ObservableCollection<Ticket>();
        public ObservableCollection<Ticket> TicketCollection
        {
            get { return _ticketCollection; }
        }

        public TicketsPageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI) 
            : base(navigationService, sharepointAPI)
        {
            Title = "Tickets";
            SharepointAPI.Init(App.SiteUrl);
            SaveUser();
            SyncData();
        }

        private async void SaveUser()
        {
            try
            {
                if (connected)
                {
                    var savedUser = realm.All<User>().FirstOrDefault();
                    if(savedUser == null)
                    {
                        var user = await SharepointAPI.GetCurrentUser();
                        realm.Write(()=> {
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
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SaveUser", e.Message);
            }
        }

        private async void SyncData()
        {
            try
            {
                if (connected)
                {

                    System.Diagnostics.Debug.WriteLine("rtFa", TokenService.GetInstance().ExtractRtFa());

                    //to upload all the items that were saved during offline situations
                    var results = await CheckSavedRequests();

                    var laborUsedQuery = "$select=ID,Title,WorkType,STHours,OTHours,PerDiem,Billable,TicketId,Created,Modified,GUID" +
                                         ",Employee/Title&$expand=Employee";
                    var thirdPartUsedQuery = "$select=ID,RequestDate,Description,Amount,MarkUp,Billable,ChargeType,TicketId,Created," +
                                             "Modified,Vendor/Title&$expand=Vendor";

                    var batchRequests = new List<Task<HttpResponseMessage>>() {
                        SharepointAPI.GetListItemsByListTitle("Tickets"),
                        SharepointAPI.GetListItemsByListTitle("Customers"),
                        SharepointAPI.GetListItemsByListTitle("Equipment Unit"),
                        SharepointAPI.GetListItemsByListTitle("EquipmentUsed"),
                        SharepointAPI.GetListItemsByListTitle("Invoiced Tickets"),
                        SharepointAPI.GetListItemsByListTitle("Jobs"),
                        SharepointAPI.GetListItemsByListTitle("Labor Used",laborUsedQuery),
                        SharepointAPI.GetListItemsByListTitle("Material"),
                        SharepointAPI.GetListItemsByListTitle("Material Used"),
                        SharepointAPI.GetListItemsByListTitle("Third Party Used", thirdPartUsedQuery),
                    };

                    System.Diagnostics.Debug.WriteLine("tix", "Done getting response");

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

                    System.Diagnostics.Debug.WriteLine("tix", "Done converting response");

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
                    #endregion

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
                    #endregion

                    System.Diagnostics.Debug.WriteLine("tix", "Done deserializing");

                    var batchSync = new List<Task<bool>>() {
                        tickets.SyncData(savedTickets,tickets.D.Results),
                        customers.SyncData(savedCustomers, customers.D.Results),
                        equipmentUnit.SyncData(savedEquipmentUnit, equipmentUnit.D.Results),
                        equipmentUsed.SyncData(savedEquipmentUsed, equipmentUsed.D.Results),
                        invoicedTickets.SyncData(savedInvoicedTickets, invoicedTickets.D.Results),
                        jobs.SyncData(savedJobs, jobs.D.Results),
                        laborUsed.SyncData(savedLaborUsed, laborUsed.D.Results),
                        material.SyncData(savedMaterial, material.D.Results),
                        materialUsed.SyncData(savedMaterialUsed, materialUsed.D.Results),
                        thirdPartyUsed.SyncData(savedThirdPartyUsed, thirdPartyUsed.D.Results),
                    };

                    var doneSync = await Task.WhenAll(batchSync.ToArray());

                    System.Diagnostics.Debug.WriteLine("tix", "Done saving data");
                }

                GetTickets();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncData", e.Message);
            }
        }

        public async Task<HttpResponseMessage[]> CheckSavedRequests()
        {   //fix this shit
            try
            {
                var savedRequests = realm.All<SavedRequests>()
                                .ToList();

                var batch = new List<Task<HttpResponseMessage>>();

                var formDigest = await SharepointAPI.GetFormDigest();

                if (savedRequests != null || savedRequests.Any())
                {
                    foreach (var body in savedRequests)
                    {
                        var item = new StringContent(body.requestBody);
                        item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                        batch.Add(SharepointAPI.AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                               "Invoiced Tickets", item));
                    }

                    var results = await Task.WhenAll(batch);

                    for (int i = 0; i < results.Length; i++)
                    {
                        if (results[i].IsSuccessStatusCode)
                            realm.Write(() => {
                                realm.Remove(savedRequests[i]);
                            });
                    }

                    System.Diagnostics.Debug.WriteLine("connect", "success");

                    return results;
                }

                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("connect", e.Message);
                return null;
            }
        }

        private void GetTickets()
        {
            var tickets = realm.All<Ticket>();
            foreach (var item in tickets)
            {
                TicketCollection.Add(item);
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

    }
}