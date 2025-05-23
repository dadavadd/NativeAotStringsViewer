using Lreks.Interfaces;
using Lreks.Structs;

namespace Lreks;

internal class ModuleInfoRowsReader
{
    private readonly IMemoryReader _memory;
    private readonly ReadyToRunHeaderInfo _headerInfo;

    public ModuleInfoRowsReader(IMemoryReader memory, ReadyToRunHeaderInfo headerInfo)
    {
        _memory = memory;
        _headerInfo = headerInfo;
    }

    public unsafe ModuleInfoRow[] GetAllRows()
    {
        int count = _headerInfo.Header.NumberOfSections;
        int size = _headerInfo.Header.EntrySize;

        var rows = new ModuleInfoRow[count];
        var baseAddr = _headerInfo.Address + sizeof(ReadyToRunHeader);

        for (int i = 0; i < count; i++)
        {
            var rowAddr = baseAddr + (i * size);
            rows[i] = _memory.Read<ModuleInfoRow>(rowAddr);
        }

        return rows;
    }
}