using System;
using System.Collections.Generic;

namespace HierarchySoundbank.Chunks;

public record NodeBaseParams {
    public NodeBaseParams(ReadOnlySpan<byte> buffer, ref int offset, uint version) {
        IsFXOverride = buffer[offset++] == 1;
        var count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            FXBypass = buffer[offset++];
            FX.AddRange(MemoryMarshalEx.Cast<FXChunk>(buffer, ref offset, count).ToArray());
        }

        IsOverrideParams = buffer[offset++] == 1;
        OverrideBusId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        DirectParentId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
        Flags = buffer[offset++];

        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            var keys = buffer.Slice(offset, count);
            var values = MemoryMarshalEx.Cast<uint>(buffer, ref offset, count);
            for (var i = 0; i < count; ++i) {
                Params[keys[i]] = values[i];
            }
        }

        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            var keys = buffer.Slice(offset, count);
            var values = MemoryMarshalEx.Cast<uint>(buffer, ref offset, count * 2);
            for (var i = 0; i < count; ++i) {
                RangeParams[keys[i]] = (values[i * 2], values[i * 2 + 1]);
            }
        }

        Positioning = buffer[offset++];

        var hasOverride = ((Positioning >> 0) & 1) == 1;
        var has3d = false;
        if (hasOverride) {
            if (version <= 129) {
                has3d = ((Positioning >> 4) & 1) == 1;
            } else {
                has3d = ((Positioning >> 1) & 1) == 1;
            }
        }

        if (has3d) {
            Positioning3D = buffer[offset++];
            if (version <= 128) {
                AttenuationId = MemoryMarshalEx.Read<uint>(buffer, ref offset);
            }
        }

        bool hasAutomation;
        if (version <= 128) {
            hasAutomation = ((Positioning3D >> 6) & 1) == 1;
        } else {
            hasAutomation = ((Positioning >> 5) & 3) != 0;
        }

        if (hasAutomation) {
            PathMode = buffer[offset++];
            TransitionTime = MemoryMarshalEx.Read<int>(buffer, ref offset);
            count = MemoryMarshalEx.Read<int>(buffer, ref offset);
            if (count > 0) {
                PositionVertices.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count * 4).ToArray());
            }

            count = MemoryMarshalEx.Read<int>(buffer, ref offset);
            if (count > 0) {
                PositionPlaylist.AddRange(MemoryMarshalEx.Cast<uint>(buffer, ref offset, count * 2).ToArray());
                PositionParams.AddRange(MemoryMarshalEx.Cast<float>(buffer, ref offset, count * 3).ToArray());
            }
        }

        Aux = buffer[offset++];
        AdvancedFlags = buffer[offset++];
        Behavior = buffer[offset++];
        MaxInstances = MemoryMarshalEx.Read<ushort>(buffer, ref offset);
        BelowBehavior = buffer[offset++];
        OverrideFlags = buffer[offset++];

        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            StateProps.AddRange(MemoryMarshalEx.Cast<StateProp>(buffer, ref offset, count).ToArray());
        }

        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            StateGroups.EnsureCapacity(count);
            for (var i = 0; i < count; ++i) {
                StateGroups.Add(new StateGroup(buffer, ref offset));
            }
        }

        count = MemoryMarshalEx.Read<int>(buffer, ref offset);
        if (count > 0) {
            RTPC.EnsureCapacity(count);
            for (var i = 0; i < count; ++i) {
                RTPC.Add(new RTPCManager(buffer, ref offset));
            }
        }
    }

    public bool IsFXOverride { get; }
    public byte FXBypass { get; }
    public List<FXChunk> FX { get; } = new();
    public bool IsOverrideParams { get; }
    public uint OverrideBusId { get; }
    public uint DirectParentId { get; }
    public byte Flags { get; }
    public Dictionary<byte, uint> Params { get; } = new();
    public Dictionary<uint, (uint, uint)> RangeParams { get; } = new();
    public byte Positioning { get; }
    public uint AttenuationId { get; }
    public byte Positioning3D { get; }
    public uint PathMode { get; }
    public int TransitionTime { get; }
    public List<uint> PositionVertices { get; } = new();
    public List<uint> PositionPlaylist { get; } = new();
    public List<float> PositionParams { get; } = new();
    public byte Aux { get; }
    public byte AdvancedFlags { get; }
    public byte Behavior { get; }
    public ushort MaxInstances { get; }
    public byte BelowBehavior { get; }
    public byte OverrideFlags { get; }
    public List<StateProp> StateProps { get; } = new();
    public List<StateGroup> StateGroups { get; } = new();
    public List<RTPCManager> RTPC { get; } = new();
}
