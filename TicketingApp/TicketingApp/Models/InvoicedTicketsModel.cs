using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingApp.Models.InvoicedTickets
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<InvoicedTickets> Results { get; set; }
    }
    public class InvoicedTickets : RealmObject
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
        //[JsonProperty("InvoiceIDId")]
        //public object InvoiceIDId { get; set; }
        [JsonProperty("InvoiceVersion")]
        public string InvoiceVersion { get; set; }
        [JsonProperty("TicketNumber")]
        public string TicketNumber { get; set; }
        [JsonProperty("TicketID")]
        public string TicketID { get; set; }
        [JsonProperty("InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [JsonProperty("InvoiceHTML")]
        public string InvoiceHTML { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("SignatureCode")]
        public string SignatureCode { get; set; }
        [JsonProperty("ResponseById")]
        public int ResponseById { get; set; }
        [JsonProperty("ResponseByStringId")]
        public string ResponseByStringId { get; set; }
        [JsonProperty("Trigger")]
        public string Trigger { get; set; }
        [JsonProperty("pdfStyles")]
        public string PdfStyles { get; set; }
        [JsonProperty("pdfFilePath")]
        public string PdfFilePath { get; set; }
        [JsonProperty("ApproverComments")]
        public string ApproverComments { get; set; }
        [JsonProperty("InvoiceLink")]
        public string InvoiceLink { get; set; }
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
