using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record MusicContainer : ChunkBase {
    public MusicContainer(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        Flags = buffer[offset++];
        Params = new NodeBaseParams(buffer, ref offset, version);
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Children.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count).ToArray());
    }

    public byte Flags { get; }
    public NodeBaseParams Params { get; }
    public List<uint> Children { get; } = new();
}
