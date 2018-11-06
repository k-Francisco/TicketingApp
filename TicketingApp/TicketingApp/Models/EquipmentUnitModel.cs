using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using TicketingApp.Services;

namespace TicketingApp.Models.EquipmentUnit
{
    public class RootObject : SyncService<EquipmentUnit>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public class D
    {
        [JsonProperty("results")]
        public List<EquipmentUnit> Results { get; set; }
    }

    public class EquipmentUnit : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("EquipmentUnitName")]
        public string EquipmentUnitName { get; set; }

        [JsonProperty("Rate")]
        public int Rate { get; set; }

        [JsonProperty("DiscountedRate")]
        public int DiscountedRate { get; set; }

        [JsonProperty("ModelNumber")]
        public string ModelNumber { get; set; }

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