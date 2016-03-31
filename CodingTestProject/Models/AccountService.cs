using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public class AccountService : IAccountService
    {
        private IValidation _validation;
        private IAccountRepository _repository;


        public AccountService(IValidation validation) 
            : this(validation, new EntityAccountRepository())
        { }


        public AccountService(IValidation validation, IAccountRepository repository)
        {
            _validation = validation;
            _repository = repository;
        }


        public bool ValidateAccount(ValidateType type, Account accountToValidate, decimal amount)
        {
            switch (type)
            {
                case ValidateType.Create:
                    if (accountToValidate.Name == null || accountToValidate.Name.Trim().Length == 0)
                        _validation.AddError("NameRequired", "Name is required.");
                    if (accountToValidate.Balance < 100)
                        _validation.AddError("BalanceRequired", "Balance must not be less than 100.");
                    break;
                case ValidateType.Deposit:
                    if (amount <= 0)
                        _validation.AddError("DepositMin", "The amount to deposit must be greater than 0");
                    if (amount > 10000)
                        _validation.AddError("DepositMax", "The amount can not exceed 10000 for a single transaction.");
                    break;
                case ValidateType.Withdraw:
                    if (amount <= 0)
                        _validation.AddError("WithdrawMin", "The amount to withdraw must be greater than 0");
                    if ((accountToValidate.Balance - amount) < 100)
                        _validation.AddError("BalanceMin", "Balance must not be less than 100 after withdrawal.");
                    if (amount > (accountToValidate.Balance * 0.9m))
                        _validation.AddError("WithdrawMax", "You cannot withdraw more than 90% of total balance in a single transaction");
                    break;
            }


            return _validation.IsValid;
        }


        #region IAccountService Members

        public bool CreateAccount(Account accountToCreate)
        {
            // Validation logic
            if (!ValidateAccount(ValidateType.Create, accountToCreate, 0))
                return false;

            // Database logic
            try
            {
                _repository.CreateAccount(accountToCreate);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DepositAccount(Account accountToDeposit, decimal amount)
        {
            // Validation logic
            if (!ValidateAccount(ValidateType.Deposit, accountToDeposit, amount))
                return false;

            // Database logic
            try
            {
                _repository.UpdateAccountBalance(accountToDeposit.ID, accountToDeposit.Balance + amount);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WithdrawAccount(Account accountToDeposit, decimal amount)
        {
            // Validation logic
            if (!ValidateAccount(ValidateType.Withdraw, accountToDeposit, amount))
                return false;

            // Database logic
            try
            {
                _repository.UpdateAccountBalance(accountToDeposit.ID, accountToDeposit.Balance - amount);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteAccount(Account accountToDelete)
        {
            try
            {
                _repository.DeleteAccount(accountToDelete);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Account GetAccount(int id)
        {
            return _repository.GetAccount(id);
        }

        public IEnumerable<Account> GetAccounts(int userId)
        {
            return _repository.GetAccounts(userId);
        }

        #endregion
    }

    public enum ValidateType { Create, Deposit, Withdraw }
}