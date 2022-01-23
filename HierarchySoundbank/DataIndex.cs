using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HierarchySoundbank;

public record DataIndex : IChunk {
    public DataIndex(ReadOnlySpan<byte> data) {
        var index = MemoryMarshal.Cast<byte, DataIndexEntry>(data);
        Entries.EnsureCapacity(index.Length);
        foreach (var entry in index) {
            Entries[entry.Id] = entry;
        }
    }

    public Dictionary<uint, DataIndexEntry> Entries { get; } = new();

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public record struct DataIndexEntry(uint Id, uint Offset, uint Size);
}
