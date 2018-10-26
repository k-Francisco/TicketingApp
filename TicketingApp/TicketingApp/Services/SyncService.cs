using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace TicketingApp.Services
{
    public class SyncService<T> : ISyncService<T>
        where T : RealmObject
    {
        Realm realm = Realm.GetInstance();
        public async Task<bool> SyncData(List<T> oldData, List<T> newData)
        {
            try
            {
                realm.Write(() =>
                {
                    realm.RemoveAll<T>();
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
                System.Diagnostics.Debug.WriteLine("SyncDataError_" + nameof(T), e.Message);
                return false;
            }
        }
    }
}
