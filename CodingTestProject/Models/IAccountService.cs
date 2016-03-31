using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public interface IAccountService
    {
        bool CreateAccount(Account accountToCreate);
        bool DeleteAccount(Account accountToDelete);
        bool DepositAccount(Account accountToDeposit, decimal amount);
        bool WithdrawAccount(Account accountToWithdraw, decimal amount);
        Account GetAccount(int id);
        IEnumerable<Account> GetAccounts(int userId);
    }
}