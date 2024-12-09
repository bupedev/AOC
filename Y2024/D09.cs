using AOC.Utilities;

namespace AOC.Y2024;

public class D09DiskFragmenter() : Solution(2024, 9) {
    protected override object GetPart1Result(string input) {
        var memory = input
            .Trim()
            .Select(character => int.Parse(character.ToString()))
            .WithIndices()
            .SelectMany(
                p => p.Index % 2 == 0
                    ? Enumerable.Repeat((long?)(p.Index / 2), p.Value)
                    : Enumerable.Repeat((long?)null, p.Value)
            )
            .ToArray();

        var frontPointer = 0;
        var backPointer = memory.Length - 1;
        while (frontPointer < backPointer) {
            if (memory[frontPointer] is not null) {
                frontPointer++;
                continue;
            }

            if (memory[backPointer] is null) {
                backPointer--;
                continue;
            }

            memory[frontPointer] = memory[backPointer];
            memory[backPointer] = null;
        }

        return memory.WithIndices().Select(p => p.Value.HasValue ? p.Value.Value * p.Index : 0).Sum();
    }

    protected override object GetPart2Result(string input) {
        var blocks = input
            .Trim()
            .Select(character => int.Parse(character.ToString()))
            .WithIndices()
            .Scan(
                new Block(BlockType.Free, ..0),
                (previous, size) => {
                    var start = previous.Range.End.Value;
                    var range = start..(start + size.Value);
                    return (size.Index % 2) switch {
                        0 => new Block(BlockType.File, range, (long)size.Index / 2),
                        1 => new Block(BlockType.Free, range),
                        _ => throw new InvalidOperationException()
                    };
                }
            )
            .Where(block => block.Range.GetLength() != 0)
            .ToList();

        var fileId = blocks.Last(block => block.Type == BlockType.File).Id!.Value;
        while (fileId > 0L) Fragment(blocks, fileId--);

        return blocks
            .SelectMany(block => Enumerable.Repeat(block.Id ?? 0, block.Range.End.Value - block.Range.Start.Value))
            .WithIndices()
            .Sum(p => p.Value * p.Index);
    }

    private void Fragment(List<Block> blocks, long fileId) {
        var fileIndex = blocks.FindIndex(block => block.Id == fileId);
        var fileRange = blocks[fileIndex].Range;

        var freeIndex = blocks.FindIndex(
            block => block.Type == BlockType.Free &&
                     block.Range.GetLength() >= fileRange.GetLength() &&
                     block.Range.Start.Value < fileRange.Start.Value
        );

        if (freeIndex < 0) return;

        var splitFreeRange = blocks[freeIndex].Range.SplitOffset(fileRange.GetLength()).ToArray();
        var requireInsert = splitFreeRange.Length > 1;

        blocks[freeIndex] = new Block(BlockType.File, splitFreeRange[0], fileId);
        if (requireInsert) blocks.Insert(freeIndex + 1, new Block(BlockType.Free, splitFreeRange[1]));

        var updateFileIndex = fileIndex + (requireInsert ? 1 : 0);
        blocks[updateFileIndex] = new Block(BlockType.Free, blocks[updateFileIndex].Range);
    }

    private enum BlockType {
        File,
        Free
    }

    private sealed record Block(BlockType Type, Range Range, long? Id = null);
}