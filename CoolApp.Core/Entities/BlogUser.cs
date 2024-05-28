using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class BlogUser:BaseModel
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }
    }
}
