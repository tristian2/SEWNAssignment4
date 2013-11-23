using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace ParseHtmlLinks
{
    class Program
    {
        public static string  currentPage = "";
        public static int numberOfPages = 0;
        public static List<String> Pages = new List<string>();
        public static string aVisitedPage = "";

        static void Main(string[] args)
        {
            String urlToProcess = ("http://www.dcs.bbk.ac.uk/~martin/sewn/ls4/sewn-crawl-2013.txt");
            parseHtmlLinks(urlToProcess);
            
            /*
            ParseHtmlLinks phl = new ParseHtmlLinks(urlToParse);

            Console.WriteLine("Parsing links from: " + phl.getUrlToParse());
            phl.parseHtmlLinks(urlToParse);

            Report report = new Report(phl.ReportList);
            report.CrawlReport = @"C:\Users\Public\Crawl.txt";
            report.ResultsReport = @"C:\Users\Public\Results.txt";
            report.generateReport();
             * */
            Console.Write("Report run, check \nPress any key to exit");
            Console.Write("number of links:" + numberOfPages.ToString());
            PrintCollection<string>(Pages);
            Console.Read();
        }


        public static void PrintCollection<T>(IEnumerable<T> col)
        {
            foreach (var item in col)
                Console.WriteLine(item); // Replace this with your version of printing
        }


        static void parseHtmlLinks(string urlToProcess)
        {

            //var adjacencyMatrix = new Array [,];


            try
            {

                Uri url = new Uri(urlToProcess);
                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                oReq.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";

                HttpWebResponse resp = (HttpWebResponse)oReq.GetResponse();
                Console.Write(resp);


                if (resp.ContentType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        var resultStream = resp.GetResponseStream();
                        
                        string line;
                        using (StreamReader reader = new StreamReader(resultStream))
                        {
                       
                            while ((line = reader.ReadLine()) != null)
                            {
                                
                                //Console.WriteLine(line); // Write to console.

                                if (line.StartsWith("Visited: "))
                                {
                                    aVisitedPage = line.Replace("Visited: ", "");
                                    currentPage = line.Replace("Visited: ", "");
                                    Console.WriteLine(currentPage); // Write to console.
                                }
                                else //it a link
                                {
                                    //Console.WriteLine(absolutizeUri(line.Replace("Link: ", "").Replace("    ", ""))); // Write to console.
                                    currentPage = line.Replace("Link: ", "");
                                    if (!currentPage.Contains("http://"))
                                    {
                                        currentPage = aVisitedPage + currentPage;
                                    }
                                    
                                }
                                Pages.Add(currentPage);
                                numberOfPages++;

                            }
                        }
                       
                    }
                    catch (System.Net.WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    /*
                    HtmlDocument doc = new HtmlDocument();
                    try
                    {
                        var resultStream = resp.GetResponseStream();
                        doc.Load(resultStream); // The HtmlAgilityPack
                    }
                    catch (System.Net.WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Url", url);
                        throw;
                    }

                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes(@"//a[@href]"))
                    {

                        HtmlAttribute att = link.Attributes["href"];
                        if (att == null) continue;
                        string href = att.Value;
                        if (href.StartsWith("javascript", StringComparison.InvariantCultureIgnoreCase)) continue;      // ignore javascript on buttons using a tags

                        Uri urlNext = new Uri(href, UriKind.RelativeOrAbsolute);

                        // Make it absolute if it's relative
                        //TODO handle absolute urls with a different domain
                        if (!urlNext.IsAbsoluteUri)
                        {
                            urlNext = new Uri(urlRoot + urlNext);
                        }

                        if (!this.htmlLinks.Contains(urlNext.ToString()))
                        {
                            addHtmlLink(urlToParse, urlNext.ToString());// keep track of every page we've handed off             

                            Uri u = new Uri(urlRoot);
                            parseHtmlLinks(urlNext.ToString());
                        }
                        if (!alreadyInHtmlLinks(urlNext.ToString()))
                            addHtmlLink(urlToParse, urlNext.ToString());

                    }
                    group++;
                     * */
                }





            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string absolutizeUri(string uri)
        {
            if (!uri.StartsWith("https://") || !uri.StartsWith("http://") || !uri.StartsWith("ftp://"))
            {
                uri = currentPage + uri;
            }
            return uri;

        }
        
    }
}
