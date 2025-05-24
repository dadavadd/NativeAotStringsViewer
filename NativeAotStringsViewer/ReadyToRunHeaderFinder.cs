using Lreks.Interfaces;
using Lreks.Structs;

namespace Lreks;

public class ReadyToRunHeaderFinder(IMemoryReader memory, ProcessSegmentStartInfo segment)
{
    public ReadyToRunHeaderInfo Find()
    {
        IntPtr headerAddress;
        int version;

        if ((headerAddress = Find(Constants.ReadyToRunSignatureDotnet10)) != IntPtr.Zero)
        {
            version = 10;
        }
        else if ((headerAddress = Find(Constants.ReadyToRunSignatureDotnet9)) != IntPtr.Zero)
        {
            version = 9;
        }
        else
        {
            throw new InvalidOperationException("ReadyToRun header not found.");
        }

        var header = memory.Read<ReadyToRunHeader>(headerAddress);
        return new ReadyToRunHeaderInfo(version, headerAddress, header);
    }

    private IntPtr Find(ReadOnlySpan<byte> signature)
    {
        var bytes = memory.ReadBytes(segment.Offset, segment.Size);
        for (int i = 0; i <= bytes.Length - signature.Length; i++)
        {
            if (bytes.Slice(i, signature.Length).SequenceEqual(signature))
            {
                return segment.Offset + i;
            }
        }
        return IntPtr.Zero;
    }
}
