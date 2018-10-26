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
        [JsonProperty("EquipmentUnitIDId")]
        public string EquipmentUnitIDId { get; set; }
        [JsonProperty("ChargeType")]
        public string ChargeType { get; set; }
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
        [JsonProperty("AttachmentId")]
        public string AttachmentId { get; set; }
        [JsonProperty("VendorId")]
        public int VendorId { get; set; }
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
