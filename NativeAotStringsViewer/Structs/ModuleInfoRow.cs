using Lreks.Enums;
using System.Runtime.InteropServices;

namespace Lreks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ModuleInfoRow
{
    public ReadyToRunSectionType SectionId;
    public int Flags;
    public IntPtr Start;
    public IntPtr End;
};
