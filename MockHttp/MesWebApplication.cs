

using System.Runtime.InteropServices;

namespace MockHttp;

public static class MesWebApplication
{
    public static WebApplicationBuilder CreateMesWebAppBuilder(string[]? args)
    {
        object obj;
        if (args == null)
        {
            obj = null;
        }
        else
        {
            string? text = args.FirstOrDefault((string s) => s.StartsWith("--env="));
            if (text == null)
            {
                obj = null;
            }
            else
            {
                string text2 = text;
                obj = text2.Substring(6, text2.Length - 6);
            }
        }
        string text3 = (string)obj;
        if (text3 != null)
        {
            IEnumerable<string> enumerable = null;
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text3)))
            {
                enumerable = File.ReadLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text3));
            }
            else if (File.Exists(text3))
            {
                enumerable = File.ReadLines(text3);
            }
            foreach (string item in enumerable ?? Array.Empty<string>())
            {
                int length = item.IndexOf('=');
                string variable = item.Substring(0, length);
                string text2 = item;
                int num = length++;
                Environment.SetEnvironmentVariable(variable, text2.Substring(num, text2.Length - num));
            }
        }
        return WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = (WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : null)
        });
    }
}
public static class WindowsServiceHelpers
{
    /// <summary>
    /// Check if the current process is hosted as a Windows Service.
    /// </summary>
    /// <returns><c>True</c> if the current process is hosted as a Windows Service, otherwise <c>false</c>.</returns>
    public static bool IsWindowsService()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }
        Process parentProcess = Win32.GetParentProcess();
        if (parentProcess == null)
        {
            return false;
        }
        if (parentProcess.SessionId == 0)
        {
            return string.Equals("services", parentProcess.ProcessName, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}
internal static class Win32
{
    [Flags]
    private enum SnapshotFlags : uint
    {
        HeapList = 1u,
        Process = 2u,
        Thread = 4u,
        Module = 8u,
        Module32 = 0x10u,
        All = 0xFu,
        Inherit = 0x80000000u,
        NoHeaps = 0x40000000u
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct PROCESSENTRY32
    {
        internal const int MAX_PATH = 260;

        internal int dwSize;

        internal int cntUsage;

        internal int th32ProcessID;

        internal IntPtr th32DefaultHeapID;

        internal int th32ModuleID;

        internal int cntThreads;

        internal int th32ParentProcessID;

        internal int pcPriClassBase;

        internal int dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal string szExeFile;
    }

    [DllImport("kernel32", SetLastError = true)]
    private static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);

    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool Process32First([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool Process32Next([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle([In] IntPtr hObject);

    internal static Process GetParentProcess()
    {
        IntPtr intPtr = IntPtr.Zero;
        try
        {
            intPtr = CreateToolhelp32Snapshot(SnapshotFlags.Process, 0u);
            PROCESSENTRY32 lppe = new PROCESSENTRY32
            {
                dwSize = Marshal.SizeOf(typeof(PROCESSENTRY32))
            };
            if (Process32First(intPtr, ref lppe))
            {
                int id = Process.GetCurrentProcess().Id;
                do
                {
                    if (id == lppe.th32ProcessID)
                    {
                        return Process.GetProcessById(lppe.th32ParentProcessID);
                    }
                }
                while (Process32Next(intPtr, ref lppe));
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            CloseHandle(intPtr);
        }
        return null;
    }
}

