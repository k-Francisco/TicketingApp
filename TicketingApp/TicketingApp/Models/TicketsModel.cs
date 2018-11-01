using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using TicketingApp.Services;

namespace TicketingApp.Models.Tickets
{
    public class RootObject : SyncService<Ticket>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public class D
    {
        [JsonProperty("results")]
        public List<Ticket> Results { get; set; }
    }

    public class Ticket : RealmObject
    {
        //[JsonProperty("FileSystemObjectType")]
        //public int FileSystemObjectType { get; set; }

        //[JsonProperty("Id")]
        //public int Id { get; set; }

        //[JsonProperty("ServerRedirectedEmbedUrl")]
        //public string ServerRedirectedEmbedUrl { get; set; }

        //[JsonProperty("ContentTypeId")]
        //public string ContentTypeId { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("JobId")]
        public int JobId { get; set; }

        [JsonProperty("ApprovalId")]
        public int? ApprovalId { get; set; }

        [JsonProperty("InvoiceStatus")]
        public string InvoiceStatus { get; set; }

        [JsonProperty("WorkCompletedDescription")]
        public string WorkCompletedDescription { get; set; }

        [JsonProperty("ApprovedStatus")]
        public bool ApprovedStatus { get; set; }

        [JsonProperty("CommentsToOffice")]
        public string CommentsToOffice { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }

        [JsonProperty("CusomterName")]
        public string CusomterName { get; set; }

        [JsonProperty("CustRefNo")]
        public string CustRefNo { get; set; }

        [JsonProperty("CustContactName")]
        public string CustContactName { get; set; }

        [JsonProperty("CustContactEmail")]
        public string CustContactEmail { get; set; }

        [JsonProperty("CustContactNo")]
        public string CustContactNo { get; set; }

        [JsonProperty("JobName")]
        public string JobName { get; set; }

        [JsonProperty("JobLocation")]
        public string JobLocation { get; set; }

        [JsonProperty("JobAfe")]
        public string JobAfe { get; set; }

        [JsonProperty("JobMinor")]
        public int? JobMinor { get; set; }

        [JsonProperty("JobMajor")]
        public int? JobMajor { get; set; }

        [JsonProperty("JobPO")]
        public string JobPO { get; set; }

        [JsonProperty("JobType")]
        public string JobType { get; set; }

        [JsonProperty("ApproverComments")]
        public string ApproverComments { get; set; }

        [JsonProperty("InvoiceApproverId")]
        public int InvoiceApproverId { get; set; }

        [JsonProperty("TotalAmount")]
        public double TotalAmount { get; set; }

        [JsonProperty("TestAmount")]
        public double? TestAmount { get; set; }

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