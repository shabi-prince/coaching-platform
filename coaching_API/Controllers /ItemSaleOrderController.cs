using Application.Business.UnitOfWork;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Helper;
using System.Threading.Tasks;

namespace AQAcademy_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemSaleOrderController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ItemSaleOrderController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }
       
        [Route("GetSaleOrderDetail/{id}")]
        [HttpGet]
        public IActionResult GetSaleOrderDetail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var SaleOrderDetail = _UnitOfWork.ItemSaleOrder.GetSaleOrderDetail(id);
            if (SaleOrderDetail == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(SaleOrderDetail);
            }
        } 
        [Route("GetSaleOrderByPlayerId/{id}")]
        [HttpGet]
        public IActionResult GetSaleOrderByPlayerId(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var SaleOrderDetail = _UnitOfWork.ItemSaleOrder.GetSaleOrderByPlayerId(id);
            //if (SaleOrderDetail == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
                return Ok(SaleOrderDetail);
            //}
        }
        [Route("CreateUpdateSaleOrder")]
        [HttpPost]
        public async Task<IActionResult> CreateSaleOrderAsync(ItemSaleOders iso)
        {
            try
            {
                if (iso.id == 0)
                {
                    iso.CreatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.ItemSaleOrder.Add(iso);
                    await _UnitOfWork.SaveDbChanges();
                }
                else
                {
                    iso.UpdatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.ItemSaleOrder.UpdateChildWithParent(iso, "ItemSaleGoods");
                    //_UnitOfWork.ItemSaleOrder.Update(iso);
                    await _UnitOfWork.SaveDbChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
       
        [Route("GetOrderNo")]
        [HttpGet]
        public string GetOrderNo()
        {
            string orderno = _UnitOfWork.ItemSaleOrder.OrderNo();
            return orderno;
        }
    }
}
