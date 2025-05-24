using Lreks;
using Lreks.Enums;
using NativeAotStringsViewer.Readers;

internal class Program
{
    private unsafe static void Main(string[] args)
    {
        Console.Title = "NativeAotStringsViewer | https://github.com/dadavadd/NativeAotStringsViewer";

        string processName = GetProcessNameFromUser();

        using var reader = new AotRuntimeReader(processName);
        var headerFinder = new ReadyToRunHeaderFinder(reader, reader.SegmentStartInfo);
        var headerInfo = headerFinder.Find();

        if (headerFinder == null)
            throw new InvalidOperationException("Binary is not a NativeAOT binary.");

        var rowsReader = new ModuleInfoRowsReader(reader, headerInfo);
        var rows = rowsReader.GetAllRows();

        var frozenRow = rows.First(r => r.SectionId == ReadyToRunSectionType.FrozenObjectRegion);

        var (strings, vtableAddr) = new StringReader(reader).ExtractAllStrings(frozenRow.Start, (int)(frozenRow.End - frozenRow.Start));

        Console.WriteLine($".NET Version: {headerInfo.DotnetVersion}");
        Console.WriteLine($"String VTable Address: {vtableAddr:X}");

        foreach (var str in strings)
        {
            Console.WriteLine(str.ToString());
        }

        Console.Read();
    }

    private static unsafe string GetProcessNameFromUser()
    {
        Console.Write("Enter the name of the process (e.g., nativeAotBinary.exe): ");
        string processName = Console.ReadLine() ?? throw new ArgumentNullException("Process name cannot be null.");
        Console.Clear();

        return processName;
    }
}
