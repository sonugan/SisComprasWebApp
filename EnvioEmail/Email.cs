using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EnvioEmail
{
    public class Email
    {

        public Email()
        {
            Adjuntos = new List<Attachment>();
        }

        //string subject, string text, string to, string from, string cc
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public string Para { get; set; }
        public string Desde { get; set; }
        public string Cc { get; set; }
        public IList<Attachment> Adjuntos { get; }

        public void AgregarAdjunto(System.IO.Stream file)
        {
            Adjuntos.Add(new Attachment(file, "ordenDeCompra"));
        }
    }
}
