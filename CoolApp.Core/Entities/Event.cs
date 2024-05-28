using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class Event:BaseModel
    {
        public string Name { get; set; }
        public DateTime StartedDate { get; set; }
        public string Description { get; set; }
        public bool ForMainPage { get; set; }
        public string Image { get; set; }
        public bool SpecialEvent { get; set; }
        public List<EventContact> contacts { get;set; }
        public List<EventMessage> messages { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
    }
}
