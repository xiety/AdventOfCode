using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem20;

public class Solver : IProblemSolver<long>
{
    const string BroadcasterName = "broadcaster";

    public long RunA(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);
        var graph = Create(items);

        const int total = 1000;

        var dic = new Dictionary<bool, long>() { [false] = 0, [true] = 0 };
        var conjDic = new Dictionary<string, List<long>>();

        var button = graph.OfType<RadioButton>().First();

        var currentList = new List<Radio>();
        var newList = new List<Radio>();

        for (var i = 0L; i < total; ++i)
        {
            currentList.Add(button);
            Process(currentList, newList, dic, conjDic, false, i + 1L);
        }

        return dic.Select(a => a.Value).Mul();
    }

    public long RunB(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);
        var graph = Create(items);

        var deadend = graph.OfType<RadioDeadend>().First();

        var dic = new Dictionary<bool, long>() { [false] = 0, [true] = 0 };

        var buttonPress = 0L;

        var button = graph.OfType<RadioButton>().First();

        var currentList = new List<Radio>();
        var newList = new List<Radio>();

        var conjDic = new Dictionary<string, List<long>>();

        var prev = FindConj(deadend).Inputs.Select(a => a.From.Name).ToList();

        do
        {
            currentList.Add(button);

            buttonPress++;

            if (Process(currentList, newList, dic, conjDic, true, buttonPress))
                break;

            if (conjDic.Count(a => prev.Contains(a.Key)) == prev.Count
             && conjDic.Where(a => prev.Contains(a.Key)).All(a => a.Value.Count > 1))
                break;
        }
        while (true);

        return MathExtensions.Lcm(conjDic.Where(a => prev.Contains(a.Key))
            .Select(a => (int)(a.Value[1] - a.Value[0])));
    }

    static RadioConjunction FindConj(Radio parent)
    {
        var child = parent.Inputs.Select(a => a.From).OfType<RadioConjunction>().FirstOrDefault();
        return child ?? parent.Inputs.Select(a => a.From).Select(FindConj).First();
    }

    static bool Process(List<Radio> currentList, List<Radio> newList, Dictionary<bool, long> dic, Dictionary<string, List<long>> conjDic, bool exit, long buttonPress)
    {
        var step = 1L;

        do
        {
            foreach (var radio in currentList)
            {
                switch (radio)
                {
                    case RadioButton:
                    case RadioBroadcaster:
                        SendSignal(dic, newList, radio, false, exit);
                        break;
                    case RadioFlipflop flipflop:
                        Flipflop(newList, dic, flipflop, exit);
                        break;
                    case RadioConjunction conjunction:
                        Conjunction(newList, dic, conjunction, conjDic, exit, buttonPress);
                        break;

                    case RadioDeadend:
                        {
                            var (_, pulse) = radio.PulseQueue.Dequeue();

                            if (pulse == false && exit)
                                return true;
                            break;
                        }
                }

                step++;
            }

            (currentList, newList) = (newList, currentList);
            newList.Clear();
        }
        while (currentList.Count > 0);

        return false;
    }

    static void Flipflop(List<Radio> newList, Dictionary<bool, long> dic, RadioFlipflop flipflop, bool exit)
    {
        var (connection, pulse) = flipflop.PulseQueue.Dequeue();

        connection.PulseMemory = pulse;

        if (pulse == false)
        {
            flipflop.Turn = !flipflop.Turn;

            SendSignal(dic, newList, flipflop, flipflop.Turn, exit);
        }
    }

    static readonly Predicate<Connection> Check1 = a => a.PulseMemory;

    static void Conjunction(List<Radio> newList, Dictionary<bool, long> dic, RadioConjunction conjunction, Dictionary<string, List<long>> conjDic, bool exit, long buttonPress)
    {
        var (connection, pulse) = conjunction.PulseQueue.Dequeue();

        connection.PulseMemory = pulse;

        var allHigh = Array.TrueForAll(conjunction.Inputs, Check1);
        var nextPulse = !allHigh;

        if (nextPulse)
        {
            var list = conjDic.GetOrCreate(connection.To.Name, () => []);
            list.Add(buttonPress);
        }

        SendSignal(dic, newList, conjunction, nextPulse, exit);
    }

    static void SendSignal(Dictionary<bool, long> dic, List<Radio> newList, Radio radio, bool pulse, bool exit)
    {
        var size = radio.Outputs.Length;

        for (var i = 0; i < size; ++i)
        {
            var connection = radio.Outputs[i];
            connection.To.PulseQueue.Enqueue((connection, pulse));

            newList.Add(connection.To);

            if (!exit)
                dic[pulse]++;
        }
    }

    static Radio[] Create(Item[] items)
    {
        var radios = new List<Radio>();

        var button = new RadioButton { Name = "button" };
        radios.Add(button);

        foreach (var item in items)
        {
            var radioName = GetName(item.Name);

            Radio radio;

            if (item.Name == BroadcasterName)
                radio = new RadioBroadcaster { Name = radioName };
            else if (item.Name.StartsWith('%'))
                radio = new RadioFlipflop { Name = radioName };
            else if (item.Name.StartsWith('&'))
                radio = new RadioConjunction { Name = radioName };
            else
                throw new();

            radios.Add(radio);
        }

        foreach (var item in items)
        {
            var fromName = GetName(item.Name);
            var fromRadio = radios.First(a => a.Name == fromName);

            if (fromName == BroadcasterName)
            {
                var connection = new Connection { From = button, To = fromRadio };
                button.Outputs = [.. button.Outputs, connection];
                fromRadio.PulseQueue.Enqueue((connection, false));
            }

            foreach (var toName in item.Outputs)
            {
                var toRadio = radios.FirstOrDefault(a => a.Name == toName);

                if (toRadio is null)
                {
                    toRadio = new RadioDeadend() { Name = toName };
                    radios.Add(toRadio);
                }

                var connection = new Connection { From = fromRadio, To = toRadio };

                fromRadio.Outputs = [.. fromRadio.Outputs, connection];
                toRadio.Inputs = [.. toRadio.Inputs, connection];
            }
        }

        return [.. radios];
    }

    static string GetName(string text)
    {
        if (text == BroadcasterName)
            return text;
        else if (text.StartsWith('%'))
            return text[1..];
        else if (text.StartsWith('&'))
            return text[1..];
        else
            throw new();
    }
}

record Item(string Name, string[] Outputs);

abstract class Radio
{
    public required string Name;

    public Connection[] Inputs = [];
    public Connection[] Outputs = [];

    public readonly Queue<(Connection, bool)> PulseQueue = [];
}

class RadioButton : Radio
{
}

class RadioBroadcaster : Radio
{
}

class RadioDeadend : Radio
{
}

class RadioFlipflop : Radio
{
    public bool Turn;
}

class RadioConjunction : Radio
{
}

class Connection
{
    public required Radio From;
    public required Radio To;

    public bool PulseMemory;
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Name)}>.\w+|broadcaster) \-\> ((?<{nameof(Item.Outputs)}>\w+)(\, )?)+$")]
    public static partial Regex Regex();
}
