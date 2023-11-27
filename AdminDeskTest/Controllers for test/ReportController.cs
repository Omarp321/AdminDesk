using System;
using System.Threading.Tasks;
using AdminDesk.Controllers;
using AdminDesk.Models.Report;
using AdminDesk.Entities;
using AdminDesk.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AdminDeskTest
{
    public class ReportControllerTests
    {
        // Test for Index action
        [Fact]
        public void Index_ReturnsCorrectViewModel()
        {
            // Arrange
            var reportRepositoryMock = new Mock<IReportRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();
            var controller = new ReportController(reportRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = controller.Index(1) as ViewResult;
            var model = result?.Model as ReportFullViewModel;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Index", result.ViewName); // Checks if the correct view is returned
            Xunit.Assert.NotNull(model);
            Xunit.Assert.Equal(1, model.UpsertModel.ServiceOrderId); // Checks if the model has the correct ServiceOrderId
        }

        // Test for PostReport action when ModelState is valid
        [Fact]
        public async Task PostReport_RedirectsToServiceOrderSpesificOnSuccess()
        {
            // Arrange
            var reportRepositoryMock = new Mock<IReportRepository>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>();// Assuming GetUserManagerMock provides a valid mock for UserManager
            var controller = new ReportController(reportRepositoryMock.Object, userManagerMock.Object);

            var viewModel = new ReportFullViewModel
            {
                UpsertModel = new ReportViewModel
                {
                    ServiceOrderId = 123,
                    Mechanic = "John",
                    ServiceType = "Repair",
                    ReportWriteDate = DateTime.Now,
                }
            };

            // Act
            var result = await controller.PostReport(viewModel) as RedirectToActionResult;

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Spesific", result.ActionName); // Checks if the action redirects to "Spesific"
            reportRepositoryMock.Verify(repo => repo.Upsert(It.IsAny<AdminDesk.Entities.Report>()), Times.Once);
            Xunit.Assert.Equal(viewModel.UpsertModel.ServiceOrderId, result.RouteValues["id"]); // Checks if the correct id is passed in the route values
        }

    }
}
