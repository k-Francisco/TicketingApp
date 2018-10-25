using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;

namespace TicketingApp.ViewModels
{
    public class TicketsPageViewModel : ViewModelBase
    {
        public TicketsPageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI) 
            : base(navigationService, sharepointAPI)
        {
            SharepointAPI.Init(App.SiteUrl);
            SyncData();
        }

        private async void SyncData()
        {
            if (connected)
            {
                var batchRequests = new List<Task<HttpResponseMessage>>() {
                    SharepointAPI.GetListItemsByListTitle("Tickets"),
                    SharepointAPI.GetListItemsByListTitle("Customers"),
                    SharepointAPI.GetListItemsByListTitle("Equipment Unit"),
                    SharepointAPI.GetListItemsByListTitle("EquipmentUsed"),
                    SharepointAPI.GetListItemsByListTitle("Invoiced Tickets"),
                    SharepointAPI.GetListItemsByListTitle("Jobs"),
                    SharepointAPI.GetListItemsByListTitle("Labor Used"),
                    SharepointAPI.GetListItemsByListTitle("Material"),
                    SharepointAPI.GetListItemsByListTitle("Material Used"),
                    SharepointAPI.GetListItemsByListTitle("Third Party Used"),
                };

                var responses = await Task.WhenAll(batchRequests.ToArray());

                var tickets = JsonConvert.DeserializeObject<Models.Tickets.RootObject>(await responses[0].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var customers = JsonConvert.DeserializeObject<Models.Customers.RootObject>(await responses[1].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var equipmentUnit = JsonConvert.DeserializeObject<Models.EquipmentUnit.RootObject>(await responses[2].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var equipmentUsed = JsonConvert.DeserializeObject<Models.EquipmentUsed.RootObject>(await responses[3].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var invoicedTickets = JsonConvert.DeserializeObject<Models.InvoicedTickets.RootObject>(await responses[4].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var jobs = JsonConvert.DeserializeObject<Models.Jobs.RootObject>(await responses[5].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var laborUsed = JsonConvert.DeserializeObject<Models.LaborUsed.RootObject>(await responses[6].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var material = JsonConvert.DeserializeObject<Models.Material.RootObject>(await responses[7].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var materialUsed = JsonConvert.DeserializeObject<Models.MaterialUsed.RootObject>(await responses[8].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                var thirdPartyUsed = JsonConvert.DeserializeObject<Models.ThirdPartyUsed.RootObject>(await responses[9].Content.ReadAsStringAsync(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

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

                var batchSync = new List<Task<bool>>() {
                    SyncData1(savedTickets, tickets.D.Results),
                    SyncData2(savedCustomers, customers.D.Results),
                    SyncData3(savedEquipmentUnit, equipmentUnit.D.Results),
                    SyncData4(savedEquipmentUsed, equipmentUsed.D.Results),
                    SyncData5(savedInvoicedTickets, invoicedTickets.D.Results),
                    SyncData6(savedJobs, jobs.D.Results),
                    SyncData7(savedLaborUsed, laborUsed.D.Results),
                    SyncData8(savedMaterial, material.D.Results),
                    SyncData9(savedMaterialUsed, materialUsed.D.Results),
                    SyncData10(savedThirdPartyUsed, thirdPartyUsed.D.Results),
                };

                var doneSync = await Task.WhenAll(batchSync.ToArray());

            }
        }

    }
}