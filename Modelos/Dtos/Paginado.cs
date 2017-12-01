using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Dtos
{
    public class Paginado
    {
        public int? PaginaInicial { get; set; }
        public int? TamanioHoja { get; set; }
        public int CantidadDeRegistros { get; set; }
        public int RegistrosFiltrados { get; set; }
    }
}
