using System.Runtime.InteropServices;
using HIRCDump.HierarchyChunks;

namespace HIRCDump;

public record Hierarchy : IChunk {
    public enum HierarchyType : byte {
        None = 0x00,
        State = 0x01,
        Sound = 0x02,
        Action = 0x03,
        Event = 0x04,
        Container = 0x05,
        SwitchContainer = 0x06,
        ActorMixer = 0x07,
        AudioBus = 0x08,
        LayerContainer = 0x09,
        MusicSegment = 0x0A,
        MusicTrack = 0x0B,
        MusicSwitch = 0x0C,
        MusicContainer = 0x0D,
        Attenuation = 0x0E,
        DialogueEvent = 0x0F,
        FXShareSet = 0x10,
        FXCustom = 0x11,
        AuxiliaryBus = 0x12,
        LFO = 0x13,
        Envelope = 0x14,
        AudioDevice = 0x15,
        TimeMod = 0x16,
    }

    public Hierarchy(ReadOnlySpan<byte> buffer) {
        var count = MemoryMarshal.Read<uint>(buffer);
        var offset = 4;

        for (var i = 0; i < count; ++i) {
            var start = offset;
            var (type, size, id) = MemoryMarshal.Read<HierarchyChunkHeader>(buffer[offset..]);
            var chunkBuffer = buffer.Slice(offset + 5, (int) size);
            offset += (int) size + 5;

            Chunks[id] = type switch {
                HierarchyType.Sound => new HierarchySound(chunkBuffer),
                HierarchyType.Action => new HierarchyAction(chunkBuffer),
                HierarchyType.Event => new HierarchyEvent(chunkBuffer),
                HierarchyType.Container => new HierarchyContainer(chunkBuffer),
                HierarchyType.SwitchContainer => new HierarchySwitchContainer(chunkBuffer),
                HierarchyType.MusicSegment => new HierarchyMusicSegment(chunkBuffer),
                HierarchyType.MusicTrack => new HierarchyMusicTrack(chunkBuffer),
                HierarchyType.MusicSwitch => new HierarchyMusicSwitch(chunkBuffer),
                HierarchyType.MusicContainer => new HierarchyMusicContainer(chunkBuffer),
                _ => new HierarchyChunkHeader(type, (uint) start, id), // reuse chunk header to store offset.
            };
        }
    }

    public Dictionary<uint, IChunk> Chunks { get; } = new();

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    public record struct HierarchyChunkHeader(HierarchyType Type, uint Size, uint Id) : IChunk;
}
