using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record RandomContainer : ChunkBase {
    public RandomContainer(ReadOnlySpan<byte> buffer, ref int offset, uint version) : base(buffer, ref offset, version) {
        Params = new NodeBaseParams(buffer, ref offset, version);
        Loop = MemoryMarshalEx.Read<ValueMod<ushort>>(buffer, ref offset);
        Transition = MemoryMarshalEx.Read<ValueMod<float>>(buffer, ref offset);
        AvoidRepeatCount = MemoryMarshalEx.Read<ushort>(buffer, ref offset);
        TransitionMode = buffer[offset++];
        RandomMode = buffer[offset++];
        Mode = buffer[offset++];
        Flags = buffer[offset++];
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Children.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count).ToArray());
        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        Playlist.AddRange(MemoryMarshalEx.Cast<PlaylistItem>(buffer, ref offset, count).ToArray());
    }

    public NodeBaseParams Params { get; }
    public ValueMod<ushort> Loop { get; }
    public ValueMod<float> Transition { get; }
    public ushort AvoidRepeatCount { get; }
    public byte TransitionMode { get; }
    public byte RandomMode { get; }
    public byte Mode { get; }
    public byte Flags { get; }
    public List<uint> Children { get; } = new();
    public List<PlaylistItem> Playlist { get; } = new();
}
