using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem19;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var chunks = File.ReadAllLines(filename).Split(String.Empty).ToArray();

        var workflows = CompiledRegs.Regex1().FromLines<ItemRaw1>(chunks[0].ToArray())
            .Select(a=> new Item1(a.Name, a.Conditions.Select(b =>CompiledRegs.Regex2().MapTo<Item1Condition>(b)).ToArray(), a.LastCondition))
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
