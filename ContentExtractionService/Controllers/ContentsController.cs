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
            _mapper = mapper;
            _contentExtractor = contentExtractor;
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
                    var response = _mapper.Map<Response>(relevantDataBO);
                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Get Exception during process.");
                return BadRequest();
            }
        }
	}
}