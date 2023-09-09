using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Core.Interfaces;
using PruebaIngresoBibliotecario.Domain.Models;
using PruebaIngresoBibliotecario.Domain.Entities;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        ILoanService _loanService;
        public PrestamoController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(CreateLoanInDto newLoan)
        {
            CreateLoanOutDto loanOut = await _loanService.SaveLoan(newLoan);

            return Ok(loanOut);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchLoan(Guid id)
        {
            Loan loan = await _loanService.GetLoan(id);

            return Ok(loan);
        }
    }
}
