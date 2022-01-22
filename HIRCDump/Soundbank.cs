using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HIRCDump;

public record Soundbank {
    public enum ChunkId : uint {
        BKHD = 0x44484B42, // BankHeader
        DATA = 0x44415441, // Data
        DIDX = 0x44494458, // Data Index
        FXPR = 0x46585052, // Effects Processor
        ENVS = 0x454E5653, // Environments
        HIRC = 0x48495243, // Hierarchy
        STMG = 0x53544D47, // Sound Type Manager Groups
        STID = 0x53544944, // Sound Type IDs
    }

    public Soundbank(Stream stream) {
        Span<uint> headerBuffer = stackalloc uint[4];
        stream.Read(MemoryMarshal.AsBytes(headerBuffer));
        Debug.Assert(headerBuffer[0] == (uint) ChunkId.BKHD);
        Version = headerBuffer[1];
        Id = headerBuffer[2];
        LanguageId = headerBuffer[3];

        Debug.Assert(Version >= 128, "Version >= 128");

        Span<ChunkHeader> chunkHeaderBuffer = stackalloc ChunkHeader[1];
        while (stream.Position < stream.Length) {
            var start = (uint) stream.Position;
            stream.Read(MemoryMarshal.AsBytes(chunkHeaderBuffer));
            var target = stream.Position + chunkHeaderBuffer[0].Size - 8;
            try {
                switch (chunkHeaderBuffer[0].Id) {
                    case ChunkId.DATA: {
                        var chunkBuffer = new Span<byte>(new byte[chunkHeaderBuffer[0].Size]);
                        stream.Read(chunkBuffer);
                        Chunks[chunkHeaderBuffer[0].Id] = new Data(chunkBuffer);
                        break;
                    }
                    case ChunkId.DIDX: {
                        var chunkBuffer = new Span<byte>(new byte[chunkHeaderBuffer[0].Size]);
                        stream.Read(chunkBuffer);
                        Chunks[chunkHeaderBuffer[0].Id] = new DataIndex(chunkBuffer);
                        break;
                    }
                    case ChunkId.HIRC: {
                        var chunkBuffer = new Span<byte>(new byte[chunkHeaderBuffer[0].Size]);
                        stream.Read(chunkBuffer);
                        Chunks[chunkHeaderBuffer[0].Id] = new Hierarchy(chunkBuffer);
                        break;
                    }
                    default:
                        Chunks[chunkHeaderBuffer[0].Id] = new ChunkHeader(chunkHeaderBuffer[0].Id, start); // reuse chunk header to store offset.
                        break;
                }
            } finally {
                stream.Position = target;
            }
        }
    }

    public uint Version { get; }
    public uint Id { get; }
    public uint LanguageId { get; }
    public Dictionary<ChunkId, IChunk> Chunks { get; } = new();

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public record struct ChunkHeader(ChunkId Id, uint Size) : IChunk;
}
