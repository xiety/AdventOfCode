using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem19;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty).ToArray();

        var workflows = CompiledRegs.Regex1().FromLines<ItemRaw1>(chunks[0].ToArray())
            .Select(a => new Item1(a.Name, a.Conditions.Select(b => CompiledRegs.Regex2().MapTo<Item1Condition>(b)).ToArray(), a.LastCondition))
            .ToArray();

        var items = CompiledRegs.Regex3().FromLines<Item2>(chunks[1]).ToArray();

        var result = 0L;

        foreach (var item in items)
        {
            var workflowName = "in";

            do
            {
                var workflow = workflows.First(a => a.Name == workflowName);

                workflowName = workflow.LastOutput;

                foreach (var condition in workflow.Conditions)
                {
                    var actualValue = item.Values[Array.IndexOf(item.Variables, condition.Variable)];

                    if ((condition.Operation == ">" && actualValue > condition.Number)
                     || (condition.Operation == "<" && actualValue < condition.Number))
                    {
                        workflowName = condition.Output;
                        break;
                    }
                }

                if (workflowName == "A")
                {
                    result += item.Values.Sum();
                    break;
                }

                if (workflowName == "R")
                    break;
            }
            while (true);
        }

        return result;
    }

    private readonly static int num = 4000;

    public long RunB(string filename)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty).ToArray();

        var workflows = CompiledRegs.Regex1().FromLines<ItemRaw1>(chunks[0].ToArray())
            .Select(a => new Item1(a.Name, a.Conditions.Select(b => CompiledRegs.Regex2().MapTo<Item1Condition>(b)).ToArray(), a.LastCondition))
            .ToArray();

        var dic = new Dictionary<string, Range[]>
        {
            ["x"] = [1..num],
            ["m"] = [1..num],
            ["a"] = [1..num],
            ["s"] = [1..num],
        };

        var result = Recurse(workflows, "in", conditionNum: 0, depth: 0, dic);

        return result;
    }

    private long Recurse(Item1[] workflows, string workflowName, int conditionNum, int depth, Dictionary<string, Range[]> dic)
    {
        if (workflowName == "A")
            return dic.Values.Select(a => a.Sum(b => b.GetOffsetAndLength(num).Length + 1L)).Mul();

        if (workflowName == "R")
            return 0;

        var workflow = workflows.First(a => a.Name == workflowName);

        if (conditionNum >= workflow.Conditions.Length)
        {
            return Recurse(workflows, workflow.LastOutput, 0, depth + 1, dic);
        }
        else
        {
            var condition = workflow.Conditions[conditionNum];

            var result = 0L;

            var ranges = dic[condition.Variable];

            Range[] left;
            Range[] right;

            if (condition.Operation == "<")
                (left, right) = StripRanges(ranges, condition.Number, middleToLeft: false);
            else
                (right, left) = StripRanges(ranges, condition.Number, middleToLeft: true);

            var dicLeft = new Dictionary<string, Range[]>(dic) { [condition.Variable] = left };

            result += Recurse(workflows, condition.Output, 0, depth + 1, dicLeft);

            var dicRight = new Dictionary<string, Range[]>(dic) { [condition.Variable] = right };

            result += Recurse(workflows, workflowName, conditionNum + 1, depth + 1, dicRight);

            return result;
        }
    }

    private (Range[], Range[]) StripRanges(Range[] ranges, int divider, bool middleToLeft)
    {
        var left = new List<Range>();
        var right = new List<Range>();

        foreach (var range in ranges)
        {
            var start = range.Start.GetOffset(num);
            var end = range.End.GetOffset(num);

            if (middleToLeft)
            {
                if (end <= divider)
                    left.Add(range);
                else if (start > divider)
                    right.Add(range);
                else
                {
                    left.Add(start..divider);
                    right.Add((divider + 1)..end);
                }
            }
            else
            {
                if (end < divider)
                    left.Add(range);
                else if (start >= divider)
                    right.Add(range);
                else
                {
                    left.Add(start..(divider - 1));
                    right.Add(divider..end);
                }
            }
        }

        return (left.ToArray(), right.ToArray());
    }
}

record ItemRaw1(string Name, string[] Conditions, string LastCondition);
record Item1(string Name, Item1Condition[] Conditions, string LastOutput);
record Item1Condition(string Variable, string Operation, int Number, string Output);
record Item2(string[] Variables, int[] Values);

static partial class CompiledRegs
{
    //px{a<2006:qkq,m>2090:A,rfg}
    [GeneratedRegex(@$"^(?<{nameof(ItemRaw1.Name)}>\w+)\{{((?<{nameof(ItemRaw1.Conditions)}>\w[\<\>]\d+\:\w+)\,)+(?<{nameof(ItemRaw1.LastCondition)}>\w+)\}}$")]
    public static partial Regex Regex1();

    //a<2006:qkq
    [GeneratedRegex(@$"^(?<{nameof(Item1Condition.Variable)}>\w)(?<{nameof(Item1Condition.Operation)}>[\<\>])(?<{nameof(Item1Condition.Number)}>\d+)\:(?<{nameof(Item1Condition.Output)}>\w+)$")]
    public static partial Regex Regex2();

    //{x=2127,m=1623,a=2188,s=1013}
    [GeneratedRegex(@$"^\{{((?<{nameof(Item2.Variables)}>\w)=(?<{nameof(Item2.Values)}>\d+)\,?)+\}}$")]
    public static partial Regex Regex3();
}
