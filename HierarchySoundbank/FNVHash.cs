using System;
using System.Runtime.CompilerServices;

namespace HierarchySoundbank;

public static class FNVHash {
    public const uint FNV_PRIME_32 = 0x01000193;
    public const uint FNV_BASIS_32 = 0x811C9DC5;
    public const uint FNV_BASIS_32_ALT = 0x8D9A085E;
    public const ulong FNV_PRIME_64 = 0x00000100000001B3;
    public const ulong FNV_BASIS_64 = 0xCBF29CE484222325;
    public const ulong FNV_BASIS_64_ALT = 0xDF8E50CB9ED4BBFE;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint fnv32(ReadOnlySpan<byte> buffer, uint basis = FNV_BASIS_32, uint prime = FNV_PRIME_32) {
        foreach (var ch in buffer) {
            basis *= prime;
            basis ^= ch;
        }

        return basis;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint fnva32(ReadOnlySpan<byte> buffer, uint basis = FNV_BASIS_32, uint prime = FNV_PRIME_32) {
        foreach (var ch in buffer) {
            basis ^= ch;
            basis *= prime;
        }

        return basis;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong fnv64(ReadOnlySpan<byte> buffer, ulong basis = FNV_BASIS_64, ulong prime = FNV_PRIME_64) {
        foreach (var ch in buffer) {
            basis *= prime;
            basis ^= ch;
        }

        return basis;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong fnva64(ReadOnlySpan<byte> buffer, ulong basis = FNV_BASIS_64, ulong prime = FNV_PRIME_64) {
        foreach (var ch in buffer) {
            basis ^= ch;
            basis *= prime;
        }

        return basis;
    }
}
