using System;
using System.Collections.Generic;
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

            XDocument dox = XDocument.Parse(xml);
            var total = dox.Descendants().Where(n => n.Name == "total").FirstOrDefault();


            if (total == null)
            {
                return GetRejectResponse(response);
            }



            Decimal gst_rate = 1.15M;
            Decimal total_value;
            if (Decimal.TryParse(total.Value, out total_value))
            {

                var totalExcludingGst = Decimal.Round(Decimal.Divide(total_value, gst_rate), 2);
                var gst = total_value - totalExcludingGst;

                response.expense = new Expense
                {
                    Total = total_value,
                    GST = gst,
                    TotalExcludingGST = totalExcludingGst
                };

            }
            else
            {
                return GetRejectResponse(response);
            }

            return response;
        }

        private Response GetRejectResponse(Response response)
        {
            response.StatusCode = 0;
            response.StatusDescription = "reject";
            return response;
        }
    }
}