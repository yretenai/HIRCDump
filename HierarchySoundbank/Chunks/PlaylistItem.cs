using System.Runtime.InteropServices;

namespace HierarchySoundbank.Chunks;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PlaylistItem(uint Id, int Weight);
