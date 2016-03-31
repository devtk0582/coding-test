using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using CodingTestProject.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodingTestProject.Tests.Models
{
    [TestClass]
    public class AccountServiceTest
    {
        private IAccountService _service;
        private ModelStateDictionary _modelState;
        private IAccountRepository _repository;
        private int _userId;

        [TestInitialize]
        public void Initialize()
        {
            _userId = 1;
            _modelState = new ModelStateDictionary();
            _repository = new ListAccountRepository();
            _service = new AccountService(new ModelStateWrapper(_modelState), _repository);

        }

        [TestMethod]
        public void TestCreateAccount()
        {
            //Arrange
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            };

            //Act
            var result = _service.CreateAccount(accountToCreate);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCreateAccountRequireName()
        {
            //Arrange
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "",
                UserID = _userId,
                Balance = 120
            };

            //Act
            var result = _service.CreateAccount(accountToCreate);

            //Assert
            Assert.IsFalse(result);
            var error = _modelState["NameRequired"].Errors[0];
            Assert.AreEqual(error.ErrorMessage, "Name is required.");
        }

        [TestMethod]
        public void TestCreateAccountBalanceMin()
        {
            //Arrange
            var accountToCreate = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 20
            };

            //Act
            var result = _service.CreateAccount(accountToCreate);

            //Assert
            Assert.IsFalse(result);
            var error = _modelState["BalanceRequired"].Errors[0];
            Assert.AreEqual(error.ErrorMessage, "Balance must not be less than 100.");
        }

        [TestMethod]
        public void TestCreateMultipleAccounts()
        {
            //Arrange
            var accountToCreate1 = new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 120
            };
            var accountToCreate2 = new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 200
            };
            var accountToCreate3 = new Account()
            {
                ID = -1,
                Name = "Test Account3",
                UserID = _userId,
                Balance = 300
            };

            //Act
            var result1 = _service.CreateAccount(accountToCreate1);
            var result2 = _service.CreateAccount(accountToCreate2);
            var result3 = _service.CreateAccount(accountToCreate3);

            //Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
        }

        [TestMethod]
        public void TestGetAccountsCount()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.GetAccounts(_userId);

            //Assert      
            Assert.IsTrue(result.Select(a => a.ID).Distinct().Count() == 2);
        }

        [TestMethod]
        public void TestGetAccountsIndividualValid()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.GetAccounts(_userId);

            //Assert      
            foreach (var account in result)
            {
                Assert.IsTrue(account != null && account.Name != null && account.Name.Trim() != string.Empty && account.Balance >= 100);
            }
        }

        [TestMethod]
        public void TestGetAccount()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.GetAccount(1);

            //Assert      
            Assert.IsTrue(result != null && result.UserID == _userId && result.Name == "Test Account1" && result.Balance == 150);
        }

        [TestMethod]
        public void TestGetAccountNotFound()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.GetAccount(3);

            //Assert      
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void TestDeleteAccount()
        {
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.DeleteAccount(new Account()
            {
                ID = 1
            });

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteAccountNotFound()
        {
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account2",
                UserID = _userId,
                Balance = 300
            });

            //Act
            var result = _service.DeleteAccount(new Account()
            {
                ID = 3
            });

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDepositAccount()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            var account = _service.GetAccount(1);

            //Act
            var result = _service.DepositAccount(account, 3000);

            //Assert
            Assert.IsTrue(result);
            account = _service.GetAccount(1);
            Assert.IsTrue(account.Balance == 3150);
        }

        [TestMethod]
        public void TestDepositAccountDepositMax()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            var account = _service.GetAccount(1);

            //Act
            var result = _service.DepositAccount(account, 10500);

            //Assert
            Assert.IsFalse(result);
            account = _service.GetAccount(1);
            Assert.IsTrue(account.Balance == 150);
        }

        [TestMethod]
        public void TestWithdrawAccount()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 500
            });
            var account = _service.GetAccount(1);

            //Act
            var result = _service.WithdrawAccount(account, 100);

            //Assert
            Assert.IsTrue(result);
            account = _service.GetAccount(1);
            Assert.IsTrue(account.Balance == 400);
        }

        [TestMethod]
        public void TestWithdrawAccountWithdrawMax()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 500
            });
            var account = _service.GetAccount(1);

            //Act
            var result = _service.WithdrawAccount(account, 460);

            //Assert
            Assert.IsFalse(result);
            account = _service.GetAccount(1);
            Assert.IsTrue(account.Balance == 500);
        }

        [TestMethod]
        public void TestWithdrawAccountAfterWithdrawalLimit()
        {
            //Arrange
            _service.CreateAccount(new Account()
            {
                ID = -1,
                Name = "Test Account1",
                UserID = _userId,
                Balance = 150
            });
            var account = _service.GetAccount(1);

            //Act
            var result = _service.WithdrawAccount(account, 60);

            //Assert
            Assert.IsFalse(result);
            account = _service.GetAccount(1);
            Assert.IsTrue(account.Balance == 150);
        }
    }
}
