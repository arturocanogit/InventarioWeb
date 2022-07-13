using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    [Table("Negocios")]
    public class Negocio : Entity
    {
        [Key]
        public int NegocioId { get; set; }
        [Required, StringLength(64)]
        public string Nombre { get; set; }
    }
}