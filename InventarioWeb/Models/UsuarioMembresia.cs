using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    public class UsuarioMembresia
    {
        public int NegocioId { get; set; }
        public int UsuarioMembresiaId { get; set; }
        public int UsuarioId { get; set; }
        public int MembresiaId { get; set; }
    }
}