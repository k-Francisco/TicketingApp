using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;

namespace TicketingApp.Models.LaborUsed
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public class D
    {
        [JsonProperty("results")]
        public List<LaborUsed> Results { get; set; }
    }

    public class LaborUsed : RealmObject
    {
        [JsonProperty("Employee")]
        public Employee Employee { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("WorkType")]
        public string WorkType { get; set; }

        [JsonProperty("STHours")]
        public int STHours { get; set; }

        [JsonProperty("OTHours")]
        public int OTHours { get; set; }

        [JsonProperty("PerDiem")]
        public bool PerDiem { get; set; }

        [JsonProperty("Billable")]
        public bool Billable { get; set; }

        [JsonProperty("TicketId")]
        public int TicketId { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("GUID")]
        public string GUID { get; set; }

        public string Total
        {
            get { return String.Format("${0}", Convert.ToString((100 * STHours) + (150 * OTHours))); }
        }
    }

    public class Employee : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}