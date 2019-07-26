﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContentExtractionService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private readonly ResponseContext _context;

        public ContentsController(ResponseContext context)
        {
            _context = context;

            if (_context.Responses.Count() == 0)
            {
                _context.Responses.Add(new Response { TraceId = Guid.NewGuid() });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet("{guid}")]
        public async Task<ActionResult<Response>> GetResponse(Guid guid)
        {
            var todoItem = await _context.Responses.FindAsync(guid);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Response>> Extract(Request request)
        {
            try
            {
                String content = request.EmailContent;


                Response response = GetResponse(content);
                _context.Responses.Add(response);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetResponse), new { guid = response.TraceId }, response);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }


        }


        private Response GetResponse(String content)
        {
            Response response = new Response();
            string mixed = content;
            string xml = "<FOO>" + mixed + "</FOO>";

            RelevantData relevantData;

            if (GetRelevantData(xml, out relevantData))
            {
                return GetOkResponse(response, relevantData);
            }
            else
            {
                return GetRejectResponse(response);
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
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private String GetElementValue(XDocument dox, String name)
        {
            var element = dox.Descendants().Where(n => n.Name == name).FirstOrDefault();
            return element == null ? String.Empty : element.Value.ToString();
        }

        private Response GetRejectResponse(Response response)
        {
            response.StatusCode = 0;
            response.StatusDescription = "reject";
            return response;
        }

        private Response GetOkResponse(Response response, RelevantData relevantData)
        {
            response.RelevantData = relevantData;
            response.StatusCode = 1;
            response.StatusDescription = "ok";
            return response;
        }
    }
}