using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ContentExtractionService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        // POST: api/contents/
        [HttpPost]
        public IActionResult Extract2(Request request)
        {
            try
            {
                
                

                String content = request.EmailContent;
                Response response = GetResponse(content);

                

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Get Exception during process.");
                return BadRequest();
            }
        }

        private Response GetResponse(String content)
        {
            
            string mixed = content;
            string xml = "<FOO>" + mixed + "</FOO>";

            RelevantData relevantData;

            if (GetRelevantData(xml, out relevantData))
            {
                return GetOkResponse(relevantData);
            }
            else
            {
                return GetRejectResponse();
            }
        }

        private bool GetRelevantData(string content, out RelevantData relevantData)
        {
			

			relevantData = new RelevantData();

            try
            {
                XDocument dox = XDocument.Parse(content);


                var total = GetElementValue(dox, "total");

                if (String.IsNullOrEmpty(total))
                {
                    return false;
                }

                Decimal gst_rate = 1.15M;
                Decimal total_value;
                if (Decimal.TryParse(total, out total_value))
                {
                    var costCentre = GetElementValue(dox, "cost_centre");

                    var totalExcludingGst = Decimal.Round(Decimal.Divide(total_value, gst_rate), 2);
                    var gst = total_value - totalExcludingGst;

                    relevantData.Expense = new Expense
                    {
                        Total = total_value,
                        GST = gst,
                        TotalExcludingGST = totalExcludingGst,
                        CostCentre = String.IsNullOrEmpty(costCentre) ? "UNKNOWN" : costCentre,
                        PaymentMethod = GetElementValue(dox, "payment_method")
                    };

                }
                else
                {
                    return false;
                }

                relevantData.Vendor = GetElementValue(dox, "vendor");
                relevantData.Description = GetElementValue(dox, "description");
                var dateText = GetElementValue(dox, "date");

                string pattern = "dddd dd MMMM yyyy";
                DateTime dt;
                if (DateTime.TryParseExact(dateText, pattern, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out dt))
                {
                    relevantData.Date = dt;
                }
            }
            catch (System.Xml.XmlException ex)
            {
				//Logger.Log(" >>>>>> Input xml Data has format error <<<<<< " + ex.Message);
				return false;
			}

			
			return true;
        }

        private String GetElementValue(XDocument dox, String name)
        {
            var element = dox.Descendants().Where(n => n.Name == name).FirstOrDefault();
            return element == null ? String.Empty : element.Value.ToString();
        }

        private Response GetRejectResponse()
        {
			Response response = new Response();
			response.StatusCode = 0;
            response.StatusDescription = "reject";
            return response;
        }

        private Response GetOkResponse(RelevantData relevantData)
        {
			Response response = new Response();
			response.RelevantData = relevantData;
            response.StatusCode = 1;
            response.StatusDescription = "ok";
            return response;
        }
	}
}