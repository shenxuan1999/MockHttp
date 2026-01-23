namespace MockHttp.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public DwiFacilityInspectionDataRetModel? GetData(int? pageNum, int? pageSize)
    {
        //return new();
        var data = new DwiFacilityInspectionDataRetModel();
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Json");
            var filePath = Path.Combine(path, "1.json");

            var json = System.IO.File.ReadAllText(filePath);



            data = JsonSerializer.Deserialize<DwiFacilityInspectionDataRetModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new NullableDateTimeConverterTwo() }
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "读取json文件异常");
            throw;
        }


        return data;
    }



    #region 库存
    [HttpPost]
    public object? GetData2(object model, [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        // 获取请求头
        var headers = httpContextAccessor.HttpContext?.Request.Headers;

        // 打印所有请求头（调试用）
        _logger.LogWarning("请求头: {@model}", model);

        // 打印特定头（如 Authorization）
        if (headers.TryGetValue("Authorization", out var authHeader))
        {
            _logger.LogWarning("Authorization头: {AuthHeader}", authHeader);
        }

        _logger.LogWarning("结果{@model}", model);
        return new SendWmsResponseParent()
        {
            _server_messages = "sshjdahdjkahdjha1111111111222",
            Message = new()
            {
                Code = "200",
                Msg = "给水管道事实上事实上少时诵诗书好贱啊公司电话"
            }
        };
    }
    [HttpPost]
    public ResponseData Wms(Query query)
    {
        Console.WriteLine(query.page + "----------" + query.limit);
        var res = new ResponseData()
        {
            TotalCount = 500,
            Limit = 100,
            TotalPage = 5,
            Page = query.page,
        };

        var strs = Enumerable.Range((query.page - 1) * 100, 100).Select(x => $"第{query.page}页第{x}条数据").ToList();

        res.List = strs;
        return res;
    }
    #endregion




    #region 作业票
    [HttpPost]
    public dynamic Token()
    {
        _logger.LogError("获取token");
        foreach (var header in Request.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
        var obj = new
        {
            Code = 1,
            Msg = "success",
            Data = new
            {
                token = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                expireTime = 1744196694150
            }
        };
        return obj;
    }
    [HttpPost]
    public dynamic ZZDTree(ZZDWorkAreaQueryModel obj)
    {
        _logger.LogWarning("中智达请求区区域模型{@obj}", obj);
        return new ZZDResponseModel<List<ZZDWorkAreaModel>>()
        {
            Code = 1,
            Msg = "1111",
            Value = new()
            {
                Data = new()
                {
                    new(){ Sid="111",AreaName="区域去1"},
                     new(){ Sid="222",AreaName="区域去222"}
                }
            }
        };
    }
    [HttpPost]
    public dynamic LNGTree()
    {
        var res = new LNGResponseModel<List<LNGWorkAreaModel>>()
        {
            Data = new()
            {
                new()
                {
                    Id = "0001",
                    Name = "测试区域1",
                    Code = "0001",
                    DepartmentId = 1,
                    DepartmentName = "成都海德",
                    Level = 1,
                    DrawType = 1,
                    Children=new(){
                    new LNGWorkAreaModel()
                    {
                        Id = "0002",
                        Name = "测试区域2",
                        Code = "0002",
                        DepartmentId = 1,
                        DepartmentName = "成都海德",
                        Level = 2,
                        DrawType = 1,
                        Children=new(){
                            new LNGWorkAreaModel()
                            {
                                Id = "0003",
                                Name = "测试区域3",
                                Code = "0003",
                                DepartmentId = 1,
                                DepartmentName = "成都海德",
                                Level = 3,
                                DrawType = 1
                            }
                        }
                    }
                }
            }
        }
        };
        return res;
    }
    [HttpPost]
    public dynamic LNGTicket()
    {
        _logger.LogError("获取lng作业票");
        foreach (var header in Request.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var res = new LNGResponseModel<LNGzyLedgerPageModel>()
        {
            Data = new()
            {
                TotalRows = 1000,
                TotalPages = 10,
                PageIndex = 1,
                PageSize = 100,
                Content = new()
                {
                new(){ TypeName="动火作业1",StatusName="作废",Id="111",TicketCode="2222",Content="作业内容",AreaId="0001",AreaName="测试区域",RegionDepId="1",RegionDepName="成都海德",RealityStartTime=1746775101000,RealityEndTime=1746775204000},
                 new(){ TypeName="动火作业2",StatusName="作废",Id="111",TicketCode="2222",Content="作业内容",AreaId="0001",AreaName="测试区域",RegionDepId="1",RegionDepName="成都海德",RealityStartTime=1746775101000,RealityEndTime=1746775204000},
                  new(){ TypeName=$"{now}",StatusName="作废",Id="111",TicketCode="2222",Content="作业内容",AreaId="0001",AreaName="测试区域",RegionDepId="1",RegionDepName="成都海德",RealityStartTime=1746775101000,RealityEndTime=1746775204000},
                }
            }
        };
        return res;
    }
    [HttpPost]
    public dynamic ZZDTicket(ZZDWorkAreaQueryModel? obj)
    {
        _logger.LogError("获取zzd作业票");
        _logger.LogWarning("中智达请求作业票模型{@obj}", obj);
        return new ZZDResponseModel<List<ZZDJobTicketModel>>()
        {
            Code = 1,
            Msg = "1111",
            Value = new()
            {
                Data = new()
                {
                    new()
                    {
                        Sid="111",
                        Content="111",
                        StatusName="发布",
                        TaskNumber="sjdaklj",
                        TypeName="隐患",
                        StartTime=DateTime.Now,
                        EndTime=DateTime.Now,
                    }
                }
            }
        };
    }
    #endregion
}
