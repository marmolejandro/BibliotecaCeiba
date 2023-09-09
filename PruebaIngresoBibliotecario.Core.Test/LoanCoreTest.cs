using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PruebaIngresoBibliotecario.Core.Interfaces;
using PruebaIngresoBibliotecario.Core.Services;
using PruebaIngresoBibliotecario.Domain.Entities;
using PruebaIngresoBibliotecario.Domain.Models;

namespace PruebaIngresoBibliotecario.Core.Test
{
    [TestClass()]
    public class LoanCoreTest
    {
        private readonly Mock<ILoanRepository> _baseRepository = new();
        private readonly Mock<IMapperLoan> _baseMapper = new();

        private ILoanService _ILoanService;

        [TestInitialize]
        public void TestInitializer()
        {
            _ILoanService = new LoanService(
                _baseRepository.Object,
                _baseMapper.Object);
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

        [TestMethod()]
        public async Task CreatePrestamoAfiliado_Ok()
        {
            //Arrange
            CreateLoanInDto prestamoInDTO = new CreateLoanInDto()
            {
                Isbn = Guid.Parse("5c493736-2762-491b-ac8d-52d06f461560"),
                IdentificacionUsuario = "154515485",
                TipoUsuario = UserType.AFILIADO
            };

            List<Loan> prestamos = new List<Loan>();
            DateTime fechaMaximaDevolucion = CalculateReturnDate(UserType.AFILIADO);

            Loan prestamoResult = new Loan()
            {
                Id = Guid.Parse("c6880b61-eabf-4db8-8a4f-ae4213f8c930"),
                Isbn = Guid.Parse("2a9f7973-0d51-4376-8ef9-e66c32d71d04"),
                IdentificacionUsuario = "154515485",
                TipoUsuario = UserType.AFILIADO,
                FechaMaximaDevolucion = fechaMaximaDevolucion
            };

            CreateLoanOutDto prestamoOutDTOExpected = new CreateLoanOutDto()
            {
                Id = Guid.Parse("c6880b61-eabf-4db8-8a4f-ae4213f8c930"),
                FechaMaximaDevolucion = fechaMaximaDevolucion
            };

            _baseMapper.Setup(x => x.CreateLoanInDtoToLoan(It.IsAny<CreateLoanInDto>(), It.IsAny<DateTime>())).Returns(prestamoResult);
            _baseMapper.Setup(x => x.LoanToCreateLoanOutDto(It.IsAny<Loan>())).Returns(prestamoOutDTOExpected);

            _baseRepository.Setup(x => x.GetLoanByIdUser(prestamoInDTO.IdentificacionUsuario).Result).Returns(prestamos);
            _baseRepository.Setup(x => x.SaveLoan(It.IsAny<Loan>()).Result).Returns(prestamoResult);


            //Act
            var result = await _ILoanService.SaveLoan(prestamoInDTO);

            //Assert
            Assert.AreEqual(prestamoOutDTOExpected.Id, result.Id);
            Assert.AreEqual(prestamoOutDTOExpected.FechaMaximaDevolucion, result.FechaMaximaDevolucion);
        }

        

        [TestMethod()]
        public async Task GetPrestamoById_Ok()
        {
            //Arrange

            Guid idPrestamo = new();

            DateTime fechaMaximaDevolucion = CalculateReturnDate(UserType.AFILIADO);
            Loan prestamoResult = new Loan()
            {
                Id = idPrestamo,
                Isbn = Guid.Parse("2a9f7973-0d51-4376-8ef9-e66c32d71d04"),
                IdentificacionUsuario = "154515485",
                TipoUsuario = UserType.AFILIADO,
                FechaMaximaDevolucion = fechaMaximaDevolucion
            };

            _baseRepository.Setup(x => x.GetLoanById(idPrestamo).Result).Returns(prestamoResult);

            //Act
            var result = await _ILoanService.GetLoan(idPrestamo);

            //Assert
            Assert.AreEqual(prestamoResult.Id, result.Id);
            Assert.AreEqual(prestamoResult.Isbn, result.Isbn);
            Assert.AreEqual(prestamoResult.IdentificacionUsuario, result.IdentificacionUsuario);
            Assert.AreEqual(prestamoResult.TipoUsuario, result.TipoUsuario);
            Assert.AreEqual(prestamoResult.FechaMaximaDevolucion, result.FechaMaximaDevolucion);
        }
    }
}
