using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class CooursePrice:BaseModel
    {
        public double FromPrice { get; set; }
        public double ToPrice { get; set; }
    }
}
