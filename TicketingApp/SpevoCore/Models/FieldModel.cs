using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SpevoCore.Models
{
    public abstract class FieldModel
    {
        
    }

    public class CalculatedFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Formula")]
        public string Formula { get; set; }
        [JsonProperty("OutputType")]
        public int OutputType { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }

    }

    public class DateTimeFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Required")]
        public bool Required { get; set; }
        [JsonProperty("DisplayFormat")]
        public int DisplayFormat { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class NumberFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Required")]
        public bool Required { get; set; }
        [JsonProperty("Minimum%20Value")]
        public int MinimumValue { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class TextFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Required")]
        public bool Required { get; set; }
        [JsonProperty("MaxLength")]
        public string MaxLength { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }

    }

    public class MultiLineTextFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Required")]
        public bool Required { get; set; }
        [JsonProperty("NumberOfLines")]
        public int NumberOfLines { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class BooleanFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class LookupFieldModel
    {
        [JsonProperty("parameters")]
        public Parameters LookupParameters { get; set; }
        
        public class Parameters
        {
            [JsonProperty("__metadata")]
            public Metadata Metadata { get; set; }
            [JsonProperty("FieldTypeKind")]
            public int FieldTypeKind { get; set; }
            [JsonProperty("Title")]
            public string Title { get; set; }
            [JsonProperty("LookupListId")]
            public string LookupListId { get; set; }
            [JsonProperty("LookupFieldName")]
            public string LookupFieldName { get; set; }
        }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class UserFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("SelectionGroup")]
        public int SelectionGroup { get; set; }
        [JsonProperty("SelectionMode")]
        public int SelectionMode { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class ChoiceFieldModel
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FieldTypeKind")]
        public int FieldTypeKind { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Choices")]
        public Choices Choices { get; set; }
        [JsonProperty("FillInChoice")]
        public bool FillInChoice { get; set; }
        [JsonProperty("DefaultValue")]
        public string DefaultValue { get; set; }

        public StringContent GetStringContent()
        {
            var item = new StringContent(JsonConvert.SerializeObject(this).Replace("\"", "'"));
            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return item;
        }
    }

    public class Choices
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("results")]
        public List<string> Results { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    
}
