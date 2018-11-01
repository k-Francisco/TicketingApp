using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.Models.Jobs
{
    public class RootObject : SyncService<Job>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Job> Results { get; set; }
    }
    public class Job : RealmObject
    {
        //[JsonProperty("FileSystemObjectType")]
        //public int FileSystemObjectType { get; set; }
        //[JsonProperty("Id")]
        //public int Id { get; set; }
        //[JsonProperty("ServerRedirectedEmbedUri")]
        //public object ServerRedirectedEmbedUri { get; set; }
        //[JsonProperty("ServerRedirectedEmbedUrl")]
        //public string ServerRedirectedEmbedUrl { get; set; }
        //[JsonProperty("ContentTypeId")]
        //public string ContentTypeId { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("CustomerId")]
        public int CustomerId { get; set; }
        [JsonProperty("JobStatus")]
        public string JobStatus { get; set; }
        [JsonProperty("LocationName")]
        public string LocationName { get; set; }
        [JsonProperty("RequestedStartDate")]
        public DateTimeOffset RequestedStartDate { get; set; }
        [JsonProperty("RequestedEndDate")]
        public DateTimeOffset RequestedEndDate { get; set; }
        [JsonProperty("OnSiteContactName")]
        public string OnSiteContactName { get; set; }
        [JsonProperty("OnSiteContactEmail")]
        public string OnSiteContactEmail { get; set; }
        [JsonProperty("OnSiteContactPhoneNo")]
        public string OnSiteContactPhoneNo { get; set; }
        [JsonProperty("AFENo")]
        public string AFENo { get; set; }
        [JsonProperty("JobDescription")]
        public string JobDescription { get; set; }
        [JsonProperty("Major")]
        public string Major { get; set; }
        [JsonProperty("Minor")]
        public string Minor { get; set; }
        [JsonProperty("CostCode")]
        public string CostCode { get; set; }
        [JsonProperty("PurchaseOrder")]
        public string PurchaseOrder { get; set; }
        [JsonProperty("OnSiteDepartmentName")]
        public string OnSiteDepartmentName { get; set; }
        [JsonProperty("Attention")]
        public string Attention { get; set; }
        [JsonProperty("JobId")]
        public string JobId { get; set; }
        [JsonProperty("CustomerReferenceNoId")]
        public int CustomerReferenceNoId { get; set; }
        [JsonProperty("JobType")]
        public string JobType { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }
        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }
        //[JsonProperty("AuthorId")]
        //public int AuthorId { get; set; }
        //[JsonProperty("EditorId")]
        //public int EditorId { get; set; }
        //[JsonProperty("OData__UIVersionString")]
        //public string ODataUIVersionString { get; set; }
        //[JsonProperty("Attachments")]
        //public bool Attachments { get; set; }
        [JsonProperty("GUID")]
        public string GUID { get; set; }
    }
}
