using System;
using BusinessEntities;

namespace BusinessLayer
{
    public interface IContentExtractor
    {
        bool GetRelevantData(string content, out RelevantDataBO relevantData);
    }
}
