using BankingApp.Models.BankModels;
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
    [Authorize(Roles = "Admin")]
    public class BankController : ApiController
    {
        private readonly BankService _service = new BankService();

        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var banks = _service.GetBanks();
            return Ok(banks);
        }

        public IHttpActionResult Get(int id)
        {
            var bank = _service.GetBankById(id);
            return Ok(bank);
        }

        public IHttpActionResult Post(BankCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_service.CreateBank(model))
                return InternalServerError();
            return Ok($"'{model.Name}' has been successfully added.");
        }

        public IHttpActionResult Put(BankEdit model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_service.UpdateBank(model))
                return InternalServerError();
            return Ok($"'{model.Name}' has been successfully updated.");
        }

        public IHttpActionResult Delete(int id)
        {
            if (!_service.DeleteBank(id))
                return InternalServerError();
            return Ok("Bank has been successfully deleted.");
        }
    }
}
