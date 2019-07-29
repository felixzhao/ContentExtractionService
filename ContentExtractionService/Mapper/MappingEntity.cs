using System;
using BusinessLayer;
using BusinessEntities;
using AutoMapper;
using ContentExtractService.Models;

namespace ContentExtractionService
{
    /// <summary>
    /// Auto Mapping BO to Contract
    /// </summary>
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<RelevantDataBO, Response>();
            CreateMap<ExpenseBO, Expense>();
        }
    }

}
