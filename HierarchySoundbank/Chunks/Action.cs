using System;
using System.Runtime.InteropServices;

namespace HierarchySoundbank.Chunks;

public record Action : ChunkBase {
    public Action(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        ActionType = MemoryMarshal.Read<ActionType>(buffer);
        Target = MemoryMarshal.Read<uint>(buffer[2..]);

        // TODO(naomi)
    }

    public ActionType ActionType { get; }
    public uint Target { get; }
}
