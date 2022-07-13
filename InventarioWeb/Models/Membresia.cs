using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models
{
    public class Membresia : Entity
    {
        public int MembresiaId { get; set; }
        public string Nombre { get; set; }
    }
}