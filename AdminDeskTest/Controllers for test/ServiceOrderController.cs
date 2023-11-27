using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminDesk.Controllers;
using AdminDesk.Entities;
using AdminDesk.Models.Report;
using AdminDesk.Models.ServiceOrder;
using AdminDesk.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AdminDeskTest
{
    public class ServiceOrderControllerTests
    {
        // Test for Index action
        [Fact]
        public void Index_ReturnsCorrectViewModel()
        {
            // Arrange
            var serviceOrderRepositoryMock = new Mock<IServiceOrderRepository>();
            var reportRepositoryMock = new Mock<IReportRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();
            var controller = new ServiceOrderController(
                serviceOrderRepositoryMock.Object,
                reportRepositoryMock.Object,
                customerRepositoryMock.Object,
                userManagerMock.Object
            );

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as ServiceOrderFullViewModel;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Index", result.ViewName);
            Xunit.Assert.NotNull(model);
        }

        // Test for NyServiceOrdre action
        [Fact]
        public void NyServiceOrdre_ReturnsCorrectViewModel()
        {
            // Arrange
            var serviceOrderRepositoryMock = new Mock<IServiceOrderRepository>();
            var reportRepositoryMock = new Mock<IReportRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();
            var controller = new ServiceOrderController(
                serviceOrderRepositoryMock.Object,
                reportRepositoryMock.Object,
                customerRepositoryMock.Object,
                userManagerMock.Object
            );

            // Act
            var result = controller.NyServiceOrdre() as ViewResult;
            var model = result?.Model as ServiceOrderFullViewModel;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("NyServiceOrdre", result.ViewName);
            Xunit.Assert.NotNull(model);
        }

        // Test for Spesific action
        [Fact]
        public void Spesific_ReturnsCorrectViewModel()
        {
            // Arrange
            var serviceOrderRepositoryMock = new Mock<IServiceOrderRepository>();
            var reportRepositoryMock = new Mock<IReportRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();
            var controller = new ServiceOrderController(
                serviceOrderRepositoryMock.Object,
                reportRepositoryMock.Object,
                customerRepositoryMock.Object,
                userManagerMock.Object
            );

            // Anta at det er en tjenesteordre med ID 1 i depotet
            serviceOrderRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>()))
                .Returns(new ServiceOrder { ServiceOrderId = 1, /* other properties */ });

            // Anta at det er rapporter knyttet til tjenesteordren
            reportRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(new List<Report> // Konverter den til liste
                {
            new Report { ReportId = 1, },
            new Report { ReportId = 2, }
                });

            // Act
            var result = controller.Spesific(1) as ViewResult;
            var model = result?.Model as CompositeViewModel;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Spesific", result.ViewName);
            Xunit.Assert.NotNull(model);
        }

        // Test for Post action
        [Fact]
        public async Task Post_RedirectsToIndexOnSuccess()
        {
            // Arrange
            var serviceOrderRepositoryMock = new Mock<IServiceOrderRepository>();
            var reportRepositoryMock = new Mock<IReportRepository>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();
            var controller = new ServiceOrderController(
                serviceOrderRepositoryMock.Object,
                reportRepositoryMock.Object,
                customerRepositoryMock.Object,
                userManagerMock.Object
            );

            // Anta at ModelState er gyldig
            controller.ModelState.Clear();

            // Act
            var result = await controller.Post(new ServiceOrderFullViewModel()) as IActionResult;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.IsType<RedirectToActionResult>(result);
            Xunit.Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
        }

    }
}
