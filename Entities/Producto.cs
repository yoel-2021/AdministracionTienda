using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string DescripcionProducto { get; set; }
        public Marca objetoMarca { get; set; }
        public Categoria objetoCategoria { get; set; }
        public decimal Precio { get; set; }
        public string PrecioTexto { get; set; }
        public int Stock { get; set; }
        public string RutaImagen { get; set; }

        public string NombreImagen { get; set; }

        public bool Activo { get; set; }

        public string Base64 { get; set; }

        public string ExtensionImagen { get; set; }
    }
}
