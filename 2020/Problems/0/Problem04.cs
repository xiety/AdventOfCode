using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem04;

public class Solver : ISolver<int>
{
    public int RunA(string[] lines, bool isSample)
        => LoadData(lines).Count(ValidateRequired);

    public int RunB(string[] lines, bool isSample)
        => LoadData(lines)
            .Where(ValidateRequired)
            .Count(a => a.All(Validate));

    bool ValidateRequired(Item[] items)
        => items
        .Select(a => a.Key)
        .ContainsAll(["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"]); //TODO: remove array

    bool Validate(Item item)
        => item.Key switch
        {
            "byr" => ValidateNumber(item.Value, 1920, 2002),
            "iyr" => ValidateNumber(item.Value, 2010, 2020),
            "eyr" => ValidateNumber(item.Value, 2020, 2030),
            "hgt" => ValidateHeight(item.Value),
            "hcl" => ValidateColor(item.Value),
            "ecl" => ValidateEye(item.Value),
            "pid" => ValidatePassword(item.Value),
            _ => true,
        };

    static bool ValidateNumber(string value, int from, int to)
        => int.TryParse(value, out var n) && n >= from && n <= to;

    static bool ValidateHeight(string value)
        => CompiledRegs.RegexHeight().TryMapTo<Height>(value, out var height)
        && height switch
        {
            { Unit: "cm", Value: >= 150 and <= 193 } => true,
            { Unit: "in", Value: >= 59 and <= 76 } => true,
            _ => false,
        };

    static bool ValidateColor(string value)
        => CompiledRegs.RegexColor().IsMatch(value);

    static bool ValidateEye(string value)
        => value.Inside(["amb", "blu", "brn", "gry", "grn", "hzl", "oth"]); //TODO: remove array

    static bool ValidatePassword(string value)
        => value.Length == 9 && value.All(char.IsNumber);

    static Item[][] LoadData(string[] lines)
        => lines.SplitBy(string.Empty)
            .ToArray(a => a.ToArrayMany(d => d.Split(" ")
                .Select(b => CompiledRegs.RegexItem().MapTo<Item>(b))));
}

record Item(string Key, string Value);
record Height(int Value, string Unit);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Key)}>.+)\:(?<{nameof(Item.Value)}>.+)$")]
    public static partial Regex RegexItem();

    [GeneratedRegex(@"^\#[0-9a-f]{6}$")]
    public static partial Regex RegexColor();

    [GeneratedRegex(@$"^(?<{nameof(Height.Value)}>\d+)(?<{nameof(Height.Unit)}>(in|cm))$")]
    public static partial Regex RegexHeight();
}
