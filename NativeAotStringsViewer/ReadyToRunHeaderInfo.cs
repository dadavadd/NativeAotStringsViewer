using Lreks.Structs;

namespace Lreks;

public class ReadyToRunHeaderInfo(int version, IntPtr address, ReadyToRunHeader header)
{
    public int Version { get; } = version;
    public IntPtr Address { get; } = address;
    public ReadyToRunHeader Header { get; } = header;
}
