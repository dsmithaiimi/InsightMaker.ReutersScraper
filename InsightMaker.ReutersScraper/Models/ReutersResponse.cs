using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightMaker.ReutersScraper
{
    public class ReutersResponse
    {
        public class Rootobject
        {
            public Xml xml { get; set; }
            public Results results { get; set; }
        }

        public class Xml
        {
            public string version { get; set; }
            public string encoding { get; set; }
            public string standalone { get; set; }
        }

        public class Results
        {
            public Result[] result { get; set; }
            public string pollToken { get; set; }
            public Status status { get; set; }
        }

        public class Status
        {
            public string code { get; set; }
        }

        public class Result
        {
            public string id { get; set; }
            public string guid { get; set; }
            public string version { get; set; }
            public DateTime dateCreated { get; set; }
            public string slug { get; set; }
            public string source { get; set; }
            public string language { get; set; }
            public string headline { get; set; }
            public string mediaType { get; set; }
            public string priority { get; set; }
            public object geography { get; set; }
            public string channel { get; set; }
            public string author { get; set; }
        }



    }
}
