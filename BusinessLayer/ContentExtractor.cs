using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using BusinessEntities;
using Serilog;

namespace BusinessLayer
{
    /// <summary>
    /// Class for Extract Content to Business Object
    /// </summary>
    public class ContentExtractor : IContentExtractor
    {
        /// <summary>
        /// Extract relevant data from plain text
        /// </summary>
        /// <param name="content">plain text</param>
        /// <param name="relevantData">Business Object of structural content data</param>
        /// <returns>Success: return true; Fail: return false</returns>
        public bool GetRelevantData(string content, out RelevantDataBO relevantData)
        {
            relevantData = new RelevantDataBO();

            string mixed = content;
            string xml = "<FOO>" + mixed + "</FOO>";

            try
            {
                XDocument dox = XDocument.Parse(xml);

                var totalString = GetElementValue(dox, "total");

                if (String.IsNullOrEmpty(totalString))
                {
                    // if no total, reject request.
                    Log.Warning("Input data has no total value.");
                    return false;
                }

                decimal total;
                if (decimal.TryParse(totalString, out total))
                {
                    var costCentre = GetElementValue(dox, "cost_centre");

                    var totalExcludingGst = PriceCalculator.GetTotalExcludingGst(total);
                    var gst = PriceCalculator.GetGST(total, totalExcludingGst);
                    relevantData.Expense = new ExpenseBO
                    {
                        Total = total,
                        GST = gst,
                        TotalExcludingGST = totalExcludingGst,
                        CostCentre = String.IsNullOrEmpty(costCentre) ? "UNKNOWN" : costCentre,
                        PaymentMethod = GetElementValue(dox, "payment_method")
                    };
                }
                else
                {
                    // if total is not number, reject request.
                    Log.Warning("Input total value is invalid.");
                    return false;
                }

                relevantData.Vendor = GetElementValue(dox, "vendor");
                relevantData.Description = GetElementValue(dox, "description");
                var dateText = GetElementValue(dox, "date");

                // Get date based on custom format
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
                Log.Warning(ex, "Input xml Data has format error");
                return false;
            }catch(ArgumentOutOfRangeException ex)
            {
                Log.Warning(ex, ex.Message);
                throw;
            }


            return true;
        }

        /// <summary>
        /// Get Element value from XDocument
        /// </summary>
        /// <param name="dox">XDocument Object</param>
        /// <param name="name">Element Name</param>
        /// <returns>Value of Element</returns>
        private String GetElementValue(XDocument dox, String name)
        {
            var element = dox.Descendants().Where(n => n.Name == name).FirstOrDefault();
            return element == null ? String.Empty : element.Value.ToString();
        }
    }
}
