using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightMaker.ReutersScraper
{
    internal class NewsML
    {
        public class Rootobject
        {
            public string[] destination { get; set; }
            public string productlabel { get; set; }
            public string representationtype { get; set; }
            public string copyrightholder { get; set; }
            public string copyrightnotice { get; set; }
            public string credit { get; set; }
            public string dateline { get; set; }
            public string profile { get; set; }
            public string[] signal { get; set; }
            public DateTime firstcreated { get; set; }
            public string headline { get; set; }
            public string language { get; set; }
            public string mimetype { get; set; }
            public string slug { get; set; }
            public string caption { get; set; }
            public Subject[] subject { get; set; }
            public Subjectlocation subjectlocation { get; set; }
            public Altid altid { get; set; }
            public string type { get; set; }
            public int urgency { get; set; }
            public string uri { get; set; }
            public string usn { get; set; }
            public string version { get; set; }
            public DateTime versioncreated { get; set; }
            public string versionedguid { get; set; }
            public int wordcount { get; set; }
            public string body_xhtml { get; set; }
            public Rendition[] renditions { get; set; }
        }

        public class Subjectlocation
        {
            public string city { get; set; }
            public string countrycode { get; set; }
        }

        public class Altid
        {
            public string otr { get; set; }
        }

        public class Subject
        {
            public string code { get; set; }
            public string name { get; set; }
        }

        public class Rendition
        {
            public string mimetype { get; set; }
            public string uri { get; set; }
            public string versionedguid { get; set; }
            public string filename { get; set; }
        }

    }
}
