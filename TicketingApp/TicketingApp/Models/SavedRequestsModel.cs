using Realms;

namespace TicketingApp.Models.SavedRequests
{
    public class SavedRequests : RealmObject
    {
        public string requestBody { get; set; }
    }
}