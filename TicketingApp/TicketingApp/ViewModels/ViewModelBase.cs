using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Realms;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, ISyncService<Ticket, Customer, EquipmentUnit, EquipmentUsed, InvoicedTickets, Job, LaborUsed, Material, MaterialUsed, ThirdPartyUsed>
    {
        protected INavigationService NavigationService { get; private set; }
        protected ISharepointAPI SharepointAPI { get; private set; }
        protected Realm realm;
        protected bool connected;

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

        public async Task<bool> SyncData1(List<Ticket> oldData, List<Ticket> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<Ticket>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine("tickets", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData2(List<Customer> oldData, List<Customer> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<Customer>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("customer", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData3(List<EquipmentUnit> oldData, List<EquipmentUnit> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<EquipmentUnit>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("equipmentunit", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData4(List<EquipmentUsed> oldData, List<EquipmentUsed> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<EquipmentUsed>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("equipmentused", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData5(List<InvoicedTickets> oldData, List<InvoicedTickets> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<InvoicedTickets>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("invoice", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData6(List<Job> oldData, List<Job> newData)
        {
            try
            {
                realm.Write(() =>
                   {
                       realm.RemoveAll<Job>();
                       oldData.Clear();

                       foreach (var item in newData)
                       {
                           oldData.Add(item);
                           realm.Add(item);
                       }

                   });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Job", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData7(List<LaborUsed> oldData, List<LaborUsed> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<LaborUsed>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("labor used", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData8(List<Material> oldData, List<Material> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<Material>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("material", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData9(List<MaterialUsed> oldData, List<MaterialUsed> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<MaterialUsed>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("materialused", e.Message);
                return false;
            }
        }

        public async Task<bool> SyncData10(List<ThirdPartyUsed> oldData, List<ThirdPartyUsed> newData)
        {
            try
            {
                realm.Write(() =>
                    {
                        realm.RemoveAll<ThirdPartyUsed>();
                        oldData.Clear();

                        foreach (var item in newData)
                        {
                            oldData.Add(item);
                            realm.Add(item);
                        }

                    });

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("tpu", e.Message);
                return false;
            }
        }
    }
}
