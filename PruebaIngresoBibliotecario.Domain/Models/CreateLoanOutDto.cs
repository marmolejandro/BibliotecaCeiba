namespace PruebaIngresoBibliotecario.Domain.Models
{
    public class CreateLoanOutDto
    {
        public Guid Id { get; set; }
        public DateTime FechaMaximaDevolucion { get; set;}
    }
}
