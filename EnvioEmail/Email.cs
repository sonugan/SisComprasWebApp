using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioEmail
{
    public class Email
    {
        //string subject, string text, string to, string from, string cc
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public string Para { get; set; }
        public string Desde { get; set; }
        public string Cc { get; set; }
    }
}
