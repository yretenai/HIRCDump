using System.Runtime.InteropServices;

namespace HIRCDump.HierarchyChunks;

public record HierarchyAction : IChunk {
    public HierarchyAction(ReadOnlySpan<byte> buffer) {
        ActionType = MemoryMarshal.Read<ushort>(buffer);
        Target = MemoryMarshal.Read<uint>(buffer[2..]);
    } 
    
    public ushort ActionType { get; }
    public uint Target { get; }
}
