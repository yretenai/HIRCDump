using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record RTPCManager {
    public RTPCManager(ReadOnlySpan<byte> buffer, ref int offset) {
        Id = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        Type = buffer[offset++];
        Accumulator = buffer[offset++];
        ParamId = buffer[offset++];
        CurveId = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Scaling = buffer[offset++];
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Points.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count * 2).ToArray());
    }

    public uint Id { get; }
    public byte Type { get; }
    public byte Accumulator { get; }
    public byte ParamId { get; }
    public int CurveId { get; }
    public byte Scaling { get; }
    public List<uint> Points { get; } = new();
}
