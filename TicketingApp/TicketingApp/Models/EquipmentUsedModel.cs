﻿using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.Models.EquipmentUsed
{
    public class RootObject : SyncService<EquipmentUsed>
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<EquipmentUsed> Results { get; set; }
    }
    public class EquipmentUsed : RealmObject
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
        //[JsonProperty("Title")]
        //public object Title { get; set; }
        [JsonProperty("Rate")]
        public string Rate { get; set; }
        [JsonProperty("Billable")]
        public bool Billable { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [JsonProperty("TicketId")]
        public int TicketId { get; set; }
        [JsonProperty("EquipmentId")]
        public int EquipmentId { get; set; }
        [JsonProperty("EquipmentType")]
        public string EquipmentType { get; set; }
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
