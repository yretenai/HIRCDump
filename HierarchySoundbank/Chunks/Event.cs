using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record Event : ChunkBase {
    public Event(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Actions.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count).ToArray());
    }

    public List<uint> Actions { get; } = new();
}
