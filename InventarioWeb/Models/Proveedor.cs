using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    [Table("Proveedores")]
    public class Proveedor : Entity
    {
        [Key, Column(Order = 0)]
        public int NegocioId { get; set; }
        [Key, Column(Order = 1)]
        public int ProveedorId { get; set; }
        [Required, MaxLength(64)]
        public string Nombre { get; set; }
        [MaxLength(256)]
        public string Direccion { get; set; }
        [MaxLength(32)]
        public string Email { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        [MaxLength(15)]
        public string Rfc { get; set; }
    }
}