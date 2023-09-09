using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Core.Interfaces;
using PruebaIngresoBibliotecario.Domain.Entities;

namespace PruebaIngresoBibliotecario.Core.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        PersistenceContext _context;
        public LoanRepository(PersistenceContext context)
        {
            _context = context;
        }

        public async Task<Loan?> GetLoanById(Guid Id)
        {
            return await _context.Loan.Where(r => r.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Loan> SaveLoan(Loan Loan)
        {
            _context.Loan.Add(Loan);
            await _context.SaveChangesAsync();
            return Loan;
        }

        public async Task<List<Loan>> GetAll()
        {
            return await _context.Loan.ToListAsync();
        }

        public async Task<List<Loan>> GetLoanByIdUser(string IdUser)
        {
            return await _context.Loan.Where(r => r.IdentificacionUsuario == IdUser).ToListAsync();
        }
    }
}
