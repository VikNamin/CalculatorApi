using CalculatorApi.Models;
using CalculatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : Controller
    {
        private readonly CalculatorInterface _calculatorService;
        public CalculatorController(CalculatorInterface calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpGet("add")]
        public IActionResult Add(double a, double b)
        {
            return Ok(_calculatorService.Add(a, b));
        }

        [HttpGet("subtract")]
        public IActionResult Subtract(double a, double b)
        {
            return Ok(_calculatorService.Subtract(a, b));
        }

        [HttpGet("multiply")]
        public IActionResult Multiply(double a, double b)
        {
            return Ok(_calculatorService.Multiply(a, b));
        }

        [HttpGet("divide")]
        public IActionResult Divide(double a, double b)
        {
            try
            {
                return Ok(_calculatorService.Divide(a, b));
            }
            catch (DivideByZeroException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("power")]
        public IActionResult Power(double a, double b)
        {
            return Ok(_calculatorService.Power(a, b));
        }

        [HttpGet("root")]
        public IActionResult Root(double a, double b)
        {
            try
            {
                return Ok(_calculatorService.Root(a, b));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("evaluate")]
        public IActionResult Evaluate([FromBody] CalculationRequest request)
        {
            try
            {
                return Ok(new CalculationResponse
                {
                    Result = _calculatorService.EvaluateExpression(request.Expression)
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
