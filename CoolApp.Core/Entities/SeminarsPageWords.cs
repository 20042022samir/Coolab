using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class SeminarsPageWords:BaseModel
    {
        public string Filter { get; set; }
        public string FilterDesc { get;set; }
        public string Seminar { get; set; } 
        public string SeminarDesc { get;set; }
    }
}
