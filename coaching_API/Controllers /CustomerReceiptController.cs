using Application.Business.UnitOfWork;
using Domain.DTO;
using Domain.Entities;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReceiptController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerReceiptController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }

        [Route("GetAllReceipts")]
        [HttpGet]
        public List<CustomerReceiptDTO> GetAllReceipts()
        {
            var Receipts = _UnitOfWork.CustomerReceipt.GetAllReceipts();
            return Receipts;
        }

        [Route("GetReceiptDetail/{id}")]
        [HttpGet]
        public IActionResult GetReceiptDetail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ReceiptDetail = _UnitOfWork.CustomerReceipt.GetReceiptDetail(id);
            if (ReceiptDetail == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ReceiptDetail);
            }
        }

        [Route("CreateSaleOrderandReceipt")]
        [HttpPost]
        public async Task<IActionResult> CreateSaleOrderAsync(ItemSaleOders iso)
        {
            try
            {
                //Validating Item Stock
                foreach (var item in iso.ItemSaleGoods)
                {
                    Item ItemObject = await _UnitOfWork.Item.GetItemInfo(item.ItemId);
                    if (item.quantity > ItemObject.Stock) throw new Exception("Error: Cannot sale " + ItemObject.ItemName + " more than available stock(" + ItemObject.Stock.ToString() + ")");
                }
                CustomerReceipt cr = new();
                if (iso.id == 0)
                {
                    iso.CreatedAt = Helpers.GetCurrentDateTime();
                    iso.IsReceipt = true;
                    _UnitOfWork.ItemSaleOrder.Add(iso);
                    await _UnitOfWork.SaveDbChanges();
                }
                else
                {
                    iso.UpdatedAt = Helpers.GetCurrentDateTime();
                    iso.IsReceipt = true;
                    _UnitOfWork.ItemSaleOrder.UpdateChildWithParent(iso, "ItemSaleGoods");
                    await _UnitOfWork.SaveDbChanges();
                }
                cr.CreatedAt = Helpers.GetCurrentDateTime();
                cr.CreatedBy = iso.CreatedBy;
                cr.BillDate = Helpers.GetCurrentDateTime();
                cr.Amount = iso.TotalAmount;
                cr.RemainingBalance = iso.TotalAmount;
                cr.ItemSaleOrder_ID = iso.id;
                _UnitOfWork.CustomerReceipt.Add(cr);
                await _UnitOfWork.SaveDbChanges();

                return Ok(cr.id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
