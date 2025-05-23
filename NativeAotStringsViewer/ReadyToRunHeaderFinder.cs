using Lreks;
using Lreks.Interfaces;
using Lreks.Structs;

public class ReadyToRunHeaderFinder
{
    private readonly IMemoryReader _memory;
    private readonly ProcessSegmentStartInfo _segment;

    public ReadyToRunHeaderFinder(IMemoryReader memory, ProcessSegmentStartInfo segment)
    {
        _memory = memory;
        _segment = segment;
    }

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

        var header = _memory.Read<ReadyToRunHeader>(headerAddress);
        return new ReadyToRunHeaderInfo(version, headerAddress, header);
    }

    private IntPtr Find(ReadOnlySpan<byte> signature)
    {
        var bytes = _memory.ReadBytes(_segment.Offset, _segment.Size);
        for (int i = 0; i <= bytes.Length - signature.Length; i++)
        {
            if (bytes.Slice(i, signature.Length).SequenceEqual(signature))
            {
                return _segment.Offset + i;
            }
        }
        return IntPtr.Zero;
    }
}
