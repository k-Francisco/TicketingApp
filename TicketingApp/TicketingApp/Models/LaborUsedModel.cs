using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

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
        [JsonProperty("EmployeeId")]
        public int EmployeeId { get; set; }
        [JsonProperty("EmployeeStringId")]
        public string EmployeeStringId { get; set; }
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
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
