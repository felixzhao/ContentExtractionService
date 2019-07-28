using System;
using BusinessLayer;
using BusinessEntities;
using AutoMapper;
using ContentExtractService.Models;

namespace ContentExtractionService
{
    
        public class MappingEntity : Profile
        {
            public MappingEntity()
            {
                CreateMap<RelevantData, RelevantDataBO >();
            }
        }
    
}
