using Microsoft.AspNetCore.Mvc;
using SalaryManagementAPI.Dtos;
using SalaryManagementAPI.Helper;
using SalaryManagementApplication;
using SalaryManagementApplication.Dtos;
using SalaryManagementDomainModel;

namespace SalaryManagementAPI.Controllers
{
    [ApiController]
    //[Route("{datatype}/[controller]")]
    public class SalaryManagementController : ControllerBase
    {
        private readonly ISalaryManagementRepository applicationService;

        public SalaryManagementController(ISalaryManagementRepository applicationService)
        {
            this.applicationService = applicationService;
        }

        [HttpPost("xml/[controller]/Add")]
        [Consumes("application/xml")]
        public async Task<IActionResult> AddXml(ApiSalaryDto request)
        {
            await applicationService.AddSalaryPayment(request.Data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("json/[controller]/Add")]
        public async Task<IActionResult> AddJson(ApiSalaryDto request)
        {
            await applicationService.AddSalaryPayment(request.Data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("custom/[controller]/Add")]
        public async Task<IActionResult> AddCustom(ApiCustomSalaryDto request)
        {
            
            var data = request.Data.ToSalaryDto();
            await applicationService.AddSalaryPayment(data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("json/[controller]/Update")]
        public async Task<IActionResult> UpdateJson(ApiSalaryDto request)
        {
            await applicationService.UpdateSalaryPayment(request.Data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("xml/[controller]/Update")]
        [Consumes("application/xml")]
        public async Task<IActionResult> UpdateXml(ApiSalaryDto request)
        {
            await applicationService.UpdateSalaryPayment(request.Data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("custom/[controller]/Update")]
        public async Task<IActionResult> UpdateCustom(ApiCustomSalaryDto request)
        {

            var data = request.Data.ToSalaryDto();
            await applicationService.UpdateSalaryPayment(data, request.OverTimeCalculator);
            return Ok();
        }

        [HttpPost("[controller]/Delete")]
        public async Task<IActionResult> Delete(SalaryDateAndNamesDto request)
        {

            await applicationService.DeleteUserSalaryPerMonth(request);
            return Ok();
        }

        [HttpGet("[controller]/GetUserSalaryPerMonth")]
        public async Task<SalaryPaymentResultDto> GetUserSalaryPerMonth(string firstName, string lastName, DateTime date) =>
            await applicationService.GetUserSalaryPerMonth(firstName, lastName, date);

        [HttpGet("[controller]/GetUserSalaryPerMonth")]
        public async Task<List<SalaryPaymentResultDto>> GetUserSalaryPerMonth(SalaryDateAndNamesDto reqest) =>
            await applicationService.GetAllUserSalaryAsync(reqest);
    }
}