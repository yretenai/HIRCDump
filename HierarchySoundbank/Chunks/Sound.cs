using System;

namespace HierarchySoundbank.Chunks;

public record Sound : ChunkBase {
    public Sound(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) => Source = new SourceInfo(buffer, ref offset);

    public SourceInfo Source { get; }
}
