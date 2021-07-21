using BankingApp.Models.ActionModels;
using BankingApp.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebAPI.Controllers
{
    [Authorize]
    public class TransactionController : ApiController
    {
        public IHttpActionResult Post(Deposit model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!service.MakeDeposit(model))
                return InternalServerError();
            return Ok($"{model.Amount} successfully deposited.");
        }

        public IHttpActionResult Put(Transfer model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!service.MakeTransfer(model))
                return InternalServerError();
            return Ok($"{model.Amount} successfully transferred.");
        }

        public IHttpActionResult Delete(Withdrawal model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!service.MakeWithdrawal(model))
                return InternalServerError();
            return Ok($"{model.Amount} successfully withdrawn.");
        }

        public BankAccountService CreateBankAccountService()
        {
            var ownerId = Guid.Parse(User.Identity.GetUserId());
            return new BankAccountService(ownerId);
        }
    }
}
