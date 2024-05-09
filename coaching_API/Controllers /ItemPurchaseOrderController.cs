using Application.Business.UnitOfWork;
using Domain.DTO;
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
    public class ItemPurchaseOrderController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ItemPurchaseOrderController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }
        [Route("GetPurchaseOrderHistory")]
        [HttpGet]
        public List<ItemPurchaseOrderDTO> GetAllItems()
        {
            var ItemsList = _UnitOfWork.ItemPurchaseOrder.GetPurchaseOrderHistory();
            return ItemsList;
        }

        [Route("AddOrEditOrder")]
        [HttpPost]
        public async Task<IActionResult> PostOrder(ItemPurchaseOrder itemPurchaseOrder)
        {
            try
            {
                if (itemPurchaseOrder.id == 0)
                {
                    itemPurchaseOrder.CreatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.ItemPurchaseOrder.Add(itemPurchaseOrder);
                    await _UnitOfWork.SaveDbChanges();
                }
                else
                {
                    itemPurchaseOrder.UpdatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.ItemPurchaseOrder.UpdateChildWithParent(itemPurchaseOrder, "ItemPurchaseGoods");
                    await _UnitOfWork.SaveDbChanges();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
        [Route("DeleteItems")]
        [HttpPost]
        public async Task<IActionResult> DeleteItems(ItemPurchaseOrder itemPurchaseOrder)
        {
            try
            {
                var OptionsToDelete = _UnitOfWork.ItemPurchaseGoods.Delete(itemPurchaseOrder.id, itemPurchaseOrder.ItemPurchaseGoods);
                if (OptionsToDelete.Count() > 0)
                {
                    foreach (var item in OptionsToDelete)
                    {
                        _UnitOfWork.ItemPurchaseGoods.RemoveChild(item);

                    }
                    await _UnitOfWork.SaveDbChanges();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
       
        [Route("DeleteOrder")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int? id, [FromQuery] string UserId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _UnitOfWork.ItemPurchaseOrder.DeleteOrder(id);
            order.IsActive = false;
            order.DeletedBy = UserId;
            order.DeletedAt = Helpers.GetCurrentDateTime();
            _UnitOfWork.ItemPurchaseOrder.Update(order);
            await _UnitOfWork.SaveDbChanges();
            return NoContent();
        }
        [Route("GetOrderNo")]
        [HttpGet]
        public string GetOrderNo()
        {
            string orderno = _UnitOfWork.ItemPurchaseOrder.OrderNo();
            return orderno;
        }
    }
}
