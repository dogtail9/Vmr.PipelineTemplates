using Microsoft.AspNetCore.Mvc;

namespace TestDotnetWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    [HttpPost("add")]
    public ActionResult<CalculationResult> Add(CalculationRequest calculationRequest)
    {
        return new CalculationResult { Result = calculationRequest.Arg1 + calculationRequest.Arg2 };    
    }

    [HttpPost("subtract")]
    public ActionResult<CalculationResult> Subtract(CalculationRequest calculationRequest)
    {
        return new CalculationResult { Result = calculationRequest.Arg1 - calculationRequest.Arg2 };
    }
}

public class CalculationRequest
{
    public int Arg1 { get; set; }
    public int Arg2 { get; set; }
}

public class CalculationResult
{
    public int Result { get; set; }
}