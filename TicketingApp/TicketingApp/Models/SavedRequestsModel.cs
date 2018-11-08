using Realms;
using System.Collections.Generic;

namespace TicketingApp.Models.SavedRequests
{
    public class SavedRequests : RealmObject
    {
        public string RequestBody { get; set; }

        public string TicketNumber { get; set; }
    }
}