using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioWeb.Models.Dtos
{
    public class ProductoDto
    {
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public double Costo { get; set; }
        public string Costoformat { get { return Costo.ToString("C2"); } }
        public double Precio { get; set; }
        public string PrecioFormat { get { return Costo.ToString("C2"); } }
        public int Contenido { get; set; }
        public string Unidad { get; set; }
        public string ProveedorNombre { get; set; }
    }
}