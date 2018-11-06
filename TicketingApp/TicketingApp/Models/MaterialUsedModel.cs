using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.Models.MaterialUsed
{
    public class RootObject : SyncService<MaterialUsed>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<MaterialUsed> Results { get; set; }
    }
    public class MaterialUsed : RealmObject
    {
        [JsonProperty("QuantityUsed")]
        public int QuantityUsed { get; set; }
        [JsonProperty("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [JsonProperty("Billable")]
        public bool Billable { get; set; }
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
        [JsonProperty("MaterialId")]
        public int MaterialId { get; set; }
        [JsonProperty("Rate")]
        public string Rate { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }
        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }
        [JsonProperty("GUID")]
        public string GUID { get; set; }

        public string ItemName {
            get {
                var material = Realm.GetInstance().All<Material.Material>()
                               .Where(m => m.ID == MaterialId)
                               .FirstOrDefault();

                return material.Title;
            }
        }

        public string Total {
            get { return String.Format("${0}", Convert.ToString(Convert.ToInt32(Rate) * QuantityUsed)); }
        }
    }

}
