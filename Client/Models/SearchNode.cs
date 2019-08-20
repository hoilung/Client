using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{

    public class SearchNode
    {
        public string Title { get; set; } = "";
        public string Rank { get; set; } = "0";

        public Uri Url { get; set; } = new Uri("about:blank");

        public string LinkUrl { get; set; } = "about:blank";

      

    }

}
