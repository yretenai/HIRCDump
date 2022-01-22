namespace HIRCDump;

public record Data : IChunk {
    public Data(ReadOnlySpan<byte> buffer) => Buffer = buffer.ToArray();

    public ReadOnlyMemory<byte> Buffer { get; }

    public ReadOnlySpan<byte> GetStream(DataIndex.DataIndexEntry entry) {
        var (_, offset, size) = entry;
        if (offset > Buffer.Length ||
            size > Buffer.Length - offset) {
            return ReadOnlySpan<byte>.Empty;
        }

        return Buffer.Span.Slice((int) offset, (int) size);
    }
}
