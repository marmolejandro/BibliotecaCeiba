using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Domain.Entities
{
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        public string IdentificacionUsuario { get; set; } = string.Empty;
        public UserType TipoUsuario { get; set; }

        public Guid Isbn { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }
    }
}
