namespace MockHttp.Models;


public class JobTicketModel
{
}

/// <summary>
/// 
/// </summary>
public class LNGResponseModel<T>
{
    /// <summary>
    /// 
    /// </summary>
    public int? Code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public T? Data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long? Timestamp { get; set; }
}
/// <summary>
/// 
/// </summary>
public class LNGzyLedgerPageModel
{
    /// <summary>
    /// 
    /// </summary>
    public int TotalRows { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int TotalPages { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 
    /// </summary>

    public List<LNGzyLedgerContentModel>? Content { get; set; }
}
/// <summary>
/// 
/// </summary>
public class LNGzyLedgerContentModel
{
    /// <summary>
    /// <summary>
    /// 
    /// </summary>
    public string? TypeName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? StatusName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? TicketCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? TypeCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? AreaId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? AreaName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? RegionDepId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? RegionDepName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long? RealityStartTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long? RealityEndTime { get; set; }
}
public class LNGWorkAreaModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? DepartmentId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DepartmentName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? Level { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? DrawType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Coordinate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<LNGWorkAreaModel>? Children { get; set; }
}


#region ZZD
/// <summary>
/// 
/// </summary>
public class ZZDResponseModel<T>
{
    /// <summary>
    /// 
    /// </summary>
    public int? Code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ZZDResponseValueModel<T>? Value { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Msg { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Error { get; set; }
}
/// <summary>
/// 
/// </summary>
public class ZZDResponseValueModel<T>
{
    /// <summary>
    /// 
    /// </summary>
    public T? Data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? PageNum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? PageSize { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? TotalPages { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? TotalSize { get; set; }
}

/// <summary>
/// 中智达作业票工作区域模型
/// </summary>
public class ZZDWorkAreaModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? AreaName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Sid { get; set; }
}
/// <summary>
/// 中智达作业票
/// </summary>
public class ZZDJobTicketModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? Sid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? AreaName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? AreaId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? TypeName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? StatusName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? TaskNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? EndTime { get; set; }

}
/// <summary>
/// 
/// </summary>
public class ZZDWorkAreaQueryModel
{
    /// <summary>
    /// 
    /// </summary>
    public hseAreaInfoDTO? hseAreaInfoDTO { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public pageRequest? pageRequest { get; set; }
}
/// <summary>
/// 
/// </summary>
public class pageRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int pageNum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int pageSize { get; set; }
}
/// <summary>
/// 
/// </summary>
public class hseAreaInfoDTO
{

}
#endregion

