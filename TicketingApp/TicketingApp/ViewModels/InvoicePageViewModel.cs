using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services.Sharepoint_API;
using System;
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
using TicketingApp.Models.ThirdPartyUsed;
using TicketingApp.Models.Tickets;
using TicketingApp.Models.Users;
using Xamarin.Essentials;

namespace TicketingApp.ViewModels
{
    public class InvoicePageViewModel : ViewModelBase
    {
        private ObservableCollection<LaborUsed> _laborUsed;

        public ObservableCollection<LaborUsed> LaborUsed
        {
            get { return _laborUsed; }
            set { SetProperty(ref _laborUsed, value); }
        }

        private ObservableCollection<MaterialUsed> _materialUsed;

        public ObservableCollection<MaterialUsed> MaterialUsed
        {
            get { return _materialUsed; }
            set { SetProperty(ref _materialUsed, value); }
        }

        private ObservableCollection<EquipmentUsed> _equipmentUsed;

        public ObservableCollection<EquipmentUsed> EquipmentUsed
        {
            get { return _equipmentUsed; }
            set { SetProperty(ref _equipmentUsed, value); }
        }

        private ObservableCollection<ThirdPartyUsed> _thirdPartyUsed;

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

        public Func<Task<byte[]>> SignatureFromStream { get; set; }

        public InvoicePageViewModel(INavigationService navigationService, ISharepointAPI sharepointAPI)
            : base(navigationService, sharepointAPI)
        {
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

                LaborUsed = new ObservableCollection<LaborUsed>(savedLaborUsed);

                var savedMaterialUsed = realm.All<MaterialUsed>()
                                        .Where(m => m.TicketId == Ticket.ID)
                                        .ToList();

                MaterialUsed = new ObservableCollection<MaterialUsed>(savedMaterialUsed);

                var savedEquipmentUsed = realm.All<EquipmentUsed>()
                                        .Where(e => e.TicketId == Ticket.ID)
                                        .ToList();

                EquipmentUsed = new ObservableCollection<EquipmentUsed>(savedEquipmentUsed);

                var savedThirdPartyUsed = realm.All<ThirdPartyUsed>()
                                          .Where(t => t.TicketId == Ticket.ID)
                                          .ToList();

                ThirdPartyUsed = new ObservableCollection<ThirdPartyUsed>(savedThirdPartyUsed);
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
                        var signature = await SignatureFromStream();

                        using (var stream = await FileSystem.OpenAppPackageFileAsync("pdfStyles"))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var fileContents = await reader.ReadToEndAsync();
                                //TODO: check if the pdfstyles is correct
                                System.Diagnostics.Debug.WriteLine("pdfstyles", fileContents);
                            }
                        }

                        var body = GetInvoiceBody(signature);
                        //TODO: Check the body

                        if (connected)
                        {
                            var item = new StringContent(body);
                            item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                            var formDigest = await SharepointAPI.GetFormDigest();

                            var addInvoice = await SharepointAPI.AddListItemByListTitle(formDigest.D.GetContextWebInformation.FormDigestValue,
                                                   "Invoiced Tickets",item);

                            var ensure = addInvoice.EnsureSuccessStatusCode();

                            if (ensure.IsSuccessStatusCode)
                            {
                                //TODO: prompt for successful request
                                //TODO: Sync the Invoiced tickets
                                //TODO: Update the Ticket Item {ApprovalStatus and Stauts}
                            }
                            else
                            {
                                //TODO: prompt for unsuccessful request
                            }
                        }
                        else
                        {
                            //TODO: Save to DB
                        }
                    });
                }

                return _saveCommand;
            }
        }

        private string GetInvoiceBody(byte[] signature)
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

            var invoiceHtml = GetInvoiceHTML(Ticket.Title.Replace("TN", "IN") + invoiceCount.ToString(), 
                              "data:image/png;base64," + Convert.ToBase64String(signature));

            var invoice = new InvoicedTickets()
            {
                TicketID = Ticket.ID.ToString(),
                TicketNumber = Ticket.Title,
                InvoiceVersion = invoiceCount.ToString(),
                InvoiceHTML = invoiceHtml,
                InvoiceNumber = Ticket.Title.Replace("TN", "IN") + invoiceCount.ToString(),
                Status = "Approved",
                SignatureCode = "data:image/png;base64," + Convert.ToBase64String(signature),
                ResponseById = user.UserId,
            };

            builder.Append(JsonConvert.SerializeObject(invoice,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));
            builder.Append("}");

            return builder.ToString();
        }

        private string GetInvoiceHTML(string invoiceNumber,string signature)
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

        private string GetTopSection(string invoiceNumber)
        {
            string top = "<div class=\"modal-dialog\" role=\"document\" style=\"width: 80%\">" +
            "< div class=\"modal-content\" id=\"pdfContent\">" +
                "<div class=\"modal-header\" id=\"invoiceHeader\">" +
                  "<table style = \"width: 100%;color: #ffffff;\" >" +
                    "< tbody >< tr >" +
                      "< td rowspan=\"2\"><h3 id = \"companyName\" >" + Ticket.CusomterName + "</ h3 ></ td >" +
                       "< td >< p id=\"invoiceNumber\" class=\"pull-right\">Invoice Number: " + invoiceNumber + "</p></td>" +
                    "</tr>" +
                    "<tr>" +
                      "<td><p id = \"invoiceDate\" class=\"pull-right\">Date: " + DateTime.Now.ToString() + "</p></td>" +
                    "</tr>" +
                  "</tbody></table>" +
                "</div>" +
                "<div class=\"modal-body\" id=\"pdfContentBody\">" +
                    "<div class=\"container-fluid\">" +
                        "<div class=\"row\" style=\"padding-top: 10px; margin-left: 20px;margin-right: 20px;\">" +
                            "<table style = \"width:100%;\" >" +
                              "< tbody >< tr >" +
                                "< td > Bill To:</td>" +
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

        private string GetLaborUsedSection()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < LaborUsed.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                var total = (LaborUsed[i].STHours * 100) + (LaborUsed[i].OTHours * 150);
                builder.Append("<tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + LaborUsed[i].Employee.Title +
                    "</td><td>"+ LaborUsed[i].WorkType +"</td><td>"+ LaborUsed[i].STHours.ToString() + 
                    "</td><td>$100</td><td>"+ LaborUsed[i].OTHours.ToString() + "</td><td>$150</td><td>$"+ total.ToString() +"</td></tr>");

            }
            string lbs = "<div class=\"panel - group\" style=\"padding - top: 10px; margin - left: 5px; margin - right: 5px; \">" +
                         "< div class=\"col-md-12\" id=\"usedSummary\">" +
                                "<div class=\"panel panel-default\" id=\"labourUsed\">" +
                                    "<div class=\"panel-heading panel-heading-style\">Labour Used</div>" +
                                    "<div class=\"panel-body\" id=\"labourDTB\"><tbody<tr></tbody<tr>" +
                                    "<div id = \"laborsUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"laborsUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" >< th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 143px;\">" +
                                    "Full Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 129px;\">" +
                                    "Worker Type</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 161px;\">" +
                                    "Standard Hours</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 84px;\">" +
                                    "ST Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 167px;\">" +
                                    "Over-time Hours</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 86px;\">" +
                                    "OT Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 77px;\">" +
                                    "Total</th></tr></thead><tbody>" + builder.ToString() +"</ tbody ></ table ></ div ></ div ></ div >";

            return lbs;
        }

        private string GetEquipmentUsedSection()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < EquipmentUsed.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                var total = Convert.ToInt32(EquipmentUsed[i].Rate) * EquipmentUsed[i].Quantity;
                var equipment = realm.All<EquipmentUnit>()
                                .Where(e => e.ID == EquipmentUsed[i].EquipmentId)
                                .FirstOrDefault();
                builder.Append("< tr role = \"row\" class=\""+ oddOrEven +"\"><td>"+ equipment.Title +"</td><td>"+ equipment.ModelNumber +"</td><td>"+ 
                    EquipmentUsed[i].UnitOfMeasure +"</td><td>"+ EquipmentUsed[i].Quantity.ToString() + 
                    "</td><td>$"+ EquipmentUsed[i].Rate + "</td><td>$"+ total.ToString() +"</td></tr>");

            }

            string lbs = "<div class=\"panel panel-default\" id=\"equipmentUsed\">" +
                                    "< div class=\"panel-heading panel-heading-style\">Equipment Used</div>" +
                                    "<div class=\"panel-body\" id=\"equipmentDTB\"><div id = \"equipmentUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">" +
                                    "<table id = \"equipmentUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">" +
                                    "<thead><tr role = \"row\" >< th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 238px;\">" +
                                    "Equipment Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 121px;\">" +
                                    "Unit No.</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 221px;\">" +
                                    "Unit of Measure</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 127px;\">" +
                                    "Quantity</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 77px;\">" +
                                    "Rate</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 81px;\">" +
                                    "Total</th></tr></thead><tbody>" + builder.ToString() + "</tbody></table></div></div></div>";

            return lbs;
        }

        private string GetMaterialUsedSection()
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
                    var material = realm.All<Material>()
                                    .Where(e => e.ID == MaterialUsed[i].MaterialId)
                                    .FirstOrDefault();
                    builder.Append("< tr role = \"row\" class=\"" + oddOrEven + "\"><td>" + material.Title + "</td><td>" + MaterialUsed[i].QuantityUsed.ToString() +
                        "</td><td>" + MaterialUsed[i].UnitOfMeasure + "</td><td>$" + MaterialUsed[i].Rate + "</td><td>$" + total.ToString() + "</td></tr>");
                }
            }


            string lbs = "<div class=\"panel panel-default\" id=\"materialUsed\">" +
                                    "< div class=\"panel-heading panel-heading-style\">Material Used</div>" +
									"<div class=\"panel-body\" id=\"materialDTB\"><div id = \"materialUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">"+
                                    "<table id = \"materialUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">"+
                                    "<thead><tr role = \"row\" >< th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 176px;\">"+
                                    "Item Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 145px;\">"+
                                    "Quantity</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 252px;\">"+
                                    "Unit of Measure</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 216px;\">"+
                                    "Cost Per Unit</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 94px;\">"+
                                    "Total</th></tr></thead><tbody>"+ builder.ToString() +"</tbody></table></div></div></div>";


            return lbs;
        }

        private string GetThirdPartyUsedSection()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < ThirdPartyUsed.Count; i++)
            {
                var oddOrEven = "odd";
                if (i % 2 == 0)
                    oddOrEven = "even";

                builder.Append("< tr role = \"row\" class=\""+ oddOrEven +"\"><td>"+ ThirdPartyUsed[i].Vendor.Title +
                    "</td><td>$" + ThirdPartyUsed[i].Amount.ToString() + "</td><td>" + ThirdPartyUsed[i].MarkUp + 
                    "</td><td>$"+ ThirdPartyUsed[i].Amount.ToString() +"</td><td>$"+ ThirdPartyUsed[i].Amount.ToString() +"</td></tr>");

            }
            string lbs = "<div class=\"panel panel-default\" id=\"thirdParty\">"+
                                    "< div class=\"panel-heading panel-heading-style\">3<sup>rd</sup> Party</div>"+
									"<div class=\"panel-body\" id=\"otherPartyDTB\"><div id = \"opUsedTable_wrapper\" class=\"dataTables_wrapper no-footer\">"+
                                    "<table id = \"opUsedTable\" class=\"table table-striped table-bordered dataTable no-footer\" role=\"grid\">"+
                                    "<thead><tr role = \"row\" >< th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 231px;\">"+
                                    "Vendor Name</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 226px;\">"+
                                    "Original Cost</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 138px;\">"+
                                    "Markup</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 187px;\">"+
                                    "Final Price</th><th class=\"sorting_disabled\" rowspan=\"1\" colspan=\"1\" style=\"width: 101px;\">"+
                                    "Total</th></tr></thead><tbody>"+"</tbody></table></div></div></div></div>";

            return lbs;
        }

        private string GetSumOfChargesSection()
        {
            var laborUsedTotal = "";
            var equipmentUsedTotal = "";
            var materialUsedTotal = "";
            var thirdPartyTotal = "";
            var totalAmount = "";
            string lbs = "<div class=\"col - md - 6\" id=\"summOfchargesDiv\">"+
                                    "< div class=\"panel panel-default\">" +
									"<div class=\"panel-heading panel-heading-style\">Summary of Charges</div>"+
									"<table id = \"summOfcharges\" >"+
                                        "< tbody >< tr >"+
                                        "</ tr >"+
                                        "< tr id=\"labourUsed\">"+
											"<td class=\"tdLabels\">Labour Charges</td>"+
											"<td id = \"labourTotal\" >$"+ laborUsedTotal +"</td>"+
										"</tr>"+
										"<tr id = \"equipmentUsed\" >"+
                                            "< td class=\"tdLabels\">Equipment Charges</td>" +
											"<td id = \"equipmentTotal\" >$" + equipmentUsedTotal + "</td>"+
										"</tr>"+
										"<tr id = \"materialUsed\" >"+
                                             "< td class=\"tdLabels\">Material Charges</td>"+
											"<td id = \"materialTotal\" >$"+ materialUsedTotal + "</td>" +
										"</tr>" +
										"<tr id = \"thirdParty\" >" +
                                              "< td class=\"tdLabels\">3<sup>rd</sup> Party Charges</td>"+
											"<td id = \"thirdPartyTotal\" >$"+ thirdPartyTotal + "</td>" +
										"</tr>"+
										"<tr id = \"totalCharges\" >"+
                                            "< td > Total </ td >" +
                                            "< td id=\"summaryChargesTotal\">$" + totalAmount + "</td>" +
										"</tr>"+
									"</tbody></table>"+
								"</div>"+
							"</div>";

            return lbs;
        }

        private string GetSignatureSection(string signature)
        {
            string lbs = "<div class=\"col - md - 6\" id=\"signatureStamp\">"+
                                "Customer Signature Stamp"+
                                "< img src = \"" + signature + "\" id=\"sign_prev\" style=\"\" width=\"382px\" height=\"286px\">"+
                                "< div id = \"sign_prev_div\" ></ div >"+
                                 "< button type = \"button\" class=\"signatureStamp hidden\" onclick=\"loadSignatureForm()\" id=\"btnAddSign\">"+
									"<h4 id = \"signaturePlaceholder\" > Add Signature / Stamp</h4>" +
								"</button></div></div></div></div></div></div>";

            return lbs;
        }
    }
}