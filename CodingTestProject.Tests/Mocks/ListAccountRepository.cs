using CodingTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTestProject.Tests
{
    public class ListAccountRepository : IAccountRepository
    {
        private List<Account> _accounts = new List<Account>();

        public Account CreateAccount(Account accountToCreate)
        {
            accountToCreate.CreateDate = DateTime.Now;
            accountToCreate.ID = _accounts.Count + 1;
            _accounts.Add(accountToCreate);
            return accountToCreate;
        }

        public void DeleteAccount(Account accountToDelete)
        {
            var originalAccount = GetAccount(accountToDelete.ID);
            _accounts.Remove(originalAccount);
        }

        public Account GetAccount(int id)
        {
            return (from account in _accounts
                    where account.ID == id
                    select account).FirstOrDefault();
        }

        public IEnumerable<Account> GetAccounts(int userId)
        {
            return _accounts.Where(account => account.UserID == userId).ToList();
        }

        public Account UpdateAccountBalance(int id, decimal amount)
        {
            var originalAccount = GetAccount(id);
            originalAccount.Balance = amount;
            return originalAccount;
        }
    }
}
