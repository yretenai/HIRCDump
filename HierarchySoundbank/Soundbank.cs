using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace HierarchySoundbank;

public record Soundbank {
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
                        Chunks[chunkHeaderBuffer[0].Id] = new Hierarchy(chunkBuffer, Version);
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
