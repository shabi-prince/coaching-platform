using Application.Business.UnitOfWork;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Helper;

namespace AQAcademy_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentVoucherController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public PaymentVoucherController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }
        [Route("GetVoucherHistory")]
        [HttpGet]
        public List<PaymentVoucherDTO> GetVoucherHistory()
        {
            return _UnitOfWork.PaymentVoucher.GetVouchersList();
        }
        [Route("GetPaymentVoucherDetail/{id}")]
        [HttpGet]
        public PaymentVoucherDTO GetPaymentVoucherDetail(int id)
        {
            return _UnitOfWork.PaymentVoucher.GetVoucherDetail(id);
        }
        [Route("GetVoucherById/{id}")]
        [HttpGet]
        public IActionResult GetVoucherById(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var SaleOrderDetail = _UnitOfWork.PaymentVoucher.GetVoucherById(id);
            if (SaleOrderDetail == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(SaleOrderDetail);
            }
        }
        [Route("AddOrEditVoucher")]
        [HttpPost]
        public async Task<IActionResult> AddOrEditVoucher(PaymentVoucher Voucher)
        {
            try
            {
                if (Voucher.id == 0)
                {
                    Voucher.CreatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.PaymentVoucher.Add(Voucher);
                    await _UnitOfWork.SaveDbChanges();
                }
                else
                {
                    Voucher.UpdatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.PaymentVoucher.UpdateChildWithParent(Voucher, "PaymentVoucherDetails");
                    await _UnitOfWork.SaveDbChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("DeleteVoucher")]
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher(int? id, [FromQuery] string UserId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Vocuher = await _UnitOfWork.PaymentVoucher.DeleteVoucher(id);
            Vocuher.IsActive = false;
            Vocuher.DeletedAt = Helpers.GetCurrentDateTime();
            Vocuher.DeletedBy = UserId;
            _UnitOfWork.PaymentVoucher.Update(Vocuher);
            await _UnitOfWork.SaveDbChanges();
            return NoContent();
        }
        [Route("GetVoucherCode")]
        [HttpGet]
        public string GetOrderNo()
        {
            return _UnitOfWork.PaymentVoucher.VoucherCode();
        }
    }
}
