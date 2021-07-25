using BankingApp.Data;
using BankingApp.Models.BankAccountModels;
using BankingApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebAPI.Controllers
{
    [Authorize]
    public class BankAccountController : ApiController
    {
        public IHttpActionResult Get()
        {
            var service = CreateBankAccountService();
            var accounts = service.GetUserBankAccounts();
            return Ok(accounts);
        }

        public IHttpActionResult Get(int id)
        {
            var service = CreateBankAccountService();
            if (User.IsInRole("Admin"))
            {
                var account = service.GetBankAccountByIdAdmin(id);
                return Ok(account);
            }
            else
            {
                var account = service.GetBankAccountById(id);
                return Ok(account);
            }
        }

        public IHttpActionResult Post(BankAccountCreate model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!service.CreateBankAccount(model))
                return InternalServerError();
            return Ok("New account successfully created.");
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult Put(BankAccountEdit model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!service.UpdateBankAccount(model))
                return InternalServerError();
            return Ok($"Account {model.BankAccountId.ToString("D9")} has been successfully updated.");
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            var service = CreateBankAccountService();
            if (!service.DeleteBankAccount(id))
                return InternalServerError();
            return Ok("The account has been successfully deleted.");
        }

        private BankAccountService CreateBankAccountService()
        {
            var ownerId = Guid.Parse(User.Identity.GetUserId());
            return new BankAccountService(ownerId, new ApplicationDbContext());
        }
    }
}