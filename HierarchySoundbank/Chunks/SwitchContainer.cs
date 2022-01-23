using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record SwitchContainer : ChunkBase {
    public SwitchContainer(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        Params = new NodeBaseParams(buffer, ref offset, version);
        GroupType = buffer[offset++];
        GroupId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        DefaultId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        Validation = buffer[offset++];
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Children.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count).ToArray());
    }

    public NodeBaseParams Params { get; }
    public byte GroupType { get; }
    public uint GroupId { get; }
    public uint DefaultId { get; }
    public byte Validation { get; }
    public List<uint> Children { get; } = new();
}
