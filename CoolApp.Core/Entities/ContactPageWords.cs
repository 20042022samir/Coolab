using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolApp.Core.Entities
{
    public class ContactPageWords:BaseModel
    {
        public string Contact { get;set; }  
        public string ContactDesc { get; set; }
        public string Email { get; set; }
        public string Password { get;set; }
        public string Name { get;set; }
        public string Surname { get;set; }
        public string ConfirmPassword { get;set; }
        public string Muellimem { get; set; }
        public string Telebeyem { get; set; }
        public string Register { get; set; }
        public string Tedbirler { get; set; }
        public string TedbirlerDesc { get; set; }
    }
}
