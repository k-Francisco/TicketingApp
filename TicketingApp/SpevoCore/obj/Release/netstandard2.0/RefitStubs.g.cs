﻿// <auto-generated />
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Refit;
using System.Threading.Tasks;

/* ******** Hey You! *********
 *
 * This is a generated file, and gets rewritten every time you build the
 * project. If you want to edit it, you need to edit the mustache template
 * in the Refit package */

#pragma warning disable
namespace RefitInternalGenerated
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
    sealed class PreserveAttribute : Attribute
    {

        //
        // Fields
        //
        public bool AllMembers;

        public bool Conditional;
    }
}
#pragma warning restore

namespace SpevoCore.Services.Sharepoint_API
{
    using RefitInternalGenerated;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    [DebuggerNonUserCode]
    [Preserve]
    partial class AutoGeneratedISharepointAPI : ISharepointAPI        {
        /// <inheritdoc />
        public HttpClient Client { get; protected set; }
        readonly IRequestBuilder requestBuilder;

        /// <inheritdoc />
        public AutoGeneratedISharepointAPI(HttpClient client, IRequestBuilder requestBuilder)
        {
            Client = client;
            this.requestBuilder = requestBuilder;
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetCurrentUser()
        {
            var arguments = new object[] {  };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetCurrentUser", new Type[] {  });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetFormDigest()
        {
            var arguments = new object[] {  };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetFormDigest", new Type[] {  });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetSubSite(string subsite)
        {
            var arguments = new object[] { subsite };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetSubSite", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateSubsite(string formDigest,StringContent item)
        {
            var arguments = new object[] { formDigest,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateSubsite", new Type[] { typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteSubsite(string formDigest)
        {
            var arguments = new object[] { formDigest };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteSubsite", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListByTitle(string listTitle)
        {
            var arguments = new object[] { listTitle };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListByTitle", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListByGuid(string listGuid)
        {
            var arguments = new object[] { listGuid };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListByGuid", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> CreateList(string formDigest,StringContent item)
        {
            var arguments = new object[] { formDigest,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("CreateList", new Type[] { typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateListByListGuid(string formDigest,StringContent item,string listGuid)
        {
            var arguments = new object[] { formDigest,item,listGuid };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateListByListGuid", new Type[] { typeof(string),typeof(StringContent),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateListByListTitle(string formDigest,StringContent item,string listTitle)
        {
            var arguments = new object[] { formDigest,item,listTitle };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateListByListTitle", new Type[] { typeof(string),typeof(StringContent),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteListByListGuid(string formDigest,string listGuid)
        {
            var arguments = new object[] { formDigest,listGuid };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteListByListGuid", new Type[] { typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteListByListTitle(string formDigest,string listTitle)
        {
            var arguments = new object[] { formDigest,listTitle };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteListByListTitle", new Type[] { typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetField(string listGuid,string fieldId)
        {
            var arguments = new object[] { listGuid,fieldId };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetField", new Type[] { typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> CreateField(string formDigest,string listGuid,StringContent item)
        {
            var arguments = new object[] { formDigest,listGuid,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("CreateField", new Type[] { typeof(string),typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> CreateLookupField(string formDigest,string listGuid,StringContent item)
        {
            var arguments = new object[] { formDigest,listGuid,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("CreateLookupField", new Type[] { typeof(string),typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateField(string formDigest,string listGuid,string fieldId,StringContent item)
        {
            var arguments = new object[] { formDigest,listGuid,fieldId,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateField", new Type[] { typeof(string),typeof(string),typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteField(string formDigest,string listGuid,string fieldId)
        {
            var arguments = new object[] { formDigest,listGuid,fieldId };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteField", new Type[] { typeof(string),typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle)
        {
            var arguments = new object[] { listTitle };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListTitle", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle,string select)
        {
            var arguments = new object[] { listTitle,select };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListTitle", new Type[] { typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle,string select,string expand)
        {
            var arguments = new object[] { listTitle,select,expand };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListTitle", new Type[] { typeof(string),typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid)
        {
            var arguments = new object[] { listGuid };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListGuid", new Type[] { typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid,string select)
        {
            var arguments = new object[] { listGuid,select };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListGuid", new Type[] { typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid,string select,string expand)
        {
            var arguments = new object[] { listGuid,select,expand };
            var func = requestBuilder.BuildRestResultFuncForMethod("GetListItemsByListGuid", new Type[] { typeof(string),typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> AddListItemByListTitle(string formDigest,string listTitle,StringContent item)
        {
            var arguments = new object[] { formDigest,listTitle,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("AddListItemByListTitle", new Type[] { typeof(string),typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> AddListItemByListGuid(string formDigest,string listGuid,StringContent item)
        {
            var arguments = new object[] { formDigest,listGuid,item };
            var func = requestBuilder.BuildRestResultFuncForMethod("AddListItemByListGuid", new Type[] { typeof(string),typeof(string),typeof(StringContent) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateListItemByListTitle(string formDigest,string listTitle,StringContent item,string itemToBeReplacedId)
        {
            var arguments = new object[] { formDigest,listTitle,item,itemToBeReplacedId };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateListItemByListTitle", new Type[] { typeof(string),typeof(string),typeof(StringContent),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> UpdateListItemByListGuid(string formDigest,string listGuid,StringContent item,string itemToBeReplacedId)
        {
            var arguments = new object[] { formDigest,listGuid,item,itemToBeReplacedId };
            var func = requestBuilder.BuildRestResultFuncForMethod("UpdateListItemByListGuid", new Type[] { typeof(string),typeof(string),typeof(StringContent),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteListItemByListTitle(string formDigest,string listTitle,string itemToBeDeletedId)
        {
            var arguments = new object[] { formDigest,listTitle,itemToBeDeletedId };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteListItemByListTitle", new Type[] { typeof(string),typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

        /// <inheritdoc />
        public virtual Task<HttpResponseMessage> DeleteListItemByListGuid(string formDigest,string listGuid,string itemToBeDeletedId)
        {
            var arguments = new object[] { formDigest,listGuid,itemToBeDeletedId };
            var func = requestBuilder.BuildRestResultFuncForMethod("DeleteListItemByListGuid", new Type[] { typeof(string),typeof(string),typeof(string) });
            return (Task<HttpResponseMessage>)func(Client, arguments);
        }

    }
}
