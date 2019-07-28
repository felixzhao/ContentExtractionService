using System;
using BusinessEntities;

namespace BusinessLayer
{
    public class ContentExtractor : IContentExtractor
    {
        public bool GetRelevantData(string content, out RelevantDataBO relevantData)
        {
            relevantData = new RelevantDataBO();
            return true;
        }
    }
}
