namespace MockHttp.Models;

public class ResponseData
{
    public int TotalCount { get; set; }
    public int Limit { get; set; }
    public int TotalPage { get; set; }
    public int Page { get; set; }
    public List<string>? List { get; set; }
}
public class Query
{
    public int page { get; set; }
    public int limit { get; set; }
}
public class SendWmsResponseParent
{
    /// <summary>
    /// 
    /// </summary>
    public SendWmsResponse? Message { get; set; }
    /// <summary>
    /// 
    /// </summary>

    public string? _server_messages { get; set; }
}
/// <summary>
/// 
/// </summary>
public class SendWmsResponse
{
    /// <summary>
    /// 
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Msg { get; set; }
}
