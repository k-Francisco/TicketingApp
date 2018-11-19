using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;

namespace TicketingApp.Models.Customers
{
    public class RootObject
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

        [JsonProperty("GUID")]
        public string GUID { get; set; }
    }
}