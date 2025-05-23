using System;
using System.Collections.Generic;
using System.Text;

namespace Lreks.Interfaces;

public interface IMemoryReader
{
    T Read<T>(IntPtr address) where T : unmanaged;
    Span<byte> ReadBytes(IntPtr address, int size);
}
