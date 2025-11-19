using Advent.Common;

namespace A2023.Problem07;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => LoadItems(filename)
            .Select(a => (Hand: a, Combo: CalcA(a.Cards)))
            .OrderBy(a => a.Combo)
            .ThenBy(a => a.Hand.Cards.ToArray(CardScoreA), new ArrayComparer<int>())
            .Select((tuple, index) => tuple.Hand.Bid * (index + 1))
            .Sum();

    public long RunB(string filename)
        => LoadItems(filename)
            .Select(a => (Hand: a, Combo: CalcB(a.Cards)))
            .OrderBy(a => a.Combo)
            .ThenBy(a => a.Hand.Cards.ToArray(CardScoreB), new ArrayComparer<int>())
            .Select((tuple, index) => tuple.Hand.Bid * (index + 1))
            .Sum();

    static Combo CalcB(Card[] cards)
    {
        var jockers = cards.Count(a => a == Card.Jack);

        if (jockers == 5)
            return CalcA([.. Enumerable.Repeat(Card.Ace, 5)]);

        var most = cards.Where(a => a != Card.Jack)
            .GroupBy(a => a)
            .OrderByDescending(a => a.Count())
            .Select(a => a.Key)
            .First();

        return CalcA([.. cards.Select(a => a == Card.Jack ? most : a)]);
    }

    static Combo CalcA(Card[] cards)
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

    static HandA Parse(string line)
    {
        var bid = int.Parse(line[6..]);
        var cards = line[..5].ToArray(ParseCard);
        return new(cards, bid);
    }

    static Card ParseCard(char c)
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

    static int CardScoreA(Card c)
        => c switch
        {
            Card.Two => 0,
            Card.Three => 1,
            Card.Four => 2,
            Card.Five => 3,
            Card.Six => 4,
            Card.Seven => 5,
            Card.Eight => 6,
            Card.Nine => 7,
            Card.Ten => 8,
            Card.Jack => 9,
            Card.Queen => 10,
            Card.King => 11,
            Card.Ace => 12,
        };

    static int CardScoreB(Card c)
        => c switch
        {
            Card.Jack => 0,
            Card.Two => 1,
            Card.Three => 2,
            Card.Four => 3,
            Card.Five => 4,
            Card.Six => 5,
            Card.Seven => 6,
            Card.Eight => 7,
            Card.Nine => 8,
            Card.Ten => 9,
            Card.Queen => 10,
            Card.King => 11,
            Card.Ace => 12,
        };

    static IEnumerable<HandA> LoadItems(string filename)
        => File.ReadAllLines(filename).Select(Parse);
}

record HandA(Card[] Cards, int Bid);

enum Combo
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}

enum Card
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
