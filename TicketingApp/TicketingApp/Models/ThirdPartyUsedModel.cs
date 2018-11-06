using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.Models.ThirdPartyUsed
{
    public class RootObject : SyncService<ThirdPartyUsed>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<ThirdPartyUsed> Results { get; set; }
    }
    public class ThirdPartyUsed : RealmObject
    {
        [JsonProperty("Vendor")]
        public Vendor Vendor { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("RequestDate")]
        public DateTimeOffset RequestDate { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Amount")]
        public double Amount { get; set; }
        [JsonProperty("MarkUp")]
        public int MarkUp { get; set; }
        [JsonProperty("Billable")]
        public bool Billable { get; set; }
        [JsonProperty("ChargeType")]
        public string ChargeType { get; set; }
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }
        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }

        public string Total {
            get { return String.Format("${0}", Amount); }
        }
    }
    
    public class Vendor : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}
