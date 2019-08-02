using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessEntities;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using BusinessLayer;
using AutoMapper;

namespace ContentExtractionService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContentExtractor _contentExtractor;
        
        public ContentsController(IMapper mapper, IContentExtractor contentExtractor)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contentExtractor = contentExtractor ?? throw new ArgumentNullException(nameof(contentExtractor));
        }

        // POST: api/contents/
        [HttpPost]
        public IActionResult Extract(Request request)
        {
            try
            {
                String content = request.EmailContent;
                RelevantDataBO relevantDataBO;

                if (_contentExtractor.GetRelevantData(content, out relevantDataBO))
                {
                    // Extract Success. Mapping BO to Contract.
                    var response = _mapper.Map<Response>(relevantDataBO);
                    Log.Debug<string>("Extract Done.", content);
                    return Ok(response);
                }
                else
                {
                    // Extract Fail. Because of invalid data.
                    Log.Debug<string>("Extract Failed.", content);
                    return BadRequest();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Argument Out Of Range
                Log.Debug<string>("ArgumentOutOfRange", ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                // Fail. Because of Internal Server Error.
                Log.Error(ex, "Get unexpected exception during process.");
                return StatusCode(500);
            }
        }
	}
}