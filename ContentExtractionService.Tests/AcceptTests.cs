using NUnit.Framework;
using ContentExtractionService.Controllers;
using ContentExtractionService.Models;
using ContentExtractService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContentExtractionService.Tests
{
    public class AcceptTests
    {
        [SetUp]
        public void Setup()
        {

        }

        private Response GetTestResponse(string content)
        {
            ContentsController home = new ContentsController();
            Request request = new Request() { EmailContent = content };
            var actionResult = home.Extract2(request);
            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);
            return okObjectResult.Value as Response;
        }

        private void AssertResponse(Response expected, Response actual)
        {
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
            Assert.NotNull(actual.TraceId);
            Assert.NotNull(actual.RelevantData);

            Assert.AreEqual(expected.RelevantData.Date, actual.RelevantData.Date);
            Assert.AreEqual(expected.RelevantData.Description, actual.RelevantData.Description);
            Assert.AreEqual(expected.RelevantData.Vendor, actual.RelevantData.Vendor);

            Assert.NotNull(actual.RelevantData.Expense);
            Assert.AreEqual(expected.RelevantData.Expense.CostCentre, actual.RelevantData.Expense.CostCentre);
            Assert.AreEqual(expected.RelevantData.Expense.PaymentMethod, actual.RelevantData.Expense.PaymentMethod);
            Assert.AreEqual(expected.RelevantData.Expense.Total, actual.RelevantData.Expense.Total);
            Assert.AreEqual(expected.RelevantData.Expense.TotalExcludingGST, actual.RelevantData.Expense.TotalExcludingGST);
            Assert.AreEqual(expected.RelevantData.Expense.GST, actual.RelevantData.Expense.GST);
        }

        [Test]
        public void OK1()
        {
            string content = @"Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Tuesday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";

            Response expected = new Response() { StatusCode = 1, StatusDescription = "ok" };

            var actual = GetTestResponse(content);
            Assert.NotNull(actual);
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
        }

        [Test]
        public void PassTest1()
        {
            string content = @"Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre>DEV002</cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd 
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";

            var actual = GetTestResponse(content);
            Assert.NotNull(actual);

            Response expected = new Response()
            {
                StatusCode = 1,
                StatusDescription = "ok",
                RelevantData = new RelevantData()
                {
                    Date = new System.DateTime(2017, 4, 27),
                    Description = "development team’s project end celebration dinner",
                    Vendor = "Viaduct Steakhouse",
                    Expense = new Expense()
                    {
                        CostCentre = "DEV002",
                        PaymentMethod = "personal card",
                        Total = 1024.01M,
                        TotalExcludingGST = 890.44M,
                        GST = 133.57M
                    }
                }
            };


            AssertResponse(expected, actual);


    
        }

        [Test]
        public void NoCostCentre()
        {
            string content = @"Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd 
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";

            //ContentsController home = new ContentsController();
            //Request request = new Request() { EmailContent = content };

            //Response expected = new Response()
            //{
            //    StatusCode = 1,
            //    StatusDescription = "ok",
            //    RelevantData = new RelevantData()
            //    {
            //        Date = new System.DateTime(2017, 4, 27),
            //        Description = "development team’s project end celebration dinner",
            //        Vendor = "Viaduct Steakhouse",
            //        Expense = new Expense()
            //        {
            //            CostCentre = "UNKNOWN",
            //            PaymentMethod = "personal card",
            //            Total = 1024.01M,
            //            TotalExcludingGST = 890.44M,
            //            GST = 133.57M
            //        }
            //    }
            //};

            //var actionResult = home.Extract2(request);
            ////Assert
            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.NotNull(okObjectResult);

            //var actual = okObjectResult.Value as Response;
            //Assert.NotNull(actual);

            //AssertResponse(expected, actual);
        }

        [Test]
        public void NoCostCentre2()
        {
            string content = @"Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as
requested...
<expense><cost_centre></cost_centre>
<total>1024.01</total><payment_method>personal card</payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd 
Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
<description>development team’s project end celebration dinner</description> on
<date>Thursday 27 April 2017</date>. We expect to arrive around
7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";

            //ContentsController home = new ContentsController();
            //Request request = new Request() { EmailContent = content };

            //Response expected = new Response()
            //{
            //    StatusCode = 1,
            //    StatusDescription = "ok",
            //    RelevantData = new RelevantData()
            //    {
            //        Date = new System.DateTime(2017, 4, 27),
            //        Description = "development team’s project end celebration dinner",
            //        Vendor = "Viaduct Steakhouse",
            //        Expense = new Expense()
            //        {
            //            CostCentre = "UNKNOWN",
            //            PaymentMethod = "personal card",
            //            Total = 1024.01M,
            //            TotalExcludingGST = 890.44M,
            //            GST = 133.57M
            //        }
            //    }
            //};

            //var actionResult = home.Extract2(request);
            ////Assert
            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.NotNull(okObjectResult);

            //var actual = okObjectResult.Value as Response;
            //Assert.NotNull(actual);

            //AssertResponse(expected, actual);
        }


        [Test]
        public void CheckGSTAndTotalExcludingGST()
        {
            string content = @"Hi Yvaine,
<expense>
<total>1024.01</total>
</expense>
Regards,
Ivan";

            //ContentsController home = new ContentsController();
            //Request request = new Request() { EmailContent = content };

            //Response expected = new Response()
            //{
            //    StatusCode = 1,
            //    StatusDescription = "ok",
            //    RelevantData = new RelevantData()
            //    {
            //        Expense = new Expense()
            //        {
            //            Total = 1024.01M,
            //            TotalExcludingGST = 890.44M,
            //            GST = 133.57M
            //        }
            //    }
            //};

            //var actionResult = home.Extract2(request);
            ////Assert
            //var okObjectResult = actionResult as OkObjectResult;
            //Assert.NotNull(okObjectResult);

            //var actual = okObjectResult.Value as Response;
            //Assert.NotNull(actual);



            //Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            //Assert.AreEqual(expected.StatusDescription, actual.StatusDescription);
            //Assert.NotNull(actual.TraceId);
            //Assert.NotNull(actual.RelevantData);

            //Assert.NotNull(actual.RelevantData.Expense);
            
            
            //Assert.AreEqual(expected.RelevantData.Expense.Total, actual.RelevantData.Expense.Total);
            //Assert.AreEqual(expected.RelevantData.Expense.TotalExcludingGST, actual.RelevantData.Expense.TotalExcludingGST);
            //Assert.AreEqual(expected.RelevantData.Expense.GST, actual.RelevantData.Expense.GST);

        }
    }
}
