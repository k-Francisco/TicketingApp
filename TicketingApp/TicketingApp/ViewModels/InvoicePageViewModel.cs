using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketingApp.Models.EquipmentUnit;
using TicketingApp.Models.EquipmentUsed;
using TicketingApp.Models.InvoicedTickets;
using TicketingApp.Models.Jobs;
using TicketingApp.Models.LaborUsed;
using TicketingApp.Models.Material;
using TicketingApp.Models.MaterialUsed;
using TicketingApp.Models.SavedRequests;
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using TicketingApp.Models.Users;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class InvoicePageViewModel : ViewModelBase
    {
        private ObservableCollection<LaborUsed> _laborUsed = new ObservableCollection<LaborUsed>();

        public ObservableCollection<LaborUsed> LaborUsed
        {
            get { return _laborUsed; }
            set { SetProperty(ref _laborUsed, value); }
        }

        private ObservableCollection<MaterialUsed> _materialUsed = new ObservableCollection<MaterialUsed>();

        public ObservableCollection<MaterialUsed> MaterialUsed
        {
            get { return _materialUsed; }
            set { SetProperty(ref _materialUsed, value); }
        }

        private ObservableCollection<EquipmentUsed> _equipmentUsed = new ObservableCollection<EquipmentUsed>();

        public ObservableCollection<EquipmentUsed> EquipmentUsed
        {
            get { return _equipmentUsed; }
            set { SetProperty(ref _equipmentUsed, value); }
        }

        private ObservableCollection<ThirdPartyUsed> _thirdPartyUsed = new ObservableCollection<ThirdPartyUsed>();

        public ObservableCollection<ThirdPartyUsed> ThirdPartyUsed
        {
            get { return _thirdPartyUsed; }
            set { SetProperty(ref _thirdPartyUsed, value); }
        }

        private string _costCode;
        public string CostCode
        {
            get { return _costCode; }
            set { SetProperty(ref _costCode, value); }
        }

        private Ticket _ticket;
        public Ticket Ticket
        {
            get { return _ticket; }
            set { SetProperty(ref _ticket, value); }
        }

        private int _laborUsedHeight;
        public int LaborUsedHeight
        {
            get { return _laborUsedHeight; }
            set { SetProperty(ref _laborUsedHeight, value); }
        }

        private int _equipmentUsedHeight;
        public int EquipmentUsedHeight
        {
            get { return _equipmentUsedHeight; }
            set { SetProperty(ref _equipmentUsedHeight, value); }
        }

        private int _materialUsedHeight;
        public int MaterialUsedHeight
        {
            get { return _materialUsedHeight; }
            set { SetProperty(ref _materialUsedHeight, value); }
        }

        private int _thirdPTUsedHeight;
        public int ThirdPTUsedHeight
        {
            get { return _thirdPTUsedHeight; }
            set { SetProperty(ref _thirdPTUsedHeight, value); }
        }

        private double _laborUsedTotal;
        public double LaborUsedTotal
        {
            get { return _laborUsedTotal; }
            set { SetProperty(ref _laborUsedTotal, value); }
        }

        private double _equipmentUsedTotal;
        public double EquipmentUsedTotal
        {
            get { return _equipmentUsedTotal; }
            set { SetProperty(ref _equipmentUsedTotal, value); }
        }

        private double _materialUsedTotal;
        public double MaterialUsedTotal
        {
            get { return _materialUsedTotal; }
            set { SetProperty(ref _materialUsedTotal, value); }
        }

        private double _thirdPTUsedTotal;
        public double ThirdPTUsedTotal
        {
            get { return _thirdPTUsedTotal; }
            set { SetProperty(ref _thirdPTUsedTotal, value); }
        }

        public Func<Task<byte[]>> SignatureFromStream { get; set; }

        private string _status = "Open";
        private bool _approvalStatus = false;

        public InvoicePageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI)
            : base(navigationService, sharepointAPI)
        {
            Title = "Invoice";

            Connectivity.ConnectivityChanged += (s, e) =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    System.Diagnostics.Debug.WriteLine("connect", "yeah");
                    CheckSavedRequests();
                }
            };
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                Ticket = parameters.GetValue<Ticket>("ticket");
                var job = parameters.GetValue<Job>("job");

                CostCode = job.CostCode;

                var savedLaborUsed = realm.All<LaborUsed>()
                                    .Where(l => l.TicketId == Ticket.ID)
                                    .ToList();
                foreach (var item in savedLaborUsed)
                {
                    LaborUsed.Add(item);
                    LaborUsedHeight += 170;
                    //todo: labor used total
                }

                var savedMaterialUsed = realm.All<MaterialUsed>()
                                        .Where(m => m.TicketId == Ticket.ID)
                                        .ToList();

                foreach (var item in savedMaterialUsed)
                {
                    MaterialUsed.Add(item);
                    MaterialUsedHeight += 250;
                    //todo: material used total
                }

                var savedEquipmentUsed = realm.All<EquipmentUsed>()
                                        .Where(e => e.TicketId == Ticket.ID)
                                        .ToList();

                foreach (var item in savedEquipmentUsed)
                {
                    EquipmentUsed.Add(item);
                    EquipmentUsedHeight += 200;
                    //todo: equipment used total
                }

                var savedThirdPartyUsed = realm.All<ThirdPartyUsed>()
                                          .Where(t => t.TicketId == Ticket.ID)
                                          .ToList();

                foreach (var item in savedThirdPartyUsed)
                {
                    ThirdPartyUsed.Add(item);
                    ThirdPTUsedHeight += 180;
                    //todo: third party used total
                }

            }
        }

        public async Task<HttpResponseMessage[]> CheckSavedRequests()
        {
            try
            {
                var savedRequests = realm.All<SavedRequests>()
                                .ToList();

                var batch = new List<Task<HttpResponseMessage>>();

                var formDigest = await SharepointAPI.GetFormDigest();

                if (savedRequests != null || savedRequests.Any())
                {
                    foreach (var body in savedRequests)
                    {
                        var item = new StringContent(body.requestBody);
                        item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                        batch.Add(SharepointAPI.AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                               "Invoiced Tickets", item));
                    }

                    var results = await Task.WhenAll(batch);

                    for (int i = 0; i < results.Length; i++)
                    {
                        if (results[i].IsSuccessStatusCode)
                            realm.Write(()=> {
                                realm.Remove(savedRequests[i]);
                            });
                    }

                    System.Diagnostics.Debug.WriteLine("connect","success");

                    return results;
                }

                return null;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("connect", e.Message);
                return null;
            }
        }

        private DelegateCommand _saveCommand;

        public DelegateCommand Save
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(async () =>
                    {
                        try
                        {
                            var signature = await SignatureFromStream();

                            //using (var stream = await FileSystem.OpenAppPackageFileAsync("pdfStyles.txt"))
                            //{
                            //    using (var reader = new StreamReader(stream))
                            //    {
                            //        var fileContents = await reader.ReadToEndAsync();
                            //    }
                            //}

                            var body = GetInvoiceBody(signature);

                            if (connected)
                            {
                                var item = new StringContent(body);
                                item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                                var formDigest = await SharepointAPI.GetFormDigest();

                                //TODO: sync invoices first before adding

                                var addInvoice = await SharepointAPI.AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                                       "Invoiced Tickets", item);

                                var ensure = addInvoice.EnsureSuccessStatusCode();

                                if (ensure.IsSuccessStatusCode)
                                {
                                    //TODO: prompt for successful request
                                    System.Diagnostics.Debug.WriteLine("Success!");

                                    var response = JsonConvert.DeserializeObject<RootObject2>(await addInvoice.Content.ReadAsStringAsync());
                                    var invoice = response.invoice;
                                    realm.Write(()=> {
                                        realm.Add(invoice);
                                    });

                                    UpdateTicket(formDigest.D.GetContextWebInformation.FormDigestValue);
                                }
                                else
                                {
                                    //TODO: prompt for unsuccessful request
                                    System.Diagnostics.Debug.WriteLine("not success!");
                                }
                            }
                            else
                            {
                                realm.Write(()=> {
                                    realm.Add<SavedRequests>(new SavedRequests() {
                                        requestBody = body,
                                    });
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("saveerror", e.Message);
                        }
                    });
                }

                return _saveCommand;
            }
        }

        private async void UpdateTicket(string formDigest)
        {
            try
            {
                if (connected)
                {
                    var builder = new StringBuilder();
                    builder.Append("{'__metadata':{'type':'SP.Data.TicketsListItem'},");

                    builder.Append("'ApprovedStatus':'" + _approvalStatus + "',");
                    builder.Append("'Status':'" + _status + "'");
                    builder.Append("}");

                    var body = builder.ToString();

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var updateTix = await SharepointAPI.UpdateListItemByListTitle(formDigest, "Tickets", item, Ticket.ID.ToString());

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

        private string GetInvoiceBody(byte[] signature)
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("{'__metadata':{'type':'SP.Data.InvoicedTicketsListItem'},");

                var user = realm.All<User>().FirstOrDefault();

                var invoiceCount = 0;

                var invoicedTickets = realm.All<InvoicedTickets>()
                                      .Where(i => i.TicketNumber.Equals(Ticket.Title))
                                      .ToList();

                if (invoicedTickets != null && invoicedTickets.Count != 0)
                {
                    invoiceCount = invoicedTickets.Count + 1;
                }

                if(signature != null)
                {
                    _status = "Approved";
                    _approvalStatus = true;
                }

                var invoiceHtml = GetInvoiceHTML(Ticket.Title.Replace("TN", "IN") + invoiceCount.ToString(),
                                  "data:image/png;base64," + Convert.ToBase64String(signature));

                builder.Append("'TicketID':'" + Ticket.ID.ToString() + "',");
                builder.Append("'TicketNumber':'" + Ticket.Title + "',");
                builder.Append("'InvoiceVersion':'" + invoiceCount.ToString() + "',");
                builder.Append("'InvoiceHTML':'" + invoiceHtml + "',");
                builder.Append("'InvoiceNumber':'" + Ticket.Title.Replace("TN", "IN") + invoiceCount.ToString() + "',");
                builder.Append("'Status':'"+ _status +"',");
                builder.Append("'Trigger':'generatePDF',");
                builder.Append("'SignatureCode':'" + "data:image/png;base64," + Convert.ToBase64String(signature) + "',");
                builder.Append("'ResponseById':'" + user.UserId.ToString() + "'");
                builder.Append("}");

                var invoiceHTML = builder.ToString();

                System.Diagnostics.Debug.WriteLine("invoicehtml", invoiceHTML);

                return invoiceHTML;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("invoicebodyerror", e.Message);
                return string.Empty;
            }
        }

        private string GetInvoiceHTML(string invoiceNumber, string signature)
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("<!DOCTYPE html><body>");

                builder.Append(GetTopSection(invoiceNumber));
                builder.Append(GetLaborUsedSection());
                builder.Append(GetEquipmentUsedSection());
                builder.Append(GetMaterialUsedSection());
                builder.Append(GetThirdPartyUsedSection());
                builder.Append(GetSumOfChargesSection());
                builder.Append(GetSignatureSection(signature));

                builder.Append("</body></html>");

                return builder.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("invoicehtmlerror", e.Message);
                return string.Empty;
            }
        }

        private string GetTopSection(string invoiceNumber)
        {
            try
            {
                string top = "<div class=\"modal-dialog\" role=\"document\" style=\"width: 80%\">" +
                    "<div class=\"modal-content\" id=\"pdfContent\">" +
                        "<div class=\"modal-header\" id=\"invoiceHeader\">" +
                          "<table style = \"width: 100%;color: #ffffff;\">" +
                            "<tbody><tr>" +
                              "<td rowspan=\"2\"><h3 id = \"companyName\" >" + Ticket.CusomterName + "</h3></td>" +
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
                                        "<td class=\"tdBorderBottom\" id=\"billTo\" style=\"width: 35%;\">" + Ticket.CusomterName + "</td>" +
                                        "<td>&nbsp;&nbsp;AFE:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"AFE\">" + Ticket.JobAfe + "</td>" +
                                      "</tr>" +
                                      "<tr>" +
                                        "<td>Location:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"location\">" + Ticket.JobLocation + "</td>" +
                                        "<td>&nbsp;&nbsp;Code:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"code\">" + CostCode + "</td>" +
                                      "</tr>" +
                                      "<tr>" +
                                        "<td>Contact:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"contact\">" + Ticket.CustContactName + "</td>" +
                                        "<td>&nbsp;&nbsp;Contact Email:</td>" +
                                        "<td class=\"tdBorderBottom\" id=\"contactEmail\">" + Ticket.CustContactEmail + "</td>" +
                                      "</tr>" +
                                    "</tbody></table>" +
                                "</div>";

                return top;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("toperror", e.Message);
                return string.Empty;
            }
        }

        private string GetLaborUsedSection()
        {
            try
            {
                var builder = new StringBuilder();
                for (int i = 0; i < LaborUsed.Count; i++)
                {
                    var oddOrEven = "odd";
                    if (i % 2 == 0)
                        oddOrEven = "even";

                    var total = (LaborUsed[i].STHours * 100) + (LaborUsed[i].OTHours * 150);
                    LaborUsedTotal += total;

                    builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + LaborUsed[i].Employee.Title +
                        "</td><td>" + LaborUsed[i].WorkType + "</td><td>" + LaborUsed[i].STHours.ToString() +
                        "</td><td>$100</td><td>" + LaborUsed[i].OTHours.ToString() + "</td><td>$150</td><td>$" + total.ToString() + "</td></tr>");
                }
                string lbs = "<div class=\"panel - group\" style=\"padding - top: 10px; margin - left: 5px; margin - right: 5px; \">" +
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

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("laborusederror", e.Message);
                return string.Empty;
            }
        }

        private string GetEquipmentUsedSection()
        {
            try
            {
                var builder = new StringBuilder();
                for (int i = 0; i < EquipmentUsed.Count; i++)
                {
                    var oddOrEven = "odd";
                    if (i % 2 == 0)
                        oddOrEven = "even";

                    var total = Convert.ToInt32(EquipmentUsed[i].Rate) * EquipmentUsed[i].Quantity;
                    EquipmentUsedTotal += total;
                    var equipment = realm.All<EquipmentUnit>()
                                    .Where(e => e.ID == EquipmentUsed[i].EquipmentId)
                                    .FirstOrDefault();
                    builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + equipment.Title + "</td><td>" + equipment.ModelNumber + "</td><td>" +
                        EquipmentUsed[i].UnitOfMeasure + "</td><td>" + EquipmentUsed[i].Quantity.ToString() +
                        "</td><td>$" + EquipmentUsed[i].Rate + "</td><td>$" + total.ToString() + "</td></tr>");
                }

                string lbs = "<div class=\"panel panel-default\" id=\"equipmentUsed\">" +
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

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("equipmenterror", e.Message);
                return string.Empty;
            }
        }

        private string GetMaterialUsedSection()
        {
            try
            {
                var builder = new StringBuilder();
                for (int i = 0; i < MaterialUsed.Count; i++)
                {
                    if (MaterialUsed[i].Billable)
                    {
                        var oddOrEven = "odd";
                        if (i % 2 == 0)
                            oddOrEven = "even";

                        var total = Convert.ToInt32(MaterialUsed[i].Rate) * MaterialUsed[i].QuantityUsed;
                        MaterialUsedTotal += total;
                        var material = realm.All<Material>()
                                        .Where(e => e.ID == MaterialUsed[i].MaterialId)
                                        .FirstOrDefault();
                        builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + material.Title + "</td><td>" + MaterialUsed[i].QuantityUsed.ToString() +
                            "</td><td>" + MaterialUsed[i].UnitOfMeasure + "</td><td>$" + MaterialUsed[i].Rate + "</td><td>$" + total.ToString() + "</td></tr>");
                    }
                }

                string lbs = "<div class=\"panel panel-default\" id=\"materialUsed\">" +
                                        "<div class=\"panel-heading panel-heading-style\">Material Used</div>" +
                                        "<div class=\"panel-body\" id=\"materialDTB\"><div id = \"materialUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                        "<table id = \"materialUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                        "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 176px;\">" +
                                        "Item Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 145px;\">" +
                                        "Quantity</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 252px;\">" +
                                        "Unit of Measure</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 216px;\">" +
                                        "Cost Per Unit</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 94px;\">" +
                                        "Total</th></tr></thead><tbody>" + builder.ToString() + "</tbody></table></div></div></div>";

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("materialerror", e.Message);
                return string.Empty;
            }
        }

        private string GetThirdPartyUsedSection()
        {
            try
            {
                var builder = new StringBuilder();
                for (int i = 0; i < ThirdPartyUsed.Count; i++)
                {
                    var oddOrEven = "odd";
                    if (i % 2 == 0)
                        oddOrEven = "even";

                    ThirdPTUsedTotal += ThirdPartyUsed[i].Amount;

                    builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + ThirdPartyUsed[i].Vendor.Title +
                        "</td><td>$" + ThirdPartyUsed[i].Amount.ToString() + "</td><td>" + ThirdPartyUsed[i].MarkUp +
                        "</td><td>$" + ThirdPartyUsed[i].Amount.ToString() + "</td><td>$" + ThirdPartyUsed[i].Amount.ToString() + "</td></tr>");
                }
                string lbs = "<div class=\"panel panel-default\" id=\"thirdParty\">" +
                                        "<div class=\"panel-heading panel-heading-style\">3<sup>rd</sup> Party</div>" +
                                        "<div class=\"panel-body\" id=\"otherPartyDTB\"><div id = \"opUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                        "<table id = \"opUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                        "<thead><tr role = \"row\" ><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 231px;\">" +
                                        "Vendor Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 226px;\">" +
                                        "Original Cost</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 138px;\">" +
                                        "Markup</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 187px;\">" +
                                        "Final Price</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 101px;\">" +
                                        "Total</th></tr></thead><tbody>" + "</tbody></table></div></div></div></div>";

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("tpuerror", e.Message);
                return string.Empty;
            }
        }

        private string GetSumOfChargesSection()
        {
            try
            {
                var total = LaborUsedTotal + EquipmentUsedTotal + MaterialUsedTotal + ThirdPTUsedTotal;
                string lbs = "<div class=\"col - md - 6\" id=\"summOfchargesDiv\">" +
                                        "<div class=\"panel panel-default\">" +
                                        "<div class=\"panel-heading panel-heading-style\">Summary of Charges</div>" +
                                        "<table id = \"summOfcharges\" >" +
                                            "<tbody><tr>" +
                                            "</tr>" +
                                            "<tr id=\"labourUsed\">" +
                                                "<td class=\"tdLabels\">Labour Charges</td>" +
                                                "<td id = \"labourTotal\" >$" + LaborUsedTotal + "</td>" +
                                            "</tr>" +
                                            "<tr id = \"equipmentUsed\" >" +
                                                "<td class=\"tdLabels\">Equipment Charges</td>" +
                                                "<td id = \"equipmentTotal\" >$" + EquipmentUsedTotal + "</td>" +
                                            "</tr>" +
                                            "<tr id = \"materialUsed\">" +
                                                 "<td class=\"tdLabels\">Material Charges</td>" +
                                                "<td id = \"materialTotal\" >$" + MaterialUsedTotal + "</td>" +
                                            "</tr>" +
                                            "<tr id = \"thirdParty\" >" +
                                                  "<td class=\"tdLabels\">3<sup>rd</sup> Party Charges</td>" +
                                                "<td id = \"thirdPartyTotal\" >$" + ThirdPTUsedTotal + "</td>" +
                                            "</tr>" +
                                            "<tr id = \"totalCharges\" >" +
                                                "<td > Total </td >" +
                                                "<td id=\"summaryChargesTotal\">$" + total + "</td>" +
                                            "</tr>" +
                                        "</tbody></table>" +
                                    "</div>" +
                                "</div>";

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("sumerror", e.Message);
                return string.Empty;
            }
        }

        private string GetSignatureSection(string signature)
        {
            try
            {
                string lbs = "<div class=\"col - md - 6\" id=\"signatureStamp\">" +
                                        "Customer Signature Stamp" +
                                        "<img src = \"" + signature + "\" id=\"sign_prev\" style=\"\" width=\"382px\" height=\"286px\">" +
                                        "<div id = \"sign_prev_div\" ></ div >" +
                                         "<button type = \"button\" class=\"signatureStamp hidden\" onclick=\"loadSignatureForm()\" id=\"btnAddSign\">" +
                                            "<h4 id = \"signaturePlaceholder\" > Add Signature / Stamp</h4>" +
                                        "</button></div></div></div></div></div></div>";

                return lbs;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("signerror", e.Message);
                return string.Empty;
            }
        }
    }
}