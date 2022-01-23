using System;

namespace HierarchySoundbank.Chunks;

public abstract record ChunkBase {
    protected ChunkBase(ReadOnlySpan<byte> buffer, ref int offset, uint version) => Id = MemoryMarshalEx.Read<uint>(buffer, ref offset);

    public uint Id { get; }
}
