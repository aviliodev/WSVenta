using System;
using System.Collections.Generic;

namespace WSVenta.Models
{
    public partial class Ventum
    {
        public Ventum()
        {
            Conceptos = new HashSet<Concepto>();
        }

        public int? IdCliente { get; set; }
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? Total { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual ICollection<Concepto> Conceptos { get; set; }
    }
}
