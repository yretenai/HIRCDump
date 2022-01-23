using System;

namespace HierarchySoundbank.Chunks;

public record SourceInfo {
    public SourceInfo(ReadOnlySpan<byte> buffer, ref int offset) {
        PluginType = MemoryMarshalEx.Read<ushort>(buffer, ref offset);
        PluginCompany = MemoryMarshalEx.Read<ushort>(buffer, ref offset);
        StreamType = MemoryMarshalEx.Read<StreamType>(buffer, ref offset);
        SourceId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        MediaSize = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        Flags = buffer[offset++];
        Size = MemoryMarshalEx.Read<uint>(buffer, ref offset);
    }

    public ushort PluginType { get; }
    public ushort PluginCompany { get; }
    public StreamType StreamType { get; }
    public uint SourceId { get; }
    public uint MediaSize { get; }
    public byte Flags { get; }
    public uint Size { get; }
}
