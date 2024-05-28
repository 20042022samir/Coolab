using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class Subject:BaseModel
    {
        public string Name { get; set; }
        public string Image { get; set; }   
        public List<Course> Courses { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
