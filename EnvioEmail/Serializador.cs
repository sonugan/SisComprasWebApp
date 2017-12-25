﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace EnvioEmail
{
    public class Serializador<T> where T : class
    {
        public Stream Serialize(T o)
        {
            Stream stream = new MemoryStream();
            XmlSerializer writer = new XmlSerializer(typeof(T));
            writer.Serialize(stream, o);
            return stream;
        }

        public string SerializeToString(T o)
        {
            Stream stream = Serialize(o);
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
