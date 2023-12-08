using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Controllers.Account;
using WebApp.Models.Entities.Users;
using WebApp.Models.ViewModels.Account;

namespace UnitTests.Controllers.AccountTests
{
    public class RegisterTests
    {
        [Fact]
        public async Task RegisterMustRedirectToIndex()
        {
            //Arrange
            var modelTest = new RegisterViewModel();
            var userTest = new User();

            var mapperMock = new Mock<IMapper>();
            var userManagerMock = new Mock<UserManager<User>>();
            var identityResultMock = new Mock<IdentityResult>();
            var signInManagerMock = new Mock<SignInManager<User>>();

            mapperMock.Setup(mapper => mapper.Map<User>(modelTest)).Returns(userTest);
            identityResultMock.Setup(result => result.Succeeded).Returns(true);
            userManagerMock.Setup(userManager => userManager.CreateAsync(userTest, modelTest.PasswordReg))
                .ReturnsAsync(identityResultMock.Object);
            signInManagerMock.Setup(signInM => signInM.SignInAsync(userTest, false, null))
                .Returns(Task.CompletedTask);

            var controller = new RegisterController(
                mapperMock.Object, userManagerMock.Object, signInManagerMock.Object);

            //Act
            var result = await controller.Register(modelTest);

            //Assert    
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void SignInErrorMustThrowException()
        {
            //Arrange

            //Act

            //Assert
        }

        [Fact]
        public void IfModelIsNotValidMustRedirect()
        {
            //Arrange

            //Act
            
            //Assert
        }


    }
}
