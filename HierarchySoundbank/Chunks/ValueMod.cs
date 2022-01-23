using System.Runtime.InteropServices;

namespace HierarchySoundbank.Chunks;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct ValueMod<T>(T Count, T Min, T Max) where T : struct;
