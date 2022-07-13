using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    public class Usuario : Entity
    {
        [Key, Column(Order = 0)]
        public int NegocioId { get; set; }
        [Key, Column(Order = 1)]
        public int UsuarioId { get; set; }
        public string IdentityUserId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public virtual Negocio Negocio { get; set; }
    }
}