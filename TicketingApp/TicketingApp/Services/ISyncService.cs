using Realms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketingApp.Services
{
    public interface ISyncService<T>
        where T : RealmObject
    {
        Task<bool> SyncData(List<T> oldData, List<T> newData);
    }
}