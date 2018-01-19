using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Foto
    {
        public Foto(string path)
        {
            Image img = Image.FromFile(path);
            ImageConverter converter = new ImageConverter();
            ToBase64 = Convert.ToBase64String((byte[])converter.ConvertTo(img, typeof(byte[])));
            Path = path;
        }
        
        public string Extension
        {
            get
            {
                var parts = Path.Split('.');
                return parts[parts.Length - 1];
            }
        }
        public string Path { get; private set; }
        public string ToBase64 { get; } 
    }
}
