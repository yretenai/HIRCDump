namespace HierarchySoundbank;

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
