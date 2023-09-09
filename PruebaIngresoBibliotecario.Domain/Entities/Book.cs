using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Domain.Entities
{
    public class Book
    {
        [Key]
        public Guid Isbn { get; set; }
    }
}
