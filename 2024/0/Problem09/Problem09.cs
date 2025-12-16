using Advent.Common;

namespace A2024.Problem09;

public static class Solver
{
    [GeneratedTest<long>(1928, 6330095022244)]
    public static long RunA(string[] lines)
    {
        var disk = LoadData(lines);
        DefragmentA(disk);
        return Checksum(disk);
    }

    [GeneratedTest<long>(2858, 6359491814941)]
    public static long RunB(string[] lines)
    {
        var disk = LoadData(lines);
        DefragmentB(disk);
        return Checksum(disk);
    }

    static void DefragmentA(int[] disk)
    {
        var lastPos = disk.Length - 1;
        var firstPos = 0;

        do
        {
            lastPos = Array.FindLastIndex(disk, startIndex: lastPos, count: firstPos + 1, a => a != -1);

            if (lastPos == -1)
                return;

            firstPos = Array.IndexOf(disk, value: -1, startIndex: 0, count: lastPos - 1);

            if (firstPos == -1)
                return;

            MoveFile(disk, lastPos, 1, firstPos);

            firstPos++;
            lastPos--;
        }
        while (true);
    }

    static void DefragmentB(int[] disk)
    {
        var fileIndex = disk.Max();
        var previousFileStart = disk.Length - 1;

        do
        {
            var (fileStart, fileEnd) = FindFile(disk, fileIndex, previousFileStart);
            var fileLength = fileEnd - fileStart + 1;

            var found = FindFreeSpace(disk, fileStart, fileLength);

            if (found != -1)
                MoveFile(disk, fileStart, fileLength, found);

            fileIndex--;
            previousFileStart = fileStart;
        }
        while (fileIndex >= 0);
    }

    static (int, int) FindFile(int[] disk, int fileIndex, int count)
    {
        var fileEnd = Array.LastIndexOf(disk, fileIndex, startIndex: count);
        var fileStart = Array.IndexOf(disk, fileIndex);
        return (fileStart, fileEnd);
    }

    static void MoveFile(int[] disk, int from, int length, int to)
    {
        Array.Copy(disk, from, disk, to, length);

        foreach (var i in from..(from + length))
            disk[i] = -1;
    }

    static int FindFreeSpace(int[] disk, int count, int requiredLength)
    {
        var start = 0;

        do
        {
            var p1 = Array.IndexOf(disk, value: -1, startIndex: start, count: count - start);

            if (p1 == -1)
                return -1;

            var p2 = Array.FindIndex(disk, p1, count - p1 + 1, a => a != -1);

            if (p2 == -1)
                return -1;

            if (p2 - p1 >= requiredLength)
                return p1;

            start = p2 + 1;
        }
        while (start < count);

        return -1;
    }

    static long Checksum(int[] disk)
        => disk.Select((value, index) => value != -1 ? value * (long)index : 0L).Sum();

    static IEnumerable<int> ToDisk(IEnumerable<int> data)
    {
        var isFile = true;
        var index = 0;

        foreach (var n in data)
        {
            if (isFile)
            {
                foreach (var i in n)
                    yield return index;

                index++;
            }
            else
            {
                foreach (var i in n)
                    yield return -1;
            }

            isFile = !isFile;
        }
    }

    static int[] LoadData(string[] lines)
        => ToDisk(lines[0].Select(a => int.Parse($"{a}"))).ToArray();
}
