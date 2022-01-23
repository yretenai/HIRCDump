using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record StateGroup {
    public StateGroup(ReadOnlySpan<byte> buffer, ref int offset) {
        Id = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        StateAsyncType = buffer[offset++];
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        States.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count * 2).ToArray());
    }

    public uint Id { get; }
    public byte StateAsyncType { get; }
    public List<uint> States { get; } = new();
}
