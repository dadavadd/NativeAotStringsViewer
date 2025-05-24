using Lreks.Structs;

namespace Lreks;

public class ReadyToRunHeaderInfo(int dotnetVersion, IntPtr address, ReadyToRunHeader header)
{
    public int DotnetVersion { get; } = dotnetVersion;
    public IntPtr Address { get; } = address;
    public ReadyToRunHeader Header { get; } = header;
}
