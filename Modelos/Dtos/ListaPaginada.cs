using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Dtos
{
    public class ListaPaginada<T> where T : class
    {
        public Paginado Paginado { get; set; }
        public List<T> Lista { get; set; }
    }
}
