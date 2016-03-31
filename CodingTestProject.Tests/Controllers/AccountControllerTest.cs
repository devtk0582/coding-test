using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodingTestProject;
using CodingTestProject.Controllers;
using CodingTestProject.Models;

namespace CodingTestProject.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private IAccountService _service;
        private ISessionManager _sessionManager;
        private int _userId;

        [TestInitialize]
        public void Initialize()
        {
            _userId = 1;
            _service = new AccountService(new ModelStateWrapper(new ModelStateDictionary()), new ListAccountRepository());
            _sessionManager = new DictionarySessionManager();
            _sessionManager.Add("UserID", _userId.ToString());
        }

        [TestMethod]
        public void TestIndex()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(IEnumerable<Account>));
        }

        [TestMethod]
        public void TestCreate()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateWithoutSession()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            };
            _sessionManager.Remove("UserID");

            // Act
            RedirectToRouteResult result = controller.Create(accountToCreate) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCreateValidAccount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            };
            // Act
            RedirectToRouteResult result = controller.Create(accountToCreate) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCreateInvalidAccount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 60
            };
            // Act
            ViewResult result = controller.Create(accountToCreate) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void TestDelete()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });

            // Act
            ViewResult result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));
        }

        [TestMethod]
        public void TestDeleteValid()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToDelete = new AccountViewModel()
            {
                Account = new Account()
                {
                    ID = 1,
                    Name = "Test Account1",
                    UserID = _userId,
                    Balance = 120
                }
            };
            // Act
            RedirectToRouteResult result = controller.Delete(accountToDelete.Account) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestDeposit()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });

            // Act
            ViewResult result = controller.Deposit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
            Assert.IsNotNull(((AccountViewModel)result.ViewData.Model).Account);
        }

        [TestMethod]
        public void TestDepositValid()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToDeposit = new AccountViewModel()
            {
                Account = _service.GetAccount(1),
                Amount = 500
            };
            // Act
            RedirectToRouteResult result = controller.Deposit(accountToDeposit) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestDepositNullAccount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToDeposit = new AccountViewModel()
            {
                Account = null,
                Amount = 500
            };
            // Act
            ViewResult result = controller.Deposit(accountToDeposit) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
        }

        [TestMethod]
        public void TestDepositInvalidAmount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToDeposit = new AccountViewModel()
            {
                Account = _service.GetAccount(1),
                Amount = 10001
            };
            // Act
            ViewResult result = controller.Deposit(accountToDeposit) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
            Assert.IsNotNull(((AccountViewModel)result.ViewData.Model).Account);
        }

        [TestMethod]
        public void TestWithdraw()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });

            // Act
            ViewResult result = controller.Withdraw(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
            Assert.IsNotNull(((AccountViewModel)result.ViewData.Model).Account);
        }

        [TestMethod]
        public void TestWithdrawValid()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToWithdraw = new AccountViewModel()
            {
                Account = _service.GetAccount(1),
                Amount = 10
            };
            // Act
            RedirectToRouteResult result = controller.Deposit(accountToWithdraw) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestWithdrawNullAccount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 500
            });
            var accountToWithdraw = new AccountViewModel()
            {
                Account = null,
                Amount = 100
            };
            // Act
            ViewResult result = controller.Withdraw(accountToWithdraw) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
        }

        [TestMethod]
        public void TestWithdrawInvalidAmount()
        {
            // Arrange
            AccountController controller = new AccountController(_service, _sessionManager);
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            });
            var accountToWithdraw = new AccountViewModel()
            {
                Account = _service.GetAccount(1),
                Amount = 30
            };
            // Act
            ViewResult result = controller.Withdraw(accountToWithdraw) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AccountViewModel));
            Assert.IsNotNull((AccountViewModel)result.ViewData.Model);
            Assert.IsNotNull(((AccountViewModel)result.ViewData.Model).Account);
        }
    }
}
