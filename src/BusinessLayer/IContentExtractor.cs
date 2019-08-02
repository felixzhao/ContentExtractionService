using System;
using BusinessEntities;

namespace BusinessLayer
{
    /// <summary>
    /// Interface of Content Extractor
    /// </summary>
    public interface IContentExtractor
    {
        /// <summary>
        /// Extract relevant data from plain text
        /// </summary>
        /// <param name="content">plain text</param>
        /// <param name="relevantData">Business Object of structural content data</param>
        /// <returns>Success: return true; Fail: return false</returns>
        bool GetRelevantData(string content, out RelevantDataBO relevantData);
    }
}
