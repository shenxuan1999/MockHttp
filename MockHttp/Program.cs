using MockHttp;

var builder = MesWebApplication.CreateMesWebAppBuilder(args);
//builder.Configuration.AddCommandLine(args);


//var appid = builder.Configuration["MES:AppId"];
//Console.WriteLine("命令行参数appid"+appid);

//var DAPR_PATH = builder.Configuration["DAPR_PATH"];
//Console.WriteLine("环境变量参数DAPR_PATH" + DAPR_PATH);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    //命名规则，该值指定用于将对象上的属性名称转换为另一种格式(例如驼峰大小写)或为空以保持属性名称不变的策略[前端想要使用与后端模型本身命名格式输出]。
    options.JsonSerializerOptions.PropertyNamingPolicy = null;

    //自定义输出的时间格式
    options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverterTwo());
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache(); // .NET 6+
builder.Services.AddHttpContextAccessor(); // .NET 6+

builder.Host.UseSerilog((context, _, configuration) =>
{
    var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

    Console.WriteLine(context.HostingEnvironment.ApplicationName);
    var applicationName = context.HostingEnvironment.ApplicationName;

    configuration
    .MinimumLevel.Information()
    //对其他日志进行重写,除此之外,目前框架只有微软自带的日志组件
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("$app", context.HostingEnvironment.ApplicationName)
    .Enrich.WithProperty("$ms", context.HostingEnvironment.ApplicationName[..3])
    // 根据SourceContext只保留项目命名空间的日志与微软的host日志
    .Filter.ByIncludingOnly(le =>
    {
        if (!le.Properties.TryGetValue("SourceContext", out var sc)) return false;
        var val = sc.ToString();
        return val.Contains(applicationName) || val.Contains("Microsoft.AspNetCore.Hosting");
    })
    .WriteTo.Console(new ExpressionTemplate(
        "{ {@t, @mt, @l, @x, ..@p} }\n", theme: TemplateTheme.Literate)) // 控制台输出结构化 JSON
    .WriteTo.File(new ExpressionTemplate(
        "{ {@t, @mt, @l, @x, ..@p} }\n"), Path.Combine(dir, ".log"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30);


});

var app = builder.Build();

// Configure the HTTP request pipeline.

// 创建 ILogger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// 读取配置
var appid = builder.Configuration["MES:AppId"];
logger.LogInformation("命令行参数 appid: {AppId}", appid);

var DAPR_PATH = builder.Configuration["DAPR_PATH"];
logger.LogInformation("环境变量参数 DAPR_PATH: {DaprPath}", DAPR_PATH);


app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();
