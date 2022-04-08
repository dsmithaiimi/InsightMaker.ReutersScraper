using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace InsightMaker.ReutersScraper
{

    public class ScrapeJob
    {
        private static readonly HttpClient client = new HttpClient();
        private static List<string> guid_cache = new List<string>();
        private static string cache_file = @"guidcache.json";

        //async Task IJob.Execute(IJobExecutionContext context)
        public async void RunScraper(string channel, string outputpath)
        {

            var conf = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var user = conf.GetSection("Reuters")["user"];
            var password = conf.GetSection("Reuters")["password"];

            string[] cacheFile = null;
            // read cache file into list.
            if (File.Exists(cache_file))
            {
                cacheFile = File.ReadAllLines(cache_file);                
            }
            else
            {
                File.Create(cache_file);
                cacheFile = File.ReadAllLines(cache_file);
            }
            guid_cache = new List<string>(cacheFile);

            var auth = await GetAccessToken(user, password);
            var csv = await ProcessRepositories(outputpath, channel, auth);
            WriteCSV(csv.Item1, csv.Item2);
            WriteCacheToDisc();
        }

        #region private methods
        private static async Task<string> GetAccessToken(string user, string password)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var stringTask = client.GetStringAsync($@"http://commerce.reuters.com/rmd/rest/xml/login?username={user}&password={password}");
            var msg = await stringTask;
            var jsonText = JsonConvert.DeserializeObject<ReutersAuthToken.Rootobject>(msg);
            var token = jsonText.authToken.authToken.ToString();
            await WriteToConsole($"Access token granted: {token}");
            return token;
        }

        private static async Task<(List<string>, string)> ProcessRepositories(string outputpath, string channel, string authToken)
        {
            var stringTask = client.GetStringAsync($"http://rmb.reuters.com/rmd/rest/xml/items?channel={channel}&token={authToken}");

            var msg = await stringTask;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);
            string jsonText = JsonConvert.SerializeXmlNode(doc);

            var reuters = JsonConvert.DeserializeObject<ReutersResponse.Rootobject>(jsonText);
            var heads = "channel,dateCreated,geography,guid,headline,id,language,mediaType,priority,slug,source,version,article";

            List<string> csv = new List<string>();
            csv.Add(heads);
            int countItems = 0;
            foreach (var itm in reuters.results.result)
            {
                string geography, guid, headline, id, slug, source;
                Sanitise(itm, out geography, out guid, out headline, out id, out slug, out source);

                if (!guid_cache.Contains(guid))
                {
                    var articleText = await GetArticle(authToken, id);
                    // quick and dirty just to see if we can put the article text in the CSV.
                    var art = "\u0022" + articleText.Replace(",", "|").Replace("[", "").Replace("]", "").Replace("\n", " ").Replace("\r", "").Replace("</p>", " ").Replace("<p>", " ").Replace("\"", "").Replace("|", "") + "\u0022";

                    art = Regex.Replace(art, @"\s+", " ");

                    var outstring = $@"{itm.channel},{itm.dateCreated.ToString("yyyy-MM-ddThh:mm:ss.SSSZ")},{geography},{guid},{headline},{id},{itm.language},{itm.mediaType},{itm.priority},{slug},{source},{itm.version},{art}";
                    await WriteToConsole(headline);

                    csv.Add(outstring);
                    await AddToCacheList(guid);
                    countItems++;
                }
                else
                {
                    await WriteToConsole($"DUPLICATE: [{guid}] - {headline}", ConsoleColor.Red);
                }
            }
            var outputFileName = @$"{outputpath}\Reuters-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            await WriteToConsole($"{countItems} items written to file: {outputFileName}");
            return (csv, outputFileName);
        }

        private static async Task<string> GetArticle(string authToken, string id)
        {
            var article = await client.GetStringAsync($"https://rmb.reuters.com/rmd/rest/json/item?id={id.Replace("\"", "")}&token={authToken.Replace("\"", "")}");
            var json = JsonConvert.DeserializeObject<NewsML.Rootobject>(article);
            return json.body_xhtml.Trim();
        }

        private static void WriteCSV(List<string> csv, string outputFileName)
        {
            System.IO.File.WriteAllLines(outputFileName, csv);
        }

        private static async Task WriteToConsole(string text, ConsoleColor colour = ConsoleColor.White)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine($"[{DateTime.Now.ToString()}] - {text}");
        }

        private static async Task AddToCacheList(string guid)
        {
            // write guid to json file to be read into a list and compared so that we don't write duplicates out to the csv files.
            guid_cache.Add(guid);
        }

        private static void WriteCacheToDisc()
        {
            System.IO.File.WriteAllLines(cache_file, guid_cache);
        }

        private static void Sanitise(ReutersResponse.Result itm, out string geography, out string guid, out string headline, out string id, out string slug, out string source)
        {
            geography = (itm.geography == null) ? "" : itm.geography.ToString().Replace(",", "|").Replace("[", "").Replace("]", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("\"", "");
            guid = "\u0022" + itm.guid + "\u0022";
            headline = "\u0022" + itm.headline + "\u0022";
            id = "\u0022" + itm.id + "\u0022";
            slug = "\u0022" + itm.slug + "\u0022";
            source = "\u0022" + itm.source + "\u0022";
        }
        #endregion
    }
}
