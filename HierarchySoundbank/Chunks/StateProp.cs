using System.Runtime.InteropServices;

namespace HierarchySoundbank.Chunks;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct StateProp(byte Id, byte AccumilatorType, byte InDb);
