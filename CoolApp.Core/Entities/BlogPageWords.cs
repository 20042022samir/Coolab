using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class BlogPageWords:BaseModel
    {
        public string Categories { get; set; }
        public string LastCategories { get; set; }
        public string CreateOwnBlog { get;set; }
        public string Filter { get; set; }
        public string FilterDesc { get; set; }
    }
}
