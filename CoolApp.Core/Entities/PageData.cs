using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class PageData:BaseModel
    {
        public string Logo { get; set; }
        public string Email { get;set; }
        public string PhoneNumber { get;set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Youtube { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
