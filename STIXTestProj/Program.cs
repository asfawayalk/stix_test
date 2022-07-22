using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace STIXTestProj
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string[] arguments = new string[] { "dinneraroundyou.com", "fnotifyd", "krystynajasinska86@gmail.com", "google.com" };

            var results = IsSafeMultiple(arguments);
            PrintResult(results);
        }

        public static void PrintResult(List<Result> results)
        {
            Console.WriteLine("........................................................................................");
            foreach (Result result in results)
            {
                Console.WriteLine("Augument...-> " + result.Argument);
                Console.WriteLine("IsMalicious...-> " + result.IsMalicious);
                Console.WriteLine("Source...-> " + result.Source);
                Console.WriteLine("........................................................................................");
            }
        }

        public static List<Result> IsSafeMultiple(string[] args)
        {
            List<Result> results = new List<Result>();
            foreach (string str in args)
            {
                results.Add(IsSafe(str));
            }
            return results;
        }

        public static Result IsSafe(string url)
        {
            List<Result> results = new List<Result>();
            var allUrls = GetAllUrlsFromPattern();
            var allEmails = GetAllEmailsFromPattern();
            var allProcessNames = GetAllProcessNamesFromPattern();
            //return !allUrls.Exists(x => x == url) && !allEmails.Exists(x => x == url) && !allProcessNames.Exists(x => x == url);

            if (allUrls.Exists(x => x == url))
            {
                return new Result() { Source = "URL", IsMalicious = true, Argument = url };
            }
            else if (allEmails.Exists(x => x == url))
            {
                return new Result() { Source = "EMAIL", IsMalicious = true, Argument = url };

            }
            else if (allProcessNames.Exists(x => x == url))
            {
                return new Result() { Source = "PROCESS", IsMalicious = true, Argument = url };

            }
            else
            {
                return new Result() { Source = "-", IsMalicious = false, Argument = url };

            }
        }

        public static Root LoadStixFile()
        {
            Root res = JsonConvert.DeserializeObject<Root>(File.ReadAllText("stix.json"));
            return res;
        }

        public static List<string> GetAllStixPatterns()
        {
            Root stix = LoadStixFile();
            List<string> patterns = new List<string>();
            foreach (Object obj in stix.objects)
            {
                if (obj.pattern_type != null && obj.pattern_type.Equals("stix") && obj.pattern != null)
                {
                    patterns.Add(obj.pattern);
                }
            }
            return patterns;
        }

        public static List<string> GetAllUrlsFromPattern()
        {
            List<string> allPatterns = GetAllStixPatterns();
            List<string> allUrls = new List<string>();
            foreach (string str in allPatterns)
            {
                if (str.Contains("domain-name"))
                {
                    string url = ExtractUrlFromPattern(str);
                    allUrls.Add(url);
                }

            }
            return allUrls;
        }
        public static List<string> GetAllEmailsFromPattern()
        {
            List<string> allPatterns = GetAllStixPatterns();
            List<string> allUrls = new List<string>();
            foreach (string str in allPatterns)
            {
                if (str.Contains("email-addr"))
                {
                    string url = ExtractEmailFromPattern(str);
                    allUrls.Add(url);
                }

            }
            return allUrls;

        }
        public static List<string> GetAllProcessNamesFromPattern()
        {
            List<string> allPatterns = GetAllStixPatterns();
            List<string> allUrls = new List<string>();
            foreach (string str in allPatterns)
            {
                if (str.Contains("process:name"))
                {
                    string url = ExtractProcessNameFromPattern(str);
                    allUrls.Add(url);
                }

            }
            return allUrls;
        }

        public static string ExtractUrlFromPattern(string pattern)
        {
            return ExtractString(pattern, 20);
        }

        public static string ExtractEmailFromPattern(string pattern)
        {
            return ExtractString(pattern, 19);
        }
        public static string ExtractProcessNameFromPattern(string pattern)
        {
            return ExtractString(pattern, 15);
        }
        public static string ExtractString(string pattern, int start)
        {
            string url = pattern.Substring(start, pattern.Length - 2 - start);
            return url;
        }

    }
}