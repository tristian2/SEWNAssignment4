using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace generateMatrix
{
    class PageRankItem
    {
        public string Page { get; set; }
        public double PageRank { get; set; }

        public PageRankItem(string Page, double PageRank)
        {
            this.Page = Page;
            this.PageRank = PageRank;
        }
    }
}
