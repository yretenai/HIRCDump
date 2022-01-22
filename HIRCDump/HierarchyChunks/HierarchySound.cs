using System.Runtime.InteropServices;

namespace HIRCDump.HierarchyChunks; 

public record HierarchySound : IChunk {
    public HierarchySound(ReadOnlySpan<byte> buffer) {
        Id = MemoryMarshal.Read<uint>(buffer);
    }

    public uint Id { get; }
}
