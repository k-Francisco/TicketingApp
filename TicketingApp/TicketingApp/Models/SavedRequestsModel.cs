using Realms;
using TicketingApp.Models.Tickets;

namespace TicketingApp.Models.SavedRequests
{
    public class SavedRequests : RealmObject
    {
        public string TicketNumber { get; set; }
        public Ticket SavedTicket { get; set; }
        public string LaborUsedCollection { get; set; }
        public string EquipmentUsedCollection { get; set; }
        public string MaterialUsedCollection { get; set; }
        public string ThirdPartyUsedCollection { get; set; }
        public string CostCode { get; set; }
        public string TotalCollection { get; set; }
        public string Signature { get; set; }
        public string UserId { get; set; }
        public string InvoiceCount { get; set; }
    }
}