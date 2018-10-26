using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
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
        //[JsonProperty("Title")]
        //public object Title { get; set; }
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
