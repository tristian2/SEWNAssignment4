using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace generateMatrix
{
    class Program
    {
        public static string currentPage = "";
        public static int numberOfPages = 0;
        public static List<String> Pages = new List<string>();
        public static string aVisitedPage = "";
        public static List<int[]> matrix = new List<int[]>();
        public static List<Relation> relationShipsList = new List<Relation>();
        public static List<PageRankItem> pageRanks = new List<PageRankItem>();

        static void Main(string[] args)
        {

            String urlToProcess = (ConfigurationManager.AppSettings["url"]);
            Console.WriteLine("Parsing Links...");
            ParseHtmlLinks(urlToProcess);
            //PrintCollection<string>(Pages);
            Console.WriteLine("Creating matrix...");
            CreateAdjacencyMatrix(numberOfPages, Pages);
            PrintMatrix(matrix);
            Console.WriteLine("Output matrix to file");
            DumpMatrix(matrix);
            Console.WriteLine("Calculating page rank 0.15.");
            CalcPageRank(matrix,0.15);
            Console.Write("output pagerank to file.");
            DumpPageRanks("015");
            Console.WriteLine("Calculating page rank 0.75.");
            CalcPageRank(matrix, 0.75);
            DumpPageRanks("075");
            Console.WriteLine("Calculating page rank 0.5.");
            CalcPageRank(matrix, 0.5);
            DumpPageRanks("050");
            Console.WriteLine("Calculating page rank 0.25.");
            CalcPageRank(matrix, 0.25);
            DumpPageRanks("025");

            Console.WriteLine("Calculating page rank 1.");
            CalcPageRank(matrix, 1.00);
            Console.Write("output pagerank to file.");
            DumpPageRanks("100");
            Console.WriteLine("Calculating page rank 0.00.");
            CalcPageRank(matrix, 0.00);
            DumpPageRanks("000");

            
            Console.Write("Completed.. Press a key to exit");
            Console.Read();
        }

        private static void CalcPageRank(List<int[]> Matrix, double factor)
        {
            int iterations = int.Parse(ConfigurationManager.AppSettings["iterations"]);
            int rounds = 0;
            double pageRank = 0.0;

            while (rounds < iterations)
            {
                Console.Write(".");
                int count = 0;
                foreach (var outlink in Pages)
                {
                    int[] row = Matrix[count];

                    for (int i = 0; i < row.Length; i++)
                    {

                        if (row[i] == 1)
                        {
                            pageRank = (double)1 - factor * GetPageFactor(Matrix, i, row.Sum());
                            pageRanks[i].PageRank = pageRank;
                            Log("PageRank: " + pageRank);
                        }
                        else
                        {
                            Log("ignored");
                        }
                    }

                    count++;
                }
                rounds++;
            }
        }

        private static double GetPageFactor(List<int[]> Matrix, int column, int denominator){
            double pagerankfactor = 0.0;
            foreach (var item in matrix){
                pagerankfactor = pagerankfactor+ (double) item[column] / denominator * pageRanks[column].PageRank;
            }

            return pagerankfactor;
        }

        
        private static void Statistics(List<int[]> Matrix)
        {

            int noInlinks = 0;

            int count = 0;
            foreach (var outlink in Pages)
            {
                int[] row = Matrix[count];
                        
                for(int i=0; i<row.Length;i++)
                {
                    if (row[i]==1)
                        noInlinks++;    
                }
                count++;
            }

        }

        private static void DumpPageRanks(string factor)
        {
            using (FileStream fs = new FileStream(@"c:\pageRank"+factor+".txt", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    int count = 0;

                    foreach (var item in pageRanks)
                    {
                        w.WriteLine(item.Page+","+item.PageRank);
                        count++;
                    }
                }
            }

        }

        private static void DumpMatrix(List<int[]> Matrix)
        {
            using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["matrixDump"], FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    int count = 0;

                    foreach (var outlink in Pages)
                    {
                        int[] row = Matrix[count];
                        string rowString = string.Empty;
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (row[i]==1)
                                rowString = rowString + "1";
                            else
                                rowString = rowString + "0";
                        }
                        w.WriteLine(rowString);
                        count++;
                    }
                }
            }
        }

        private static void PrintMatrix(List<int[]> Matrix)
        {
            using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["matrixPath"], FileMode.Create)) 
            { 
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8)) 
                {

                    w.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
                    w.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">");
                    w.WriteLine("<head>");
                    w.WriteLine("<title>World Wide Web Consortium (W3C)</title>");
                    w.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    w.WriteLine("<body>");
                    w.WriteLine("<div id='Header'>Matrix</div>");
                    w.WriteLine("<div id='Matrix'>");
                    w.WriteLine("<table><tr><td>&nbsp;</td>"); 
                    /*
                    1) create html File
                    2) create header row containing column header
                    3) create a row with the page id
                     * 4) then write the contents of the matrix row
                     * repeat
                     * 5) add a pagesid to url legend table at the bootom
                     * 
                     */
                    var count = 0;
                    foreach (var inlink in Pages)
                    {
                        w.WriteLine("<td>"+count.ToString()+"</td>");
                        count++;
                    }

                    w.WriteLine("</tr><tr>");
                    count = 0;

                    foreach (var outlink in Pages)
                    {
                        w.WriteLine("<td>"+count.ToString()+"</td>");

                        int[] row = Matrix[count];
                        for(int i=0; i<row.Length;i++)
                        {
                            if (row[i]==1)
                                w.WriteLine("<td>T</td>");     
                            else
                                w.WriteLine("<td>&nbsp;</td>");
                        }
                        w.WriteLine("</tr>");
                        count++;
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</div>");

                    //legend
                    count = 0;
                    w.WriteLine("<div id='legendHeader'>Legend</div>");
                    w.WriteLine("<div id='legendTable'>");
                    w.WriteLine("<table>");
                    w.WriteLine("<tr><td>id</td><td>uri</td></tr>");
                    foreach (var inlink in Pages)
                    {
                        w.WriteLine("<tr><td>" + count.ToString() + "</td><td>" + inlink + "</td></tr>");
                        count++;
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</div>");
                    w.WriteLine("</body>");
                    w.WriteLine("</html>");

                } 
            } 

        }
        public static void CreateAdjacencyMatrix(int numberOfPages, List<String> Pages)
        {

            int outlinkCount=0;
            foreach (var outlink in Pages) //outlinks
            {
                int[] row = new int[Pages.Count];

                int inlinkCount=0;
                foreach (var inlink in Pages)// inlinks
                {
                    if (!(outlink == inlink))
                    {
                        //check to see if the outlink is a child?  if so, then set the index of the row outlink
                        var children =
                        from p in relationShipsList
                        where p.Parent == outlink
                        select p;

                        if (!(children.Count() == 0))
                        {
                            foreach (var child in children)
                            {
                                if (inlink == child.Child)
                                {
                                    row[inlinkCount] = 1;
                                    //too spped up,, remove from the relationships list
                                    //relationShipsList.RemoveAll(Parent => Parent.Parent == outlink);
                                    break;
                                }                                 
                            }
                        }
                    }
                    else
                    {
                        row[inlinkCount] = 0;
                    }
                    inlinkCount++;
                }
                matrix.Add(row);
                outlinkCount++;
                
            }

        }

        public static void PrintCollection<T>(IEnumerable<T> col)
        {
            foreach (var outlink in col)
                Console.WriteLine(outlink); 
        }


        static void ParseHtmlLinks(string urlToProcess)
        {
            try
            {
                Uri url = new Uri(urlToProcess);
                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                oReq.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";

                HttpWebResponse resp = (HttpWebResponse)oReq.GetResponse();

                if (resp.ContentType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        var resultStream = resp.GetResponseStream();
                        
                        string line;
                        using (StreamReader reader = new StreamReader(resultStream))
                        {
                       
                            while ((line = reader.ReadLine()) != null)  //NEED TO UNCOMMENT THIS FOR REAL RUN
                            //while (numberOfPages<10) //TEST LINE FOR SHORTER RUN
                            {
                                //line = reader.ReadLine();//TEST LINE FOR SHORTER RUN

                                currentPage = line;
                                if (currentPage.IndexOf("?") > 0)
                                    currentPage = currentPage.Remove(currentPage.IndexOf("?"));
                                if (currentPage.IndexOf("#") > 0)
                                    currentPage = currentPage.Remove(currentPage.IndexOf("#"));

                                if (currentPage.StartsWith("Visited: "))
                                {
                                    aVisitedPage = currentPage.Replace("Visited: ", "");
                                    currentPage = currentPage.Replace("Visited: ", "").Replace("    ", "");
                                    Pages.Add(currentPage);
                                    PageRankItem pageRankItem = new PageRankItem(currentPage, 1);
                                    pageRanks.Add(pageRankItem);
                                    numberOfPages++;
                                }
                                else //it a link
                                {
                                    Relation relation = new Relation();
                                    currentPage = currentPage.Replace("Link: ", "").Replace("    ", "");

                                    if (!currentPage.Contains("http://"))
                                    {
                                        if (aVisitedPage.EndsWith(".asp") || aVisitedPage.EndsWith(".htm")
                                            || aVisitedPage.EndsWith(".html") || aVisitedPage.EndsWith(".php")
                                            || aVisitedPage.EndsWith(".rdf") || aVisitedPage.EndsWith(".tgz")
                                            || aVisitedPage.EndsWith(".txt") || aVisitedPage.EndsWith(".xml")) {
                                            aVisitedPage = aVisitedPage.Remove(aVisitedPage.LastIndexOf("/")) + "/";
                                        }
                                        currentPage = aVisitedPage  + currentPage;
                                    }

                                    relation.Parent = aVisitedPage;
                                    relation.Child = currentPage;
                                    relationShipsList.Add(relation);
                                }

                            }
                            
                            Pages = Pages.Distinct().ToList(); // remove duplicates
                        }
                       
                    }
                    catch (System.Net.WebException ex)
                    {
                        Log(ex.Message);
                    }

                }

            }
            catch (System.Net.WebException e)
            {
                Log(e.ToString());
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }

        public static string AbsolutizeUri(string uri)
        {
            if (!uri.StartsWith("https://") || !uri.StartsWith("http://") || !uri.StartsWith("ftp://"))
            {
                uri = currentPage + uri;
            }
            return uri;

        }

        public static void Log(String line)
        {
            using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["log"], FileMode.Append))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(DateTime.Now.ToFileTime() + ":" + line);
                }
            }
        }
        
    }
}
