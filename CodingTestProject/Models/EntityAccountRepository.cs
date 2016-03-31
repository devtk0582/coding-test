using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public class EntityAccountRepository : IAccountRepository
    {
        private CodingTestDBEntities _entities = new CodingTestDBEntities();

        public Account CreateAccount(Account accountToCreate)
        {
            accountToCreate.CreateDate = DateTime.Now;
            _entities.Accounts.Add(accountToCreate);
            _entities.SaveChanges();
            return accountToCreate;
        }

        public void DeleteAccount(Account accountToDelete)
        {
            var originalAccount = GetAccount(accountToDelete.ID);
            _entities.Accounts.Remove(originalAccount);
            _entities.SaveChanges();
        }

        public Account UpdateAccountBalance(int id, decimal amount)
        {
            var originalAccount = GetAccount(id);
            originalAccount.Balance = amount;
            _entities.SaveChanges();
            return originalAccount;
        }

        public Account GetAccount(int id)
        {
            return (from account in _entities.Accounts
                    where account.ID == id
                    select account).FirstOrDefault();
        }

        public IEnumerable<Account> GetAccounts(int userId)
        {
            return _entities.Accounts.Where(account => account.UserID == userId).ToList();
        }
    }
}