using Amazon.SimpleEmail.Model;
using Caramel.Pattern.Services.Application.Services.Partners;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Enums.Parterns;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Bucket;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System;
using System.Linq.Expressions;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Partners
{
    public class PartnerServiceTest
    {
        private readonly Mock<IBucketService> _bucketServiceMock;

        public PartnerServiceTest()
        {
            _bucketServiceMock = new();
            _bucketServiceMock.Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(string.Empty);
        }

        [Fact]
        public async Task GetSingleOrDefaultByIdAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data["Basic"]);

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var user = await service.GetSingleOrDefaultByIdAsync("t344r");

            Assert.NotNull(user);
            Assert.Equivalent(PartnerData.Data["Basic"], user);
        }

        [Fact]
        public async Task GetSingleOrDefaultByIdAsync_BusinessExceptions()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.GetSingleOrDefaultByIdAsync(""));

            Assert.Contains("O Id é obrigatório.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task GetSingleOrDefaultByEmailAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data["Basic"]);

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var user = await service.GetSingleOrDefaultByEmailAsync("teste@teste.com");

            Assert.NotNull(user);
            Assert.Equivalent(PartnerData.Data["Basic"], user);
        }

        [Fact]
        public async Task GetSingleOrDefaultByEmailAsync_BusinessExceptions()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.GetSingleOrDefaultByEmailAsync(""));

            Assert.Contains("O email é obrigatório.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.FetchAsync(null)).ReturnsAsync(new List<Partner>
            {
                PartnerData.Data["Basic"]
            });

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var users = await service.GetAllAsync();

            Assert.NotNull(users);
            Assert.Equivalent(PartnerData.Data["Basic"], users.FirstOrDefault());
        }

        [Fact]
        public async Task GetByFilterAsync()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.FetchAsync(null)).ReturnsAsync(new List<Partner>
            {
                PartnerData.Data["Basic"]
            });

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            PartnerFilter filter = new PartnerFilter()
            {
                Name = "Basic",
                Type = OrganizationType.Todos
            };

            var users = await service.GetByFilterAsync(filter);

            Assert.NotNull(users);
            Assert.Equivalent(PartnerData.Data["Basic"], users.FirstOrDefault());
        }

        [Fact]
        public async Task RegisterAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data["Null"]);
            unitOfWorkMock.Setup(x => x.Partners.AddAsync(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var user = await service.RegisterAsync(PartnerData.Data["Basic"]);

            Assert.NotNull(user);
            Assert.Equivalent(PartnerData.Data["Basic"], user);
        }

        [Fact]
        public async Task RegisterAsync_InvalidEntity()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                 service.RegisterAsync(PartnerData.Data["Empty"]));

            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Theory]
        [InlineData("Null", "Null", "O parâmetro Parceiro não pode ser nulo.")]
        [InlineData("Basic", "Basic", "Parceiro já existe.")]
        public async Task RegisterAsync_BusinessExceptions(
            string userDataKey,
            string userDataRequest,
            string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data[userDataKey]);

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.RegisterAsync(PartnerData.Data[userDataRequest]));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Partners.Update(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var result = await service.UpdateAsync(PartnerData.Data["Basic"], "teste");

            unitOfWorkMock.Verify(u => u.Partners.Update(It.IsAny<Partner>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equivalent(PartnerData.Data["Basic"], result);
        }

        [Theory]
        [InlineData("Null", "Null", "O parâmetro Parceiro não pode ser nulo.")]
        [InlineData("WithoutId", "WithoutId", "O Id é obrigatório.")]
        [InlineData("Null", "UpdateException", "Parceiro não encontrado na nossa base de dados.")]
        public async Task UpdateAsync_BusinessException(
            string userDataKey,
            string userDataRequest,
            string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data[userDataKey]);
            unitOfWorkMock.Setup(x => x.Partners.Update(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                service.UpdateAsync(PartnerData.Data[userDataRequest], "teste"));


            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public void UpdatePassword_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var partner = PartnerData.Data["Basic"];
            var newPassword = "dTy7m726FLaYCQWMdzOYTg==";
            var updatedPartner = new Partner { Id = partner.Id, Password = newPassword };

            unitOfWorkMock.Setup(x => x.Partners.Update(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var result = service.UpdatePassword(partner);

            unitOfWorkMock.Verify(u => u.Partners.Update(It.IsAny<Partner>()), Times.Once);
            Assert.Equal(newPassword, result.Password);
        }

        [Theory]
        [InlineData("WithoutId", "O Id é obrigatório.")]
        [InlineData("Null", "O parâmetro Parceiro não pode ser nulo.")]
        public void UpdatePassword_BusinessExceptions(
            string userDataKey,
            string message)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.Partners.Update(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = Assert.Throws<BusinessException>(() =>
              service.UpdatePassword(PartnerData.Data[userDataKey]));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>()))
                          .ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Partners.Delete(It.IsAny<Partner>())).Verifiable();

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            await service.DeleteAsync("t35t3");

            unitOfWorkMock.Verify(x => x.Partners.Delete(It.Is<Partner>(p => p == PartnerData.Data["Basic"])), Times.Once);
        }

        [Theory]
        [InlineData("Null", "t35t3", "Não foi possível encontrar nenhum Parceiro com essas informações.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("WithoutId", "", "O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        [InlineData("Null", "UpdateException", "Não foi possível encontrar nenhum Parceiro com essas informações.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity)]
        public async Task DeleteAsync_BusinessExceptions(
            string userDataKey,
            string userId,
            string expectedMessage,
            StatusProcess expectedStatus,
            HttpStatusCode expectedStatusCode)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>()))
                          .ReturnsAsync(PartnerData.Data[userDataKey]);

            var service = new PartnerService(unitOfWorkMock.Object, _bucketServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() => service.DeleteAsync(userId));

            Assert.Contains(expectedMessage, exception.ErrorDetails);
            Assert.Equal(expectedStatus, exception.Status);
            Assert.Equal(expectedStatusCode, exception.StatusCode);
        }
    }
}
