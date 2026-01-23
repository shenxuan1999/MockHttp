

namespace MockHttp.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{



    [HttpGet]
    public string GetNum()
    {
        return "111";
    }
}
