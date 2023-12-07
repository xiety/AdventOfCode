using Advent.Common;

namespace A2023.Problem07;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var hands = File.ReadAllLines(filename).Select(Parse);

        var result = hands
            .Select(a => (Hand: a, Combo: Calc(a.Cards)))
            .OrderBy(a => a.Combo)
            .ThenBy(a => a.Hand.Cards, new ArrayComparer<Card>())
            .Select((tuple, index) => tuple.Hand.Bid * (index + 1))
            .Sum();

        return result;
    }

    private Combo Calc(Card[] cards)
    {
        var grouped = cards.GroupBy(a => a).Select(a => a.Count()).OrderDescending().ToArray();

        return grouped switch
        {
            [5] => Combo.FiveOfAKind,
            [4, 1] => Combo.FourOfAKind,
            [3, 2] => Combo.FullHouse,
            [3, 1, 1] => Combo.ThreeOfAKind,
            [2, 2, 1] => Combo.TwoPairs,
            [2, 1, 1, 1] => Combo.OnePair,
            _ => Combo.HighCard,
        };
    }

    private Hand Parse(string line)
    {
        var bid = int.Parse(line[6..]);
        var cards = line[..5].Select(ParseCard).ToArray();
        return new(cards, bid);
    }

    private Card ParseCard(char c)
        => c switch
        {
            '2' => Card.Two,
            '3' => Card.Three,
            '4' => Card.Four,
            '5' => Card.Five,
            '6' => Card.Six,
            '7' => Card.Seven,
            '8' => Card.Eight,
            '9' => Card.Nine,
            'T' => Card.Ten,
            'J' => Card.Jack,
            'Q' => Card.Queen,
            'K' => Card.King,
            'A' => Card.Ace,
        };
}

public record Hand(Card[] Cards, int Bid);

public enum Combo
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}

public enum Card : int
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace,
}
