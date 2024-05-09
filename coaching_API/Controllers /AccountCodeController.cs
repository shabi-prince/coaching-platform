using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Business.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.DTO;
using Domain.Helper;

namespace AQAcademy_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountCodeController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public AccountCodeController(IUnitOfWork UOW)
        {
            _UnitOfWork = UOW;
        }
        [Route("GetAccountHeads")]
        [HttpGet]
        public List<AccountHead> GetAccountHeads()
        {
            return _UnitOfWork.AccountCode.GetAccountHeads();
        }
        [Route("GetAccountsForSelectBox/{id}")]
        [HttpGet]
        public List<AccountCodesDTO> GetAccountsForSelectBox(int? id)
        {
            return _UnitOfWork.AccountCode.GetAccountsForSelectBox(id);
        }
        [Route("GetAllAccountCodes")]
        [HttpGet]
        public List<AccountCodes> GetAllAccountCodes()
        {
            return _UnitOfWork.AccountCode.GetActiveAccountCodes();
        }
       

        [Route("AddOrEditAccountCode")]
        [HttpPost]
        public async Task<bool> AddOrEditAccountCode(AccountCodes AccountCode)
        {

            //Item item = null;
            if (_UnitOfWork.AccountCode.CheckIfAccountCodeIsDuplicate(AccountCode.AccountName, AccountCode.id))
            {
                throw new Exception("Another Account already exists with this Name");
            }
            else
            {
                if (AccountCode.id == 0)
                {

                    AccountCode.CreatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.AccountCode.Add(AccountCode);
                    await _UnitOfWork.SaveDbChanges();
                    return true;

                }
                else
                {
                    AccountCode.UpdatedAt = Helpers.GetCurrentDateTime();
                    _UnitOfWork.AccountCode.Update(AccountCode);
                    await _UnitOfWork.SaveDbChanges();
                    return true;
                }
            }
        }
        [Route("DeleteAccountCode")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccountCode(int? id, [FromQuery] string UserId)
        {
            if (id == null)
            {
                throw new Exception("Error: Cannot find Item Id");
            }
            var AccountCode = await _UnitOfWork.AccountCode.DeleteAccountCode((int)id);
            if (AccountCode == null)
            {
                throw new Exception("Error: Cannot find Account against Provided Item Id");
            }
            string AccountRefferenceErrorMessage = "Error: This Account Cannot be deleted because it has been reffered in following ";
            string AccountReferenceMessage = _UnitOfWork.Item.CheckIfItemIsReffered((int)id);
            if (!string.IsNullOrEmpty(AccountReferenceMessage))
            {
                AccountRefferenceErrorMessage += AccountReferenceMessage;
                throw new Exception(AccountRefferenceErrorMessage);
            }
            AccountCode.IsActive = false;
            AccountCode.DeletedBy = UserId;
            AccountCode.DeletedAt = Helpers.GetCurrentDateTime();
            _UnitOfWork.AccountCode.Update(AccountCode);
            await _UnitOfWork.SaveDbChanges();
            return NoContent();
        }
    }
}
