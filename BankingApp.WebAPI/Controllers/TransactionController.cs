using BankingApp.Data;
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
            return Ok($"${model.Amount} successfully deposited.");
        }

        public IHttpActionResult Put(Transfer model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            switch (service.MakeTransfer(model))
            {
                case 1:
                    return Ok($"${model.Amount} successfully transferred.");
                case 2:
                    return InternalServerError();
                case 3:
                    return BadRequest("Transfer failed due to insufficient funds.");
                default:
                    return InternalServerError();
            }
        }

        public IHttpActionResult Delete(Withdrawal model)
        {
            var service = CreateBankAccountService();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            switch (service.MakeWithdrawal(model))
            {
                case 1:
                    return Ok($"${model.Amount} successfully withdrawn.");
                case 2:
                    return InternalServerError();
                case 3:
                    return BadRequest("Cannot withdraw more than $1,000 from an Individual Investment Account.");
                case 4:
                    return BadRequest("Withdrawal failed due to insufficient funds.");
                default:
                    return InternalServerError();
            }
        }

        private BankAccountService CreateBankAccountService()
        {
            var ownerId = Guid.Parse(User.Identity.GetUserId());
            return new BankAccountService(ownerId, new ApplicationDbContext());
        }
    }
}
