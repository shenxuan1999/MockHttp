



namespace MockHttp.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }



    [HttpGet]
    public IActionResult DocxToPdf()
    {
        var docxPath = Path.Combine(AppContext.BaseDirectory, "1.docx");
        if (!System.IO.File.Exists(docxPath))
            return NotFound("源文件不存在");

        // 输出目录（推荐单独的临时目录，避免并发冲突）
        var outputDir = Path.Combine(AppContext.BaseDirectory, Guid.NewGuid().ToString());
        Directory.CreateDirectory(outputDir);

        var pdfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(docxPath) + ".pdf");

        try
        {
            // 根据系统选择转换方法
            if (OperatingSystem.IsWindows())
            {
                ConvertWithLibreOfficeWindows(docxPath, outputDir);
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                ConvertWithLibreOfficeLinux(docxPath, outputDir);
            }
            else
            {
                return StatusCode(500, "不支持的操作系统");
            }

            if (!System.IO.File.Exists(pdfPath))
                return StatusCode(500, "PDF 生成失败");

            // 读取 PDF 并返回
            var pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            return File(pdfBytes, "application/pdf", Path.GetFileName(pdfPath));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "转换pdf失败");
        }
        finally
        {
            // 清理临时目录（防止异常时遗留文件）
            try
            {
                if (Directory.Exists(outputDir))
                    Directory.Delete(outputDir, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "删除文件夹失败");
            }
        }
        return StatusCode(500, "转换过程中发生错误");
    }

    private static void ConvertWithLibreOfficeWindows(string docxPath, string outputDir)
    {
        var sofficePath = @"C:\Program Files\LibreOffice\program\soffice.exe";

        if (!System.IO.File.Exists(sofficePath))
            throw new FileNotFoundException("未找到 LibreOffice soffice.exe", sofficePath);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = sofficePath,
                Arguments = $"--headless --convert-to pdf \"{docxPath}\" --outdir \"{outputDir}\"",
                WorkingDirectory = Path.GetDirectoryName(sofficePath),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            throw new Exception("LibreOffice 转换失败：" + error);
        }
    }

    private static void ConvertWithLibreOfficeLinux(string docxPath, string outputDir)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "libreoffice",
                Arguments = $"--headless --convert-to pdf \"{docxPath}\" --outdir \"{outputDir}\"",
                WorkingDirectory = "/tmp",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            throw new Exception("LibreOffice 转换失败：" + error);
        }
    }


}
public class MyFormModel
{
    public IFormFile File { get; set; }
    public string UserId { get; set; }
}

