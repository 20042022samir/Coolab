using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class MetaTag:BaseModel
    {
        public string MetaWord1 { get; set; }
        public string MetaWord2 { get; set;}
        public string MetaWord3 { get; set; }
        public string Title { get; set; }
    }
}
