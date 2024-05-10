
﻿using Application.Business.UnitOfWork;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Helper;

namespace AQAcademy_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPaymentController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerPaymentController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }

        [Route("GetPaymentMethods")]
        [HttpGet]
        public List<PaymentMethods> GetPaymentMethods()
        {
            var PayMethods = _UnitOfWork.CustomerPayment.GetPaymentMethods();
            return PayMethods;
        }

        [Route("MakePayment")]
        [HttpPost]
        public async Task<IActionResult> MakePayment(CustomerPayment customerPayment)
        {
            decimal RemainingBalance = _UnitOfWork.CustomerReceipt.GetRemainingBalance(customerPayment.CustomerReceipt_id);
            if (customerPayment.Amount > RemainingBalance)
            {
                throw new Exception("Error: Cannot Receive Amount more than Remaining Balance (" + Convert.ToString(RemainingBalance) + ")");
            }
            if (customerPayment.id == 0)
            {
                customerPayment.CreatedAt = Helpers.GetCurrentDateTime();
                _UnitOfWork.CustomerPayment.Add(customerPayment);
                await _UnitOfWork.SaveDbChanges();
            }
            else
            {
                customerPayment.UpdatedAt = Helpers.GetCurrentDateTime();
                _UnitOfWork.CustomerPayment.Update(customerPayment);
                await _UnitOfWork.SaveDbChanges();
            }
            return Ok(customerPayment.CustomerReceipt_id);
        }
    }
}
