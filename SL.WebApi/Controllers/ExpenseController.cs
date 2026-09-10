using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.Expense;

namespace SL.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Strings.ActionRoute)]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var result = await _expenseService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Category([FromBody] ExpenseCategoryCreateRequest request)
        {
            var result = await _expenseService.CreateCategoryAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Category(long id)
        {
            var result = await _expenseService.DeleteCategoryAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> History([FromQuery] ExpenseFilterParams filter)
        {
            var result = await _expenseService.GetExpenseHistoryAsync(filter);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Summary([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _expenseService.GetExpenseSummaryAsync(fromDate, toDate);
            return Ok(new SuccessResponseResult(result));
        }

        [HttpGet]
        public async Task<IActionResult> Calendar([FromQuery] int? year, [FromQuery] int? month)
        {
            var targetYear = year ?? DateTime.UtcNow.Year;
            var targetMonth = month ?? DateTime.UtcNow.Month;

            if (targetMonth < 1 || targetMonth > 12)
            {
                return BadRequest(new ErrorResponseResult("Tháng không hợp lệ. Phải từ 1 đến 12."));
            }

            var result = await _expenseService.GetFoodCalendarAsync(targetYear, targetMonth);
            return Ok(new SuccessResponseResult(result));
        }
    }
}
