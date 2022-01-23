using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HierarchySoundbank.Chunks;
using Action = HierarchySoundbank.Chunks.Action;

namespace HierarchySoundbank;

public record Hierarchy : IChunk {
    public Hierarchy(ReadOnlySpan<byte> buffer, uint version) {
        var count = MemoryMarshal.Read<uint>(buffer);
        var offset = 4;

        for (var i = 0; i < count; ++i) {
            var start = offset;
            var (type, size, id) = MemoryMarshal.Read<HierarchyChunkHeader>(buffer[offset..]);
            var chunkBuffer = buffer.Slice(offset + 5, (int) size);
            offset += (int) size + 5;

            var chunkOffset = 0;
            Chunks[id] = type switch {
                HierarchyType.Sound => new Sound(chunkBuffer, ref chunkOffset, version),
                HierarchyType.Action => new Action(chunkBuffer, ref chunkOffset, version),
                HierarchyType.Event => new Event(chunkBuffer, ref chunkOffset, version),
                HierarchyType.RandomContainer => new RandomContainer(chunkBuffer, ref chunkOffset, version),
                HierarchyType.SwitchContainer => new SwitchContainer(chunkBuffer, ref chunkOffset, version),
                HierarchyType.MusicSegment => new MusigSegment(chunkBuffer, ref chunkOffset, version),
                HierarchyType.MusicTrack => new MusicTrack(chunkBuffer, ref chunkOffset, version),
                HierarchyType.MusicSwitch => new MusicSwitch(chunkBuffer, ref chunkOffset, version),
                HierarchyType.MusicContainer => new MusicContainer(chunkBuffer, ref chunkOffset, version),
            };
        }
    }

    public Dictionary<uint, ChunkBase> Chunks { get; } = new();

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    public record struct HierarchyChunkHeader(HierarchyType Type, uint Size, uint Id);
}
