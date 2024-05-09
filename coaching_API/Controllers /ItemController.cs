using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Business.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Helper;

namespace AQAcademy_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ItemController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }
        [Route("GetItemCategories")]
        [HttpGet]
        public IActionResult GetAllItemCategories()
        {
            var itemCategories = _UnitOfWork.ItemCategory.GetItemCategories();
            return Ok(itemCategories);
        }
        [Route("GetAllItems")]
        [HttpGet]
        public List<Item> GetAllItems()
        {
            var ItemsList = _UnitOfWork.Item.GetActiveItems();
            return ItemsList;
        }
        [Route("GetActiveItemsForReport")]
        [HttpGet]
        public List<Item> GetActiveItemsForReport()
        {
            var ItemsList = _UnitOfWork.Item.GetActiveItemsForReport();
            return ItemsList;
        }
        [Route("GetItemsByItemCategory")]
        [HttpGet]
        public List<Item> GetItemsByItemCategory(int id)
        {
            var ItemsList = _UnitOfWork.Item.GetActiveItems().Where(x => x.ItemCategoryId == id).ToList();
            return ItemsList;
        }

       
        [Route("AddOrEditItem")]
        [HttpPost]
        public async Task<bool> PostItem(Item item)
        {

            //Item item = null;
            if (_UnitOfWork.Item.CheckDuplicateItem(item.ItemName, item.id))
            {
                throw new Exception("Another Item already exists with this Name");
            }
            else if (_UnitOfWork.Item.CheckDuplicateCode(item.ItemCode, item.id))
            {
                throw new Exception("Another Item already exists with this Code");
            }
            else
            {
                if (item.id == 0)
                {

                    item.CreatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.Item.Add(item);
                    await _UnitOfWork.SaveDbChanges();
                    return true;

                }
                else
                {
                    item.UpdatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.Item.Update(item);
                    await _UnitOfWork.SaveDbChanges();
                    return true;
                }
            }


        }

        [Route("DeleteItem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteItem(int? id, [FromQuery] string UserId)
        {
            if (id == null)
            {
                throw new Exception("Error: Cannot find Item Id");
            }
            var item = await _UnitOfWork.Item.DeleteItem(id);
            if (item == null)
            {
                throw new Exception("Error: Cannot find Item against Provided Item Id");
            }
            string ItemRefferenceErrorMessage = "Error: This Item Cannot be deleted because it has been reffered in following ";
            string ItemReferenceMessage = _UnitOfWork.Item.CheckIfItemIsReffered((int)id);
            if (!string.IsNullOrEmpty(ItemReferenceMessage))
            {
                ItemRefferenceErrorMessage += ItemReferenceMessage;
                throw new Exception(ItemRefferenceErrorMessage);
            }

            item.IsActive = false;
            item.DeletedBy = UserId;
            item.DeletedAt = Helpers.GetCurrentDateTime();
            _UnitOfWork.Item.Update(item);
            await _UnitOfWork.SaveDbChanges();
            return NoContent();
        }
    }
}
