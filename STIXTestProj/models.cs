using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STIXTestProj
{
    public class Object
    {
        public string type { get; set; }
        public string spec_version { get; set; }
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool is_family { get; set; }
        public List<string> indicator_types { get; set; }
        public string pattern { get; set; }
        public string pattern_type { get; set; }
        public string pattern_version { get; set; }
        public DateTime? valid_from { get; set; }
        public string relationship_type { get; set; }
        public string source_ref { get; set; }
        public string target_ref { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string id { get; set; }
        public List<Object> objects { get; set; }
    }

    public class Result
    {
        public string Source { get; set; }
        public bool IsMalicious { get; set; }
        public string Argument { get; set; }
    }
}
