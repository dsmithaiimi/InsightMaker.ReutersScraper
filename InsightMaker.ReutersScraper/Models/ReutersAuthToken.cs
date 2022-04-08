using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightMaker.ReutersScraper
{
    internal class ReutersAuthToken
    {

        public class Rootobject
        {
            public Authtoken authToken { get; set; }
        }

        public class Authtoken
        {
            public string authToken { get; set; }
        }

    }
}
