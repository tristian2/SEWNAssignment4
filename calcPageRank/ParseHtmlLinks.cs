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

    public class ParseHtmlLinks
    {

        // ******************************************************************************************
        // ** INSTANCE VARIABLES.                                                                  **
        // ******************************************************************************************

        private String urlToParse;
        private String urlRoot;
        private List<String> htmlLinks;
        
        public HtmlDocument HtmlDocument { get; internal set; }
        private int depth = 0;
        private int maxDepth = 3;
        private int group = 1;

        private List<Report> reportList = new List<Report>();

        internal List<Report> ReportList
        {
            get { return reportList; }
            set { reportList = value; }
        }

        Robot rob = new Robot();

        // ******************************************************************************************
        // ** CONSTRUCTOR.                                                                         **
        // ******************************************************************************************

        public ParseHtmlLinks(String urlToParse)
        {
           
            this.urlRoot = urlToParse;
            this.urlToParse = urlToParse;
            this.htmlLinks = new List<String>();            
            rob.parseRobotsText(urlToParse);
        }

        // ******************************************************************************************
        // ** ACCESSOR AND MUTATOR METHODS.                                                        **
        // ******************************************************************************************

        public String getUrlToParse()
        {
            return urlToParse;
        }

        public void setUtlToParse(String urlToParse)
        {
            this.urlToParse = urlToParse;
        }

        public List<String> getHtmlLinks()
        {
            return htmlLinks;
        }

        public void setHtmlLinks(List<String> htmlLinks)
        {
            this.htmlLinks = htmlLinks;
        }

        public bool alreadyInHtmlLinks(String url)
        {
            if (this.htmlLinks.Contains(url))
                return true;
            else
                return false;
        }
        

        // ******************************************************************************************
        // ** HELPER METHODS.                                                                      **
        // ******************************************************************************************

        // Adds a HTML link found in the webpage to collection htmlLinks.
        public void addHtmlLink(String parentUrl, String htmlLink)
        {
            foreach (var item in rob.getRobotExclusions())
            {
                if (htmlLink.Contains(item))
                {
                    return;
                }
            }

            Report report = new Report() { Parent = parentUrl, Child = htmlLink };
            reportList.Add(report);
            this.htmlLinks.Add(htmlLink);
        }

        // Lists the links in the webpage.
        public void listHtmlLinks()
        {
            for (int i = 0; i < this.getHtmlLinks().Count(); i++)
            {
                Console.WriteLine((i + 1) + "::::::: " + this.getHtmlLinks()[i]);
            }
        }

        // Parses HTML links in the webpage.
        public void parseHtmlLinks(string urlToParse)
        {
            depth++;
            if (depth > maxDepth)
                return;
            
            try
            {

                Uri url = new Uri(urlToParse);
                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                oReq.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";

                HttpWebResponse resp = (HttpWebResponse)oReq.GetResponse();

                if (resp.ContentType.StartsWith("text/html", StringComparison.InvariantCultureIgnoreCase))
                {
                    
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
                            addHtmlLink(urlToParse,urlNext.ToString());// keep track of every page we've handed off             

                            Uri u = new Uri(urlRoot);
                            parseHtmlLinks(urlNext.ToString());
                        }
                        if(!alreadyInHtmlLinks(urlNext.ToString()))
                            addHtmlLink(urlToParse,urlNext.ToString());                        

                    }
                    group++;
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
            return;
        }
    }
}