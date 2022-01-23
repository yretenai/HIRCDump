using System.Runtime.InteropServices;

namespace HierarchySoundbank.Chunks;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct FXChunk(byte Index, uint Id, bool IsShare, bool IsRender);
