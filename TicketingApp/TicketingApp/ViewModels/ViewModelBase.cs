using Acr.UserDialogs;
using Fusillade;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Realms;
using SpevoCore.Services.API_Service;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TicketingApp.Models.EquipmentUnit;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.LaborUsed;
using TicketingApp.Models.Material;
using TicketingApp.Models.MaterialUsed;
using TicketingApp.Models.SavedRequests;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IUserDialogs PageDialog { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }
        protected IApiManager ApiManager { get; set; }
        protected IApiService<ISharepointAPI> SharepointApi = new ApiService<ISharepointAPI>(App.SiteUrl);
        protected Realm realm { get; private set; }
        protected bool connected { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public ViewModelBase(INavigationService navigationService, IEventAggregator eventAggregator, IApiManager apiManager)
        {
            RealmConfiguration.DefaultConfiguration.SchemaVersion = 1;

            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            ApiManager = apiManager;

            realm = Realm.GetInstance();
            PageDialog = UserDialogs.Instance;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                connected = true;

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    connected = true;
                else
                    connected = false;
            };
        }

        public void SyncData<U>(List<U> oldData, List<U> newData) where U : RealmObject
        {
            try
            {
                realm.Write(() =>
                {
                    realm.RemoveAll<U>();
                    oldData.Clear();

                    foreach (var item in newData)
                    {
                        oldData.Add(item);
                        realm.Add(item);
                    }
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SyncDataError_" + nameof(U), e.Message);
            }
        }

        #region invoice region

        public async void UpdateTicket(string formDigest, string signature, string ticketId)
        {
            try
            {
                if (connected)
                {
                    bool approvalStatus = false;
                    string status = "Open";

                    if (signature != null && !string.IsNullOrEmpty(signature))
                    {
                        status = "Approved";
                        approvalStatus = true;
                    }

                    var builder = new StringBuilder();
                    builder.Append("{'__metadata':{'type':'SP.Data.TicketsListItem'},");

                    builder.Append("'ApprovedStatus':'" + approvalStatus + "',");
                    builder.Append("'Status':'" + status + "'");
                    builder.Append("}");

                    var body = builder.ToString();

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var updateTix = await ApiManager.AddTask(SharepointApi.GetApi(Priority.UserInitiated).UpdateListItemByListTitle(formDigest, "Tickets", item, ticketId));

                    var ensure = updateTix.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine("update tix", "success");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("update tix", "failed");
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("update tix error", e.Message);
            }
        }

        public List<string> GetInvoiceBody(SavedRequests savedRequests)
        {
            var metadata = new List<string>();
            string status, invoiceNumber;

            try
            {
                invoiceNumber = savedRequests.SavedTicket.Title.Replace("TN", "IN") + savedRequests.InvoiceCount;

                if (savedRequests.Signature != null || !string.IsNullOrWhiteSpace(savedRequests.Signature))
                    status = "Approved";
                else
                    status = "Open";

                var invoiceHtml = GetInvoiceHTML(savedRequests, invoiceNumber);

                metadata.Add("{'__metadata':{'type':'SP.Data.InvoicedTicketsListItem'},");
                metadata.Add("'TicketID':'" + savedRequests.SavedTicket.ID + "',");
                metadata.Add("'TicketNumber':'" + savedRequests.SavedTicket.Title + "',");
                metadata.Add("'InvoiceVersion':'" + savedRequests.InvoiceCount + "',");
                metadata.Add("'InvoiceHTML':'" + invoiceHtml + "',");
                metadata.Add("'InvoiceNumber':'" + invoiceNumber + "',");
                metadata.Add("'Status':'" + status + "',");
                metadata.Add("'Trigger':'generatePDF',");

                if (savedRequests.Signature != null || !string.IsNullOrWhiteSpace(savedRequests.Signature))
                    metadata.Add("'SignatureCode':'" + savedRequests.Signature + "',");

                metadata.Add("'ResponseById':'" + savedRequests.UserId + "'");
                metadata.Add("}");

                return metadata;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("invoicebodyerror", e.Message);
                return null;
            }
        }

        private string GetInvoiceHTML(SavedRequests savedRequests, string invoiceNumber)
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("<!DOCTYPE html><body>");

                builder.Append(GetTopSection(invoiceNumber, savedRequests.SavedTicket, savedRequests.CostCode));
                builder.Append(GetLaborUsedSection(savedRequests.LaborUsedCollection));
                builder.Append(GetEquipmentUsedSection(savedRequests.EquipmentUsedCollection));
                builder.Append(GetMaterialUsedSection(savedRequests.MaterialUsedCollection));
                builder.Append(GetThirdPartyUsedSection(savedRequests.ThirdPartyUsedCollection));
                builder.Append(GetSumOfChargesSection(savedRequests.TotalCollection));
                builder.Append(GetSignatureSection(savedRequests.Signature));

                builder.Append("</body></html>");

                return builder.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("invoicehtmlerror", e.Message);
                return string.Empty;
            }
        }

        private string GetTopSection(string invoiceNumber, Ticket savedTicket, string costCode)
        {
            string top = "<div class=\"modal-dialog\" role=\"document\" style=\"width: 80%\">" +
                    "<div class=\"modal-content\" id=\"pdfContent\">" +
                        "<div class=\"modal-header\" id=\"invoiceHeader\">" +
                          "<table style = \"width: 100%;color: #ffffff;\">" +
                            "<tbody><tr>" +
                              "<td rowspan=\"2\"><h3 id = \"companyName\" >" + savedTicket.CusomterName + "</h3></td>" +
                               "<td ><p id=\"invoiceNumber\" class=\"pull-right\">Invoice Number: " + invoiceNumber + "</p></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td><p id = \"invoiceDate\" class=\"pull-right\">Date: " + DateTime.Now.ToString("MMMM dd, yyyy") + "</p></td>" +
                            "</tr>" +
                          "</tbody></table>" +
                        "</div>" +
                        "<div class=\"modal-body\" id=\"pdfContentBody\">" +
                            "<div class=\"container-fluid\">" +
                                "<div class=\"row\" style=\"padding-top: 10px; margin-left: 20px;margin-right: 20px;\">" +
                                    "<table style = \"width:100%;\" >" +
                                      "<tbody><tr>" +
                                        "<td> Bill To:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"billTo\" style=\"width: 35%;\">" + savedTicket.CusomterName + "</td>" +
                                        "<td>&nbsp;&nbsp;AFE:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"AFE\">" + savedTicket.JobAfe + "</td>" +
                                      "</tr>" +
                                      "<tr>" +
                                        "<td>Location:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"location\">" + savedTicket.JobLocation + "</td>" +
                                        "<td>&nbsp;&nbsp;Code:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"code\">" + costCode + "</td>" +
                                      "</tr>" +
                                      "<tr>" +
                                        "<td>Contact:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"contact\">" + savedTicket.CustContactName + "</td>" +
                                        "<td>&nbsp;&nbsp;Contact Email:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"contactEmail\">" + savedTicket.CustContactEmail + "</td>" +
                                      "</tr>" +
                                    "</tbody></table>" +
                                "</div>";

            return top;
        }

        private string GetLaborUsedSection(string laborUsedCollection)
        {
            var laborUsed = JsonConvert.DeserializeObject<List<LaborUsed>>(laborUsedCollection);

            var builder = new StringBuilder();
            for (int i = 0; i < laborUsed.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                var total = (laborUsed[i].STHours * 100) + (laborUsed[i].OTHours * 150);

                builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + laborUsed[i].Employee.Title +
                    "</td><td>" + laborUsed[i].WorkType + "</td><td>" + laborUsed[i].STHours.ToString() +
                    "</td><td>$100</td><td>" + laborUsed[i].OTHours.ToString() + "</td><td>$150</td><td>$" + total.ToString() + "</td></tr>");
            }
            string sectio = "<div class=\"panel - group\" style=\"padding - top: 10px; margin - left: 5px; margin - right: 5px; \">" +
                         "<div class=\"col-md-12\" id=\"usedSummary\">" +
                                "<div class=\"panel panel-default\" id=\"labourUsed\">" +
                                    "<div class=\"panel-heading panel-heading-style\">Labour Used</div>" +
                                    "<div class=\"panel-body\" id=\"labourDTB\"><tbody<tr></tbody<tr>" +
                                    "<div id = \"laborsUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"laborsUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 143px;\">" +
                                    "Full Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 129px;\">" +
                                    "Worker Type</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 161px;\">" +
                                    "Standard Hours</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 84px;\">" +
                                    "ST Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 167px;\">" +
                                    "Over-time Hours</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 86px;\">" +
                                    "OT Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 77px;\">" +
                                    "Total</th></tr></thead><tbody>" + builder.ToString() + "</tbody></table></div></div></div>";

            return sectio;
        }

        private string GetEquipmentUsedSection(string equipmentUsedCollection)
        {
            var equipmentUsed = JsonConvert.DeserializeObject<List<EquipmentUsed>>(equipmentUsedCollection);

            var builder = new StringBuilder();
            for (int i = 0; i < equipmentUsed.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                var total = Convert.ToInt32(equipmentUsed[i].Rate) * equipmentUsed[i].Quantity;

                //todo: expand the equipment and change the model of equipmentUsed
                var equipment = realm.All<EquipmentUnit>()
                                .Where(e => e.ID == equipmentUsed[i].EquipmentId)
                                .FirstOrDefault();

                builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + equipment.Title + "</td><td>" + equipment.ModelNumber + "</td><td>" +
                    equipmentUsed[i].UnitOfMeasure + "</td><td>" + equipmentUsed[i].Quantity.ToString() +
                    "</td><td>$" + equipmentUsed[i].Rate + "</td><td>$" + total.ToString() + "</td></tr>");
            }

            string section = "<div class=\"panel panel-default\" id=\"equipmentUsed\">" +
                                    "<div class=\"panel-heading panel-heading-style\">Equipment Used</div>" +
                                    "<div class=\"panel-body\" id=\"equipmentDTB\"><div id = \"equipmentUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"equipmentUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 238px;\">" +
                                    "Equipment Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 121px;\">" +
                                    "Unit No.</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 221px;\">" +
                                    "Unit of Measure</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 127px;\">" +
                                    "Quantity</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 77px;\">" +
                                    "Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 81px;\">" +
                                    "Total</th></tr></thead><tbody>" + builder.ToString() + "</tbody></table></div></div></div>";

            return section;
        }

        private string GetMaterialUsedSection(string materialUsedCollection)
        {
            var materialUsed = JsonConvert.DeserializeObject<List<MaterialUsed>>(materialUsedCollection);

            var builder = new StringBuilder();
            for (int i = 0; i < materialUsed.Count; i++)
            {
                if (materialUsed[i].Billable)
                {
                    var oddOrEven = "odd";
                    if (i % 2 == 0)
                        oddOrEven = "even";

                    var total = Convert.ToInt32(materialUsed[i].Rate) * materialUsed[i].QuantityUsed;

                    //todo: expand like the labor used
                    var material = realm.All<Material>()
                                    .Where(e => e.ID == materialUsed[i].MaterialId)
                                    .FirstOrDefault();
                    builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + material.Title + "</td><td>" + materialUsed[i].QuantityUsed.ToString() +
                        "</td><td>" + materialUsed[i].UnitOfMeasure + "</td><td>$" + materialUsed[i].Rate + "</td><td>$" + total.ToString() + "</td></tr>");
                }
            }

            string section = "<div class=\"panel panel-default\" id=\"materialUsed\">" +
                                    "<div class=\"panel-heading panel-heading-style\">Material Used</div>" +
                                    "<div class=\"panel-body\" id=\"materialDTB\"><div id = \"materialUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"materialUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 176px;\">" +
                                    "Item Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 145px;\">" +
                                    "Quantity</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 252px;\">" +
                                    "Unit of Measure</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 216px;\">" +
                                    "Cost Per Unit</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 94px;\">" +
                                    "Total</th></tr></thead><tbody>" + builder.ToString() + "</tbody></table></div></div></div>";

            return section;
        }

        private string GetThirdPartyUsedSection(string thirdPartyUsedCollection)
        {
            var tpu = JsonConvert.DeserializeObject<List<ThirdPartyUsed>>(thirdPartyUsedCollection);

            var builder = new StringBuilder();
            for (int i = 0; i < tpu.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + tpu[i].Vendor.Title +
                    "</td><td>$" + tpu[i].Amount.ToString() + "</td><td>" + tpu[i].MarkUp +
                    "</td><td>$" + tpu[i].Amount.ToString() + "</td><td>$" + tpu[i].Amount.ToString() + "</td></tr>");
            }
            string section = "<div class=\"panel panel-default\" id=\"thirdParty\">" +
                                    "<div class=\"panel-heading panel-heading-style\">3<sup>rd</sup> Party</div>" +
                                    "<div class=\"panel-body\" id=\"otherPartyDTB\"><div id = \"opUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"opUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 231px;\">" +
                                    "Vendor Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 226px;\">" +
                                    "Original Cost</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 138px;\">" +
                                    "Markup</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 187px;\">" +
                                    "Final Price</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 101px;\">" +
                                    "Total</th></tr></thead><tbody>" + "</tbody></table></div></div></div></div>";

            return section;
        }

        private string GetSumOfChargesSection(string totalCollection)
        {
            var totals = JsonConvert.DeserializeObject<List<double>>(totalCollection);
            /*
             * [0] = labor used total
             * [1] = equipment used total
             * [2] = material used total
             * [3] = third party used total
            */
            var total = totals[0] + totals[1] + totals[2] + totals[3];
            string section = "<div class=\"col - md - 6\" id=\"summOfchargesDiv\">" +
                                    "<div class=\"panel panel-default\">" +
                                    "<div class=\"panel-heading panel-heading-style\">Summary of Charges</div>" +
                                    "<table id = \"summOfcharges\" >" +
                                        "<tbody><tr>" +
                                        "</tr>" +
                                        "<tr id=\"labourUsed\">" +
                                            "<td class=\"tdLabels\">Labour Charges</td>" +
                                            "<td id = \"labourTotal\" >$" + totals[0] + "</td>" +
                                        "</tr>" +
                                        "<tr id = \"equipmentUsed\" >" +
                                            "<td class=\"tdLabels\">Equipment Charges</td>" +
                                            "<td id = \"equipmentTotal\" >$" + totals[1] + "</td>" +
                                        "</tr>" +
                                        "<tr id = \"materialUsed\">" +
                                             "<td class=\"tdLabels\">Material Charges</td>" +
                                            "<td id = \"materialTotal\" >$" + totals[2] + "</td>" +
                                        "</tr>" +
                                        "<tr id = \"thirdParty\" >" +
                                              "<td class=\"tdLabels\">3<sup>rd</sup> Party Charges</td>" +
                                            "<td id = \"thirdPartyTotal\" >$" + totals[3] + "</td>" +
                                        "</tr>" +
                                        "<tr id = \"totalCharges\" >" +
                                            "<td > Total </td >" +
                                            "<td id=\"summaryChargesTotal\">$" + total + "</td>" +
                                        "</tr>" +
                                    "</tbody></table>" +
                                "</div>" +
                            "</div>";

            return section;
        }

        private string GetSignatureSection(string signature)
        {
            if (signature != null)
            {
                string section = "<div class=\"col - md - 6\" id=\"signatureStamp\">" +
                                        "Customer Signature Stamp" +
                                        "<img src = \"" + signature + "\" id=\"sign_prev\" style=\"\" width=\"382px\" height=\"286px\">" +
                                        "<div id = \"sign_prev_div\" ></ div >" +
                                         "<button type = \"button\" class=\"signatureStamp hidden\" onclick=\"loadSignatureForm()\" id=\"btnAddSign\">" +
                                            "<h4 id = \"signaturePlaceholder\" > Add Signature / Stamp</h4>" +
                                        "</button></div></div></div></div></div></div>";

                return section;
            }
            return string.Empty;
        }

        #endregion invoice region

        #region boilerplates

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        #endregion boilerplates
    }
}