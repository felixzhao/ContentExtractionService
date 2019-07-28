using System;
using BusinessEntities;
using BusinessLayer;
using Xunit;

namespace XUnitTests
{
    public class AcceptTests
    {
        private void AssertResponse(RelevantDataBO expected, RelevantDataBO actual)
        {

            Assert.NotNull(actual);

            Assert.Equal(expected.Date, actual.Date);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Vendor, actual.Vendor);

            Assert.NotNull(actual.Expense);
            Assert.Equal(expected.Expense.CostCentre, actual.Expense.CostCentre);
            Assert.Equal(expected.Expense.PaymentMethod, actual.Expense.PaymentMethod);
            Assert.Equal(expected.Expense.Total, actual.Expense.Total);
            Assert.Equal(expected.Expense.TotalExcludingGST, actual.Expense.TotalExcludingGST);
            Assert.Equal(expected.Expense.GST, actual.Expense.GST);
        }

        [Fact]
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

            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);
            Assert.True(result);
            Assert.NotNull(actual);
            Assert.NotNull(actual.Expense);


            var expected = new RelevantDataBO()
            {
                Date = new System.DateTime(2017, 4, 27),
                Description = "development team’s project end celebration dinner",
                Vendor = "Viaduct Steakhouse",
                Expense = new ExpenseBO()
                {
                    CostCentre = "DEV002",
                    PaymentMethod = "personal card",
                    Total = 1024.01M,
                    TotalExcludingGST = 890.44M,
                    GST = 133.57M
                }

            };

            AssertResponse(expected, actual);
        }

        [Fact]
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

            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);
            Assert.True(result);
            Assert.NotNull(actual);
            Assert.NotNull(actual.Expense);


            var expected = new RelevantDataBO()
            {
                Date = new System.DateTime(2017, 4, 27),
                Description = "development team’s project end celebration dinner",
                Vendor = "Viaduct Steakhouse",
                Expense = new ExpenseBO()
                {
                    CostCentre = "UNKNOWN",
                    PaymentMethod = "personal card",
                    Total = 1024.01M,
                    TotalExcludingGST = 890.44M,
                    GST = 133.57M
                }

            };


            AssertResponse(expected, actual);
        }

        [Fact]
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

            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);
            Assert.True(result);
            Assert.NotNull(actual);
            Assert.NotNull(actual.Expense);


            var expected = new RelevantDataBO()
            {
                Date = new System.DateTime(2017, 4, 27),
                Description = "development team’s project end celebration dinner",
                Vendor = "Viaduct Steakhouse",
                Expense = new ExpenseBO()
                {
                    CostCentre = "UNKNOWN",
                    PaymentMethod = "personal card",
                    Total = 1024.01M,
                    TotalExcludingGST = 890.44M,
                    GST = 133.57M
                }

            };

            AssertResponse(expected, actual);
        }

        [Fact]
        public void CheckGSTAndTotalExcludingGST()
        {
            string content = @"Hi Yvaine,
                                <expense>
                                <total>1024.01</total>
                                </expense>
                                Regards,
                                Ivan";

            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);

            Assert.True(result);
            Assert.NotNull(actual);

            Assert.NotNull(actual.Expense);


            var expected = new RelevantDataBO()
            {
                Expense = new ExpenseBO()
                {
                    Total = 1024.01M,
                    TotalExcludingGST = 890.44M,
                    GST = 133.57M
                }

            };
            Assert.Equal(expected.Expense.Total, actual.Expense.Total);
            Assert.Equal(expected.Expense.TotalExcludingGST, actual.Expense.TotalExcludingGST);
            Assert.Equal(expected.Expense.GST, actual.Expense.GST);
        }

        [Theory]
        [InlineData(@"Hi Yvaine,
Please create an expense claim for the below.Relevant details are marked up as
requested...
<expense><cost_centre> DEV002 </cost_centre>
<total> 1024.01 </total ><payment_method> personal card </payment_method>
</expense>
From: Ivan Castle
Sent: Friday, 16 February 2018 10:32 AM
To: Antoine Lloyd
Subject: test
Hi Antoine,
Please create a reservation at the <vendor> Viaduct Steakhouse </vendor> our
<description> development team’s project end celebration dinner </description> on
<date> Tuesday 27 April 2017 </date>.We expect to arrive around
7.15pm.Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan")]
        [InlineData(@"Hi Yvaine,
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
Ivan")]
        [InlineData(@"Hi Yvaine,
<expense>
<total>1024.01</total>
</expense>
Regards,
Ivan")]
        public void SuccessTest(string content)
        {
            IContentExtractor extractor = new ContentExtractor();
            RelevantDataBO actual;
            var result = extractor.GetRelevantData(content, out actual);

            Assert.True(result);
            Assert.NotNull(actual);
        }
    }
}
