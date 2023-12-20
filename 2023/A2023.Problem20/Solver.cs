using System.Data;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem20;

public class Solver : IProblemSolver<long>
{
    const string broadcasterName = "broadcaster";

    public long RunA(string filename)
    {
        var items = CompiledRegs.Regex().FromFile<Item>(filename);

        var graph = Create(items);

        var total = 1000;

        var dic = new Dictionary<bool, long>() { [false] = 0, [true] = 0 };

        for (var i = 0; i < total; ++i)
        {
            var currentList = graph.OfType<RadioButton>().ToList<Radio>();
            var newList = new List<Radio>();

            do
            {
                foreach (var radio in currentList)
                {
                    if (radio is RadioButton button)
                    {
                        SendSignal(dic, newList, radio, false);
                    }
                    else if (radio is RadioBroadcaster broadcaster)
                    {
                        SendSignal(dic, newList, radio, false);
                    }
                    else if (radio is RadioFlipflop flipflop)
                    {
                        var (connection, pulse) = radio.PulseQueue.Dequeue();

                        connection.PulseMemory = pulse;

                        if (pulse == false)
                        {
                            flipflop.Turn = !flipflop.Turn;

                            SendSignal(dic, newList, radio, flipflop.Turn ? true : false);
                        }
                    }
                    else if (radio is RadioConjunction conjunction)
                    {
                        var (connection, pulse) = radio.PulseQueue.Dequeue();

                        connection.PulseMemory = pulse;

                        var allHigh = conjunction.Inputs.All(a => a.PulseMemory == true);

                        SendSignal(dic, newList, radio, !allHigh);
                    }
                }

                (currentList, newList) = (newList, currentList);
                newList.Clear();
            }
            while (currentList.Count > 0);
        }

        return dic.Select(a => a.Value).Mul();
    }

    private static void SendSignal(Dictionary<bool, long> dic, List<Radio> newList, Radio radio, bool pulse)
    {
        foreach (var connection in radio.Outputs)
        {
            connection.To.PulseQueue.Enqueue((connection, pulse));

            newList.Add(connection.To);

            dic[pulse]++;
        }
    }

    private Radio[] Create(List<Item> items)
    {
        var radios = new List<Radio>();

        var button = new RadioButton { Name = "button" };
        radios.Add(button);

        foreach (var item in items)
        {
            var radioName = GetName(item.Name);

            Radio radio;

            if (item.Name == broadcasterName)
                radio = new RadioBroadcaster { Name = radioName };
            else if (item.Name.StartsWith('%'))
                radio = new RadioFlipflop { Name = radioName };
            else if (item.Name.StartsWith('&'))
                radio = new RadioConjunction { Name = radioName };
            else
                throw new Exception();

            radios.Add(radio);
        }

        foreach (var item in items)
        {
            var fromName = GetName(item.Name);
            var fromRadio = radios.First(a => a.Name == fromName);

            if (fromName == broadcasterName)
            {
                var connection = new Connection { From = button, To = fromRadio };
                button.Outputs.Add(connection);
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

                fromRadio.Outputs.Add(connection);
                toRadio.Inputs.Add(connection);
            }
        }

        return [.. radios];
    }

    private static string GetName(string text)
    {
        if (text == broadcasterName)
            return text;
        else if (text.StartsWith('%'))
            return text[1..];
        else if (text.StartsWith('&'))
            return text[1..];
        else
            throw new Exception();
    }
}

public record Item(string Name, string[] Outputs);

public abstract class Radio
{
    public required string Name { get; init; }

    public List<Connection> Inputs { get; } = [];
    public List<Connection> Outputs { get; } = [];

    public Queue<(Connection, bool)> PulseQueue { get; } = [];
}

public class RadioButton : Radio
{
}

public class RadioBroadcaster : Radio
{
}

public class RadioDeadend : Radio
{
}

public class RadioFlipflop : Radio
{
    public bool Turn { get; set; }
}

public class RadioConjunction : Radio
{
}

public class Connection
{
    public required Radio From { get; set; }
    public required Radio To { get; set; }

    public bool PulseMemory { get; set; }
}

static partial class CompiledRegs
{
    //&bd -> nx, pz, dc, qr, cj, df, tn
    [GeneratedRegex(@$"^(?<{nameof(Item.Name)}>.\w+|broadcaster) \-\> ((?<{nameof(Item.Outputs)}>\w+)(\, )?)+$")]
    public static partial Regex Regex();
}
