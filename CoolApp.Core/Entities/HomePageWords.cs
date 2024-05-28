using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class HomePageWords:BaseModel
    {
        public string Saheler { get; set; }
        public string DescriptionSaheler { get; set; }
        public string SpecialCourse { get; set; }
        public string Semiarlar { get; set; }   
        public string SeminarlarDescription { get; set; }
        public string SpecialEvent { get; set; }
        public string Teachers { get; set; }
        public string TeachersDesc { get; set; }
        public string Comment { get; set; }
        public string CommentDescription { get; set; }
        public string Blogs { get; set; }
        public string BlogsDescription { get; set; }
        public string MakeContact { get; set; }
       
    }
}
