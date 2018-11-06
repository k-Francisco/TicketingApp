using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.Models.Material
{
    public class RootObject : SyncService<Material>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Material> Results { get; set; }
    }
    public class Material : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("MaterialDescription")]
        public string MaterialDescription { get; set; }
        [JsonProperty("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [JsonProperty("Rate")]
        public string Rate { get; set; }
        [JsonProperty("DiscountedRate")]
        public string DiscountedRate { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }
        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }
        [JsonProperty("GUID")]
        public string GUID { get; set; }
    }

}
