using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using BusinessEntities;
using Serilog;

namespace BusinessLayer
{
    public class ContentExtractor : IContentExtractor
    {
        public bool GetRelevantData(string content, out RelevantDataBO relevantData)
        {


            relevantData = new RelevantDataBO();

            string mixed = content;
            string xml = "<FOO>" + mixed + "</FOO>";

            try
            {
                XDocument dox = XDocument.Parse(xml);


                var total = GetElementValue(dox, "total");

                if (String.IsNullOrEmpty(total))
                {
                    Log.Warning("Input data has no total value.");
                    return false;
                }

                decimal gst_rate = 1.15M;
                decimal total_value;
                if (decimal.TryParse(total, out total_value))
                {
                    var costCentre = GetElementValue(dox, "cost_centre");

                    var totalExcludingGst = decimal.Round(Decimal.Divide(total_value, gst_rate), 2);
                    var gst = total_value - totalExcludingGst;

                    relevantData.Expense = new ExpenseBO
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
                Log.Warning(ex, "Input xml Data has format error");
                return false;
            }


            return true;
        }

        private String GetElementValue(XDocument dox, String name)
        {
            var element = dox.Descendants().Where(n => n.Name == name).FirstOrDefault();
            return element == null ? String.Empty : element.Value.ToString();
        }
    }
}
