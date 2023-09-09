using PruebaIngresoBibliotecario.Core.Exceptions;
using PruebaIngresoBibliotecario.Core.Interfaces;
using PruebaIngresoBibliotecario.Domain.Entities;
using PruebaIngresoBibliotecario.Domain.Models;

namespace PruebaIngresoBibliotecario.Core.Services
{
    public class LoanService : ILoanService
    {
        ILoanRepository _loanRepository;
        IMapperLoan _mapperLoan;
        public LoanService(ILoanRepository loanRepository, IMapperLoan mapperLoan)
        {
            _loanRepository = loanRepository;
            _mapperLoan = mapperLoan;
        }

        public async Task<Loan> GetLoan(Guid Id)
        {
            Loan? loan = await _loanRepository.GetLoanById(Id);

            if (loan == null)
            {
                string mensaje = $"El prestamo con id {Id} no existe";
                throw new NotFoundBusinessException(mensaje);
            }

            return loan;
        }

        public async Task<CreateLoanOutDto> SaveLoan(CreateLoanInDto newLoan)
        {
            string message = "";
            if (!ValidateUserType(newLoan.TipoUsuario))
            {
                message = $"El tipo de usuario no esta definido";
                throw new BadRequestBussinessException(message);
            }
            else if (!await ValidateLoan(newLoan.IdentificacionUsuario))
            {
                message = $"El usuario con identificacion {newLoan.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo";
                throw new BadRequestBussinessException(message);
            }

            DateTime returnDate = CalculateReturnDate(newLoan.TipoUsuario);
            Loan loanMapped = _mapperLoan.CreateLoanInDtoToLoan(newLoan, returnDate);
            Loan loanSaved = await _loanRepository.SaveLoan(loanMapped);

            CreateLoanOutDto loanOutMapped = _mapperLoan.LoanToCreateLoanOutDto(loanSaved);

            return loanOutMapped;
        }


        private DateTime CalculateReturnDate(UserType UserType)
        {
            var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            DateTime returnDate = DateTime.Now;

            int returnDays = (int)(ReturnDays)Enum.Parse(typeof(ReturnDays), UserType.ToString());

            for (int i = 0; i < returnDays;)
            {
                returnDate = returnDate.AddDays(1);
                i = weekend.Contains(returnDate.DayOfWeek) ? i : ++i;
            }

            return returnDate;
        }


        private async Task<bool> ValidateLoan(string IdUser)
        {
            List<Loan> loans = await _loanRepository.GetLoanByIdUser(IdUser);

            return loans.Where(r => (int)r.TipoUsuario == 3).ToList().Count == 0;
        }

        private bool ValidateUserType(UserType UserType)
        {
            if(!Enum.IsDefined(typeof(UserType), UserType))
                return false;
            
            return true;
        }
    }
}
