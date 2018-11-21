using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpevoCore.Services.Sharepoint_API
{
    public interface ISharepointAPI
    {
        [Get("/_api/web/currentuser?")]
        Task<HttpResponseMessage> GetCurrentUser();

        [Post("/_api/contextinfo")]
        Task<HttpResponseMessage> GetFormDigest();

        [Get("/{subsite}/_api/web")]
        Task<HttpResponseMessage> GetSubSite(string subsite);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:MERGE")]
        [Post("/_api/web")]
        Task<HttpResponseMessage> UpdateSubsite([Header("X-RequestDigest")] string formDigest,
                                                StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:DELETE")]
        [Post("/_api/web")]
        Task<HttpResponseMessage> DeleteSubsite([Header("X-RequestDigest")] string formDigest);

        [Get("/_api/web/lists/getbytitle('{listTitle}')")]
        Task<HttpResponseMessage> GetListByTitle(string listTitle);

        [Get("/_api/web/lists(guid'{listGuid}')")]
        Task<HttpResponseMessage> GetListByGuid(string listGuid);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists")]
        Task<HttpResponseMessage> CreateList([Header("X-RequestDigest")] string formDigest,
                                             StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "IF-MATCH:*",
                 "X-HTTP-Method:MERGE")]
        [Post("/_api/web/lists(guid'{listGuid}')")]
        Task<HttpResponseMessage> UpdateListByListGuid([Header("X-RequestDigest")] string formDigest,
                                             StringContent item,
                                             string listGuid);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "IF-MATCH:*",
                 "X-HTTP-Method:MERGE")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')")]
        Task<HttpResponseMessage> UpdateListByListTitle([Header("X-RequestDigest")] string formDigest,
                                             StringContent item,
                                             string listTitle);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists(guid'{listGuid}')")]
        Task<HttpResponseMessage> DeleteListByListGuid([Header("X-RequestDigest")] string formDigest,
                                             string listGuid);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')")]
        Task<HttpResponseMessage> DeleteListByListTitle([Header("X-RequestDigest")] string formDigest,
                                             string listTitle);

        [Get("/_api/web/lists(guid'{listGuid}')/fields('{fieldId}')")]
        Task<HttpResponseMessage> GetField(string listGuid, string fieldId);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists(guid'{listGuid}')/Fields")]
        Task<HttpResponseMessage> CreateField([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists(guid'{listGuid}')/Fields/addfield")]
        Task<HttpResponseMessage> CreateLookupField([Header("X-RequestDigest")] string formDigest,
                                                            string listGuid,
                                                            StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                "Content-Type:application/json;odata=verbose",
                "X-HTTP-Method:MERGE")]
        [Post("/_api/web/lists(guid'{listGuid}')/fields('{fieldId}')")]
        Task<HttpResponseMessage> UpdateField([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              string fieldId,
                                              StringContent item);

        [Headers("X-HTTP-Method:DELETE")]
        [Post("/_api/web/lists(guid'{listGuid}')/fields('{fieldId}')")]
        Task<HttpResponseMessage> DeleteField([Header("X-ReqeustDigest")] string formDigest,
                                              string listGuid,
                                              string fieldId);

        [Get("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle);

        [Get("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle,
                                                          [AliasAs("$select")] string select);

        [Get("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle,
                                                          [AliasAs("$select")] string select,
                                                          [AliasAs("$expand")] string expand);

        [Get("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid);

        [Get("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid,
                                                         [AliasAs("$select")] string select);

        [Get("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid,
                                                         [AliasAs("$select")] string select,
                                                         [AliasAs("$expand")] string expand);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> AddListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                  string listTitle,
                                  StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> AddListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                           string listGuid,
                                           StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:MERGE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items({itemToBeReplacedId})")]
        Task<HttpResponseMessage> UpdateListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                     string listTitle,
                                     StringContent item,
                                     string itemToBeReplacedId);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:MERGE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists(guid'{listGuid}')/items({itemToBeReplacedId})")]
        Task<HttpResponseMessage> UpdateListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              StringContent item,
                                              string itemToBeReplacedId);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items({itemToBeDeletedId})")]
        Task<HttpResponseMessage> DeleteListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                    string listTitle,
                                    string itemToBeDeletedId);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists(guid'{listGuid}')/items({itemToBeDeletedId})")]
        Task<HttpResponseMessage> DeleteListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              string itemToBeDeletedId);
    }
}