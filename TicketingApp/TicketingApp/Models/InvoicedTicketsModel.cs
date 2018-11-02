using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using TicketingApp.Services;

namespace TicketingApp.Models.InvoicedTickets
{
    public class RootObject : SyncService<InvoicedTickets>
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
        [JsonProperty("Title")]
        public string Title { get; set; }

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

        [JsonProperty("GUID")]
        public string GUID { get; set; }
    }
}