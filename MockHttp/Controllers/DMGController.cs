

namespace MockHttp.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class DMGController : ControllerBase
{
    private readonly ILogger<DMGController> logger;

    public DMGController(ILogger<DMGController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    public dynamic GetTest()
    {
        var obj = new
        {
            Name = "DMG Get",
            Age = 18,
        };
        return obj;
    }

    [HttpPost]
    public object PostTest([FromForm] DmgQuery query)
    {
        logger.LogWarning("请求参数{@obj}", query);

        var lst = new List<DmgTest>()
                {
                    new DmgTest()
                    {
                        Name="沈祥轩111",
                        Address="浙江",
                        Age=18,
                        Date=new DateTime(2025,9,22,6,0,0)
                    },
                     new DmgTest()
                    {
                        Name="AAA111",
                        Address="河南",
                        Age=20,
                         Date=new DateTime(2025,9,22,12,0,0)
                    },
                      new DmgTest()
                    {
                        Name="sxx",
                        Address="111111",
                        Age=18,
                         Date=new DateTime(2025,9,22,15,0,0)
                    },
                };

        var data = lst.Where(x => x.Date >= query.Begin && x.Date <= query.End).ToList();
        logger.LogWarning("符合条件的返回值{@data}", data);
        return new
        {
            value = new
            {
                data = data
            }
        };
    }

    [HttpPost]
    public object PostDmr(object obj)
    {

        return new
        {
            message = new
            {
                code = "500",
                //msg="推送成功"
                msg = "推送失败，单据不存在"
            }
        };
    }
    [HttpPost]
    public object GetInventory(object obj)
    {
        return new
        {
            message = new
            {
                totalQuantity = 998
            }
        };
    }
}

public class DmgQuery
{
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
}
public class DmgTest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public DateTime Date { get; set; }
}

