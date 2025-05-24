using Lreks;
using Lreks.Interfaces;
using Lreks.Structs;

namespace NativeAotStringsViewer.Readers;

internal class ModuleInfoRowsReader(IMemoryReader memory, ReadyToRunHeaderInfo headerInfo)
{
    public unsafe ModuleInfoRow[] GetAllRows()
    {
        int count = headerInfo.Header.NumberOfSections;
        int size = headerInfo.Header.EntrySize;

        var rows = new ModuleInfoRow[count];
        var baseAddr = headerInfo.Address + sizeof(ReadyToRunHeader);

        for (int i = 0; i < count; i++)
        {
            var rowAddr = baseAddr + i * size;
            rows[i] = memory.Read<ModuleInfoRow>(rowAddr);
        }

        return rows;
    }
}