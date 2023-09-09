using PruebaIngresoBibliotecario.Domain.Entities;
using PruebaIngresoBibliotecario.Domain.Models;

namespace PruebaIngresoBibliotecario.Core.Interfaces
{
    public interface ILoanService
    {
        public Task<Loan> GetLoan(Guid Id);
        public Task<CreateLoanOutDto> SaveLoan(CreateLoanInDto Loan);
    }
}
