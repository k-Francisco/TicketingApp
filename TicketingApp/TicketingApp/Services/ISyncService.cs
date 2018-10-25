using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TicketingApp.Services
{
    public interface ISyncService<A,B,C,D,E,F,G,H,I,J>
        where A : RealmObject
        where B : RealmObject
        where C : RealmObject
        where D : RealmObject
        where E : RealmObject
        where F : RealmObject
        where G : RealmObject
        where H : RealmObject
        where I : RealmObject
        where J : RealmObject
    {
        Task<bool> SyncData1(List<A> oldData, List<A> newData);
        Task<bool> SyncData2(List<B> oldData, List<B> newData);
        Task<bool> SyncData3(List<C> oldData, List<C> newData);
        Task<bool> SyncData4(List<D> oldData, List<D> newData);
        Task<bool> SyncData5(List<E> oldData, List<E> newData);
        Task<bool> SyncData6(List<F> oldData, List<F> newData);
        Task<bool> SyncData7(List<G> oldData, List<G> newData);
        Task<bool> SyncData8(List<H> oldData, List<H> newData);
        Task<bool> SyncData9(List<I> oldData, List<I> newData);
        Task<bool> SyncData10(List<J> oldData, List<J> newData);
    }
}
