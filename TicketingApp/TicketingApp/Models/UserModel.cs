using Realms;

namespace TicketingApp.Models.Users
{
    public class User : RealmObject
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public bool IsSiteAdmin { get; set; }
    }
}