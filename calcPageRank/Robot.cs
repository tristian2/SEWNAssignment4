using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace ParseHtmlLinks
{
    class Robot
    {
        private List<String> robotExclusions;

        public List<String> getRobotExclusions()
        {
            return robotExclusions;
        }

        public void setHtmlLinks(List<String> robotExclusions)
        {
            this.robotExclusions = robotExclusions;
        }

        // ******************************************************************************************
        // ** HELPER METHODS.                                                                      **
        // ******************************************************************************************

        // Adds a HTML link found in the webpage to collection htmlLinks.
        public void addRobotExclusion(String robotExclusion)
        {
            this.robotExclusions.Add(robotExclusion);
        }

        // Lists the links in the webpage.
        public void listRobotExclusions()
        {
            for (int i = 0; i < this.getRobotExclusions().Count(); i++)
            {
                Console.WriteLine((i + 1) + ": " + this.getRobotExclusions()[i]);
            }
        }

        // Parses HTML links in the webpage.
        public void parseRobotsText(string robotUrl)
        {
            try
            {

                robotUrl = robotUrl+"robots.txt";

                Uri url = new Uri(robotUrl);
                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                oReq.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";
                
                WebClient client = new WebClient();
                client.Headers.Set("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0");
                string str = client.DownloadString(robotUrl);

                string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("Disallow: "))
                        lines[i] = lines[i].Substring(10, lines[i].Length-10);
                }

                List<string> list = new List<string>(lines);
                list.Remove("User-agent: *");
                robotExclusions = list;
                
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
    }
}
