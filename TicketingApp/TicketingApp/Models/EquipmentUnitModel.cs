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
        [JsonProperty("FileSystemObjectType")]
        public int FileSystemObjectType { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        //[JsonProperty("ServerRedirectedEmbedUri")]
        //public object ServerRedirectedEmbedUri { get; set; }
        [JsonProperty("ServerRedirectedEmbedUrl")]
        public string ServerRedirectedEmbedUrl { get; set; }

        [JsonProperty("ContentTypeId")]
        public string ContentTypeId { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("EquipmentUnitName")]
        public string EquipmentUnitName { get; set; }

        [JsonProperty("Rate")]
        public int Rate { get; set; }

        [JsonProperty("DiscountedRate")]
        public int DiscountedRate { get; set; }

        //[JsonProperty("EquipmentType")]
        //public object EquipmentType { get; set; }
        [JsonProperty("ModelNumber")]
        public string ModelNumber { get; set; }

        //[JsonProperty("UoM")]
        //public object UoM { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("AuthorId")]
        public int AuthorId { get; set; }

        [JsonProperty("EditorId")]
        public int EditorId { get; set; }

        [JsonProperty("OData__UIVersionString")]
        public string ODataUIVersionString { get; set; }

        [JsonProperty("Attachments")]
        public bool Attachments { get; set; }

        [JsonProperty("GUID")]
        public string GUID { get; set; }
    }
}