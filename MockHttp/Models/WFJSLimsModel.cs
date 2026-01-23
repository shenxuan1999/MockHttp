namespace MockHttp.Models;

public class WFJSLimsModel
{
}
/// <summary>
/// 检测数据表
/// </summary>
public class DwiFacilityInspectionData
{
    /// <summary>
    /// 样品id
    /// </summary>
    public string? OrderId { get; set; }

    /// <summary>
    /// 测试任务id
    /// </summary>
    public string? OrderTaskId { get; set; }

    /// <summary>
    /// 测试id
    /// </summary>
    public string? TestId { get; set; }

    /// <summary>
    /// 结果表id
    /// </summary>
    public string? OrderTaskResultId { get; set; }

    /// <summary>
    /// 样品类型（中控，产品，原料）
    /// </summary>
    public string? ScategoryName { get; set; }

    /// <summary>
    /// 样品名称
    /// </summary>
    public string? MtlName { get; set; }

    /// <summary>
    /// 装置名称
    /// </summary>
    public string? DeviceName { get; set; }

    /// <summary>
    /// 取样点名称
    /// </summary>
    public string? SampleDotName { get; set; }

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? ItemName { get; set; }

    /// <summary>
    /// 指标值
    /// </summary>
    public string? ReportResult { get; set; }

    /// <summary>
    /// 指标单位
    /// </summary>
    public string? UnitName { get; set; }

    /// <summary>
    /// 取样时间
    /// </summary>
    public DateTime? SampleTime { get; set; }

    /// <summary>
    /// 录入时间
    /// </summary>
    public DateTime? SubmittedTime { get; set; }

    /// <summary>
    /// 报出时间
    /// </summary>
    public DateTime? FinishTime { get; set; }

    /// <summary>
    /// 高限a
    /// </summary>
    public string? HighLimitA { get; set; }

    /// <summary>
    /// 高限b
    /// </summary>
    public string? HighLimitB { get; set; }

    /// <summary>
    /// 高限c
    /// </summary>
    public string? HighLimitC { get; set; }

    /// <summary>
    /// 低限a
    /// </summary>
    public string? LowLimitA { get; set; }

    /// <summary>
    /// 低限b
    /// </summary>
    public string? LowLimitB { get; set; }

    /// <summary>
    /// 低限c
    /// </summary>
    public string? LowLimitC { get; set; }

    /// <summary>
    /// 创建人(Data creator)
    /// </summary>
    public string? DwCreationBy { get; set; }

    /// <summary>
    /// 创建日期(Data creation time)
    /// </summary>
    public DateTime? DwCreationDate { get; set; }

    /// <summary>
    /// 最后更新人(Data last updated by)
    /// </summary>
    public string? DwLastUpdateBy { get; set; }

    /// <summary>
    /// 最后更新日期(Data last updated time)
    /// </summary>
    public DateTime? DwLastUpdateDate { get; set; }

    /// <summary>
    /// 数据批次号(Data batch No)
    /// </summary>
    public long? DwBatchNumber { get; set; }
}
/// <summary>
/// 
/// </summary>
public class DwiFacilityInspectionDataResult
{
    /// <summary>
    /// 
    /// </summary>
    public string? Total { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PageSize { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PageNum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<DwiFacilityInspectionData>? Data { get; set; }
}
/// <summary>
/// 
/// </summary>
public class DwiFacilityInspectionDataRetJSON
{
    /// <summary>
    /// 
    /// </summary>
    public DwiFacilityInspectionDataResult? Result { get; set; }
}
/// <summary>
/// 
/// </summary>
public class DwiFacilityInspectionDataRetModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? RetCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DwiFacilityInspectionDataRetJSON? RetJSON { get; set; }
}
