using Modelos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Paginador<T> where T : class
    {
        public ListaPaginada<T> Paginar(IEnumerable<T> lista, Paginado paginado)
        {
            int paginaInicial = paginado.PaginaInicial.HasValue ? paginado.PaginaInicial.Value : 0;
            int tamanioHoja = paginado.TamanioHoja.HasValue ? paginado.TamanioHoja.Value : lista.Count();

            var listaPaginada = new ListaPaginada<T>();
            listaPaginada.Paginado = paginado;
            listaPaginada.Lista = lista
                .Skip(paginaInicial * tamanioHoja)
                .Take(tamanioHoja)
                .ToList();

            return listaPaginada;
        }
    }
}
