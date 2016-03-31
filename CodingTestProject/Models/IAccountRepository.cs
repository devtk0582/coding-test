using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public interface IAccountRepository
    {
        Account CreateAccount(Account accountToCreate);
        void DeleteAccount(Account accountToDelete);
        Account UpdateAccountBalance(int id, decimal balance);
        Account GetAccount(int id);
        IEnumerable<Account> GetAccounts(int userId);
    }
}