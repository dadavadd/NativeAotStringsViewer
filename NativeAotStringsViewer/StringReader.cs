using Lreks.Interfaces;
using System.Text;

internal class StringReader(IMemoryReader reader)
{
    private static int StringLengthOffset = IntPtr.Size;
    private static int StringFirstCharOffset = IntPtr.Size + 4;

    public readonly record struct StringInfo(IntPtr Address, string Value)
    {
        public override string ToString() 
            => $"0x{Address.ToString("X").PadLeft(IntPtr.Size * 2, '0')} | \"{Value}\"";
    }

    public (List<StringInfo>, IntPtr) ExtractAllStrings(IntPtr baseAddress, int size)
    {
        var result = new List<StringInfo>();

        var stringVTableAddress = reader.Read<IntPtr>(baseAddress + IntPtr.Size);

        for (int i = 0; i <= size - IntPtr.Size; i += IntPtr.Size)
        {
            var currentAddr = baseAddress + i;
            var potentialVTable = reader.Read<IntPtr>(currentAddr);

            if (potentialVTable != stringVTableAddress)
                continue;

            var str = ConvertAddressToString(currentAddr);
            result.Add(new(currentAddr, str));
        }

        return (result, stringVTableAddress);
    }

    private string ConvertAddressToString(IntPtr stringAddr)
    {
        int length = reader.Read<int>(stringAddr + StringLengthOffset);

        var rawBytes = reader.ReadBytes(stringAddr + StringFirstCharOffset, length * 2);
        var str = Encoding.Unicode.GetString(rawBytes);

        return EscapeString(str);
    }

    private static string EscapeString(string input)
    {
        var sb = new StringBuilder(input.Length * 2);

        foreach (char c in input)
        {
            switch (c)
            {
                case '\\': sb.Append("\\\\"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                case '\0': sb.Append("\\0"); break;
                case '"': sb.Append("\\\""); break;
                case '\'': sb.Append("\\\'"); break;
                case '\a': sb.Append("\\a"); break;
                case '\b': sb.Append("\\b"); break;
                case '\f': sb.Append("\\f"); break;
                case '\v': sb.Append("\\v"); break;
                default:
                    HandleSpecialCharacter(sb, c);
                    break;
            }
        }
        return sb.ToString();

        static void HandleSpecialCharacter(StringBuilder sb, char c)
        {
            if (char.IsControl(c) && c != '\t' && c != '\n' && c != '\r' && c != '\f' && c != '\v')
                sb.AppendFormat("\\u{0:X4}", (int)c);
            else if (c < ' ' || c > '~')
                sb.Append(c);
            else
                sb.Append(c);
        }
    }
}