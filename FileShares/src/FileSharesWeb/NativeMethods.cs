using System.Runtime.InteropServices;

namespace Steeltoe.Samples.FileSharesWeb;

internal static partial class NativeMethods
{
    [LibraryImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);
}
