using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace InsightMaker.ReutersScraper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Useage: ");
                    Console.WriteLine("");
                    Console.WriteLine("1 - Channeld (e.g. STK567");
                    Console.WriteLine("2 - Output path.");
                }
                else
                {
                    Console.WriteLine(@$"Params: {args[0]} and {args[1]}");
                    var sc = new ScrapeJob();
                    sc.RunScraper(args[0], args[1]);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
