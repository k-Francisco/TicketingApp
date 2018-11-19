using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketingApp.Models.EquipmentUsed
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public class D
    {
        [JsonProperty("results")]
        public List<EquipmentUsed> Results { get; set; }
    }

    public class EquipmentUsed : RealmObject
    {
        [JsonProperty("Rate")]
        public string Rate { get; set; }

        [JsonProperty("Billable")]
        public bool Billable { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [JsonProperty("TicketId")]
        public int TicketId { get; set; }

        [JsonProperty("EquipmentId")]
        public int EquipmentId { get; set; }

        [JsonProperty("EquipmentType")]
        public string EquipmentType { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("GUID")]
        public string GUID { get; set; }

        public string EquipmentName
        {
            get
            {
                var equipment = Realm.GetInstance().All<EquipmentUnit.EquipmentUnit>()
                                .Where(e => e.ID == EquipmentId)
                                .FirstOrDefault();

                return equipment.Title;
            }
        }

        public string UnitNumber
        {
            get
            {
                var equipment = Realm.GetInstance().All<EquipmentUnit.EquipmentUnit>()
                                .Where(e => e.ID == EquipmentId)
                                .FirstOrDefault();

                return equipment.ModelNumber;
            }
        }

        public string Total
        {
            get { return String.Format("${0}", Convert.ToString(Quantity * Convert.ToInt32(Rate))); }
        }
    }
}