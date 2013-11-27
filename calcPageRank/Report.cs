using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace generateMatrix
{
    class Report
    {
        public string Parent {get;set;}
        public string Child {get;set;}
        public List<Report> reportList = new List<Report>();
        private string crawlReport;
        private string resultsReport;

        public string CrawlReport
        {
            get { return crawlReport; }
            set { crawlReport = value; }
        }

        public string ResultsReport
        {
            get { return resultsReport; }
            set { resultsReport = value; }
        }

        public Report(){
        }

        public Report(List<Report> reportList) {
            this.reportList = reportList;
        }
        
        public void generateReport() {

            var query =
                    from priceLog in this.reportList
                    group priceLog by priceLog.Parent into dateGroup
                    select new {
                        p = dateGroup.Key,
                        c = dateGroup.Select(priceLog => priceLog.Child)
                    };

            foreach (var outlink in query)
            {
                int row = 0;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.ResultsReport, true))
                {
                    file.WriteLine("<Visited " + outlink.p + ">");
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.CrawlReport, true))
                {
                    file.WriteLine("<Visited " + outlink.p + ">");
                }
                
                foreach (var child in outlink.c)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.CrawlReport, true))
                    {
                        file.WriteLine("    <Link " + child + ">");
                    }
                    row++;
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.ResultsReport, true))
                {
                    file.WriteLine("    <No of links to Visted page: " + row + ">");
                }
            }
        }
    }
}
