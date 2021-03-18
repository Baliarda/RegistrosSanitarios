using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistrosSanitarios.EntityModel.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        [Key]
        public string UsuarioAD { get; set; }
        public string Email { get; set; }
        public string Categoria { get; set; }
        public string IdSector { get; set; }
        public string Legajo { get; set; }
        public string Telefono { get; set; }
        public Usuario Padre { get; set; }
        public List<Usuario> Hijos { get; set; }
    }
}