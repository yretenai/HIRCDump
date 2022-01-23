using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HierarchySoundbank;

internal static class MemoryMarshalEx {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T Read<T>(ReadOnlySpan<byte> buffer, ref int offset) where T : struct {
        var value = MemoryMarshal.Read<T>(buffer[offset..]);
        offset += Unsafe.SizeOf<T>();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> Cast<T>(ReadOnlySpan<byte> buffer, ref int offset, int count) where T : struct {
        var size = Unsafe.SizeOf<T>() * count;
        var value = MemoryMarshal.Cast<byte, T>(buffer.Slice(offset, size));
        offset += size;
        return value;
    }
}
