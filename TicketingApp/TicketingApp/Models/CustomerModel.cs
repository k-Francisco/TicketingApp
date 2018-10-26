using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using TicketingApp.Services;

namespace TicketingApp.Models.Customers
{
    public class RootObject : SyncService<Customer>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }

    public class D
    {
        [JsonProperty("results")]
        public List<Customer> Results { get; set; }
    }

    public class Customer : RealmObject
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
        [JsonProperty("CustReferenceNo")]
        public string CustReferenceNo { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("CustomerContactName")]
        public string CustomerContactName { get; set; }

        [JsonProperty("CustomerContactPhoneNo")]
        public string CustomerContactPhoneNo { get; set; }

        [JsonProperty("BillingAddress")]
        public string BillingAddress { get; set; }

        [JsonProperty("CustomerContactEmail")]
        public string CustomerContactEmail { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

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