namespace Redis.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisTestController : ControllerBase
{
    private RedisTestService _service;

    public RedisTestController(IConfiguration configuration)
    {
        _service = new RedisTestService(configuration);
    }

    [HttpGet]
    public async Task<ActionResult<List<RedisListVM>>> GetRedisList([FromQuery] Query request)
    {
        try
        {
            var result = await _service.GetRedisList(request);
            return result;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
}

