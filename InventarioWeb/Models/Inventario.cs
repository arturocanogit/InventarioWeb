using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    [Table("Inventario")]
    public class Inventario : Entity
    {
        [Key, Column(Order = 0)]
        public int NegocioId { get; set; }
        [Key, Column(Order = 1)]
        public int InventarioId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public virtual Producto Producto { get; set; }
    }
}