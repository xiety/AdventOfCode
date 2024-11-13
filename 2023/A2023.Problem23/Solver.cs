﻿using Advent.Common;

namespace A2023.Problem23;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c);
        var path = LongestPathFinder.Find(map, new(1, 0), new(map.GetWidth() - 2, map.GetHeight() - 1));
        return path.Length;
    }
}

public static class LongestPathFinder
{
    public static Pos[] Find(char[,] map, Pos start, Pos end)
    {
        var star = ArrayEx.CreateAndInitialize(map.GetWidth(), map.GetHeight(), -1);

        star.Set(start, 0);

        List<Pos[]> currentSteps = [[start]];
        var newSteps = new List<Pos[]>();
        Pos[] bestPath = [];

        do
        {
            foreach (var currentPath in currentSteps)
            {
                var currentStep = currentPath[^1];

                var currentDistance = star.Get(currentStep);

                var offsets = map.Get(currentStep) switch
                {
                    '>' => [new(1, 0)],
                    '<' => [new(-1, 0)],
                    'v' => [new(0, 1)],
                    '^' => [new(0, -1)],
                    '.' => ArrayEx.Offsets,
                };

                var nextSteps = offsets
                    .Select(a => currentStep + a)
                    .Where(map.IsInBounds)
                    .Where(a => map.Get(a) != '#');

                foreach (var newStep in nextSteps)
                {
                    var oldStar = star.Get(newStep);

                    var newStar = currentDistance + 1;

                    if (oldStar == -1 || oldStar < newStar)
                    {
                        if (!currentPath.Contains(newStep))
                        {
                            star.Set(newStep, newStar);

                            if (newStep == end && bestPath.Length < currentPath.Length)
                                bestPath = currentPath;

                            Pos[] nextPath = [.. currentPath, newStep];

                            newSteps.Add(nextPath);
                        }
                    }
                }
            }

            if (newSteps.Count == 0)
                break;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);

        return bestPath;
    }
}
