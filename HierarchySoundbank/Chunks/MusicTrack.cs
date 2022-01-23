using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record MusicTrack : ChunkBase {
    public MusicTrack(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        Flags = buffer[offset++];
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Sources.EnsureCapacity(count);
        for (var i = 0; i < count; ++i) {
            Sources.Add(new SourceInfo(buffer, ref offset));
        }
    }

    public byte Flags { get; }
    public List<SourceInfo> Sources { get; } = new();
}
