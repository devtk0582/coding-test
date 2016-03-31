using CodingTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodingTestProject.Controllers
{
    public class AccountController : Controller
    {
        IAccountService _service;
        ISessionManager _sessionManager;

        public AccountController()
        {
            _service = new AccountService(new ModelStateWrapper(this.ModelState));
            _sessionManager = new SessionManager();
        }

        public AccountController(IAccountService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        public ActionResult Index()
        {
            if(!_sessionManager.HasKey("UserID"))
                _sessionManager.Add("UserID", "1");
            return View(_service.GetAccounts(Convert.ToInt32(_sessionManager.Get("UserID"))));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Account accountToAdd)
        {
            try
            {
                if (!_sessionManager.HasKey("UserID"))
                    return RedirectToAction("Index");
                accountToAdd.UserID = Convert.ToInt32(_sessionManager.Get("UserID"));
                if (_service.CreateAccount(accountToAdd))
                    return RedirectToAction("Index");
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Deposit(int id)
        {
            return View(new AccountViewModel()
            {
                Account = _service.GetAccount(id),
                Amount = 0
            });
        }

        [HttpPost]
        public ActionResult Deposit(AccountViewModel accountToEdit)
        {
            try
            {
                if (_service.DepositAccount(accountToEdit.Account, accountToEdit.Amount))
                    return RedirectToAction("Index");
                else
                    return View(accountToEdit);
            }
            catch
            {
                return View(accountToEdit);
            }
        }

        public ActionResult Withdraw(int id)
        {
            return View(new AccountViewModel()
            {
                Account = _service.GetAccount(id),
                Amount = 0
            });
        }

        [HttpPost]
        public ActionResult Withdraw(AccountViewModel accountToWithdraw)
        {
            try
            {
                if (_service.WithdrawAccount(accountToWithdraw.Account, accountToWithdraw.Amount))
                    return RedirectToAction("Index");
                else
                    return View(accountToWithdraw);
            }
            catch
            {
                return View(accountToWithdraw);
            }
        }

        public ActionResult Delete(int id)
        {
            return View(_service.GetAccount(id));
        }

        [HttpPost]
        public ActionResult Delete(Account accountToDelete)
        {
            try
            {
                if (_service.DeleteAccount(accountToDelete))
                    return RedirectToAction("Index");
                else
                    return View(accountToDelete);
            }
            catch
            {
                return View(accountToDelete);
            }
        }
    }
}