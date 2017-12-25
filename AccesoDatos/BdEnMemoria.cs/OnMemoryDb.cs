using Modelos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdEnMemoria
{
    public class OnMemoryDb<T> where T : IEntiy
    {
        IList<T> Objects { get; set; }

        public int Insert(T o)
        {
            if(o.ID == 0)
            {
                o.ID = Objects.Count();
                Objects.Add(o);
            }
            return o.ID;
        }

        public void Update(T o)
        {
            var oldObj = Objects.Where(obj => obj.ID == o.ID).FirstOrDefault();
            if(oldObj != null)
            {
                Objects.Remove(oldObj);
            }
            Objects.Add(o);
        }

        public void Delete(T o)
        {
            var oldObj = Objects.Where(obj => obj.ID == o.ID).FirstOrDefault();
            if (oldObj != null)
            {
                Objects.Remove(oldObj);
            }
        }
    }
}
