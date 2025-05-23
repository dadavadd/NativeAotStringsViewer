using Lreks.Interfaces;
using Lreks.Structs;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static Windows.Win32.PInvoke;
using static Windows.Win32.System.Threading.PROCESS_ACCESS_RIGHTS;

namespace Lreks;

public class AotRuntimeReader : IMemoryReader, IDisposable
{
    private readonly SafeHandle _processHandle;
    public ProcessSegmentStartInfo SegmentStartInfo { get; private set; }

    public AotRuntimeReader(string processName)
    {
        using var process = Process.GetProcessesByName(processName.Replace(".exe", ""))[0];

        var mod = process.MainModule;

        SegmentStartInfo = new() { Offset = mod!.BaseAddress + 0x1000, Size = mod!.ModuleMemorySize };

        _processHandle = OpenProcess_SafeHandle(PROCESS_ALL_ACCESS, false, (uint)process.Id);
    }

    public unsafe T Read<T>(IntPtr address) where T : unmanaged
    {
        T result;
        ReadProcessMemory(_processHandle, address.ToPointer(), &result, (nuint)sizeof(T), null);
        return result;
    }

    public unsafe Span<byte> ReadBytes(IntPtr address, int size)
    {
        var buffer = new byte[size];
        fixed (byte* bufPtr = buffer)
            ReadProcessMemory(_processHandle, address.ToPointer(), bufPtr, (nuint)size, null);
        return buffer;
    }

    public void Dispose()
    {
        _processHandle.Dispose();
    }
}
