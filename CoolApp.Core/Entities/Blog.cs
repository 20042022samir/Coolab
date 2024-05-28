using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
   public class Blog:BaseModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public bool CreatedByUser { get;set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<BlogCategory>? BlogCategories { get; set; }
        [NotMapped]
        public List<int>? CategoryIds { get; set; }
        public bool IsAproved { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
