using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    [Table("Almacen")]
    public class Almacen : Entity
    {
        [Key, Column(Order = 0)]
        public int NegocioId { get; set; }
        [Key, Column(Order = 1)]
        public int AlmacenId { get; set; }
        [Required, MaxLength(64)]
        public string Nombre { get; set; }
    }
}