using PruebaIngresoBibliotecario.Domain.Entities;
using PruebaIngresoBibliotecario.Domain.Models;

namespace PruebaIngresoBibliotecario.Core.Interfaces
{
    public interface IMapperLoan
    {
        Loan CreateLoanInDtoToLoan(CreateLoanInDto newLoan, DateTime returnDate);
        CreateLoanOutDto LoanToCreateLoanOutDto(Loan Loan);
    }
}
