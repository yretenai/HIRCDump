namespace HIRCDump;

public record Data(ReadOnlySpan<byte> Buffer) : IChunk {
    public ReadOnlySpan<byte> GetStream(DataIndex.DataIndexEntry entry) {
        var (_, offset, size) = entry;
        if (offset > Buffer.Length ||
            size > Buffer.Length - offset) {
            return ReadOnlySpan<byte>.Empty;
        }

        return Buffer.Slice((int) offset, (int) size);
    }
}
