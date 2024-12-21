using System.Collections;
using System.Numerics;

using Advent.Common;

namespace A2021.Problem16;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var packet = LoadFile(filename);
        var result = CalcSumRecurse(packet);
        return result;
    }

    public long RunB(string filename)
    {
        var packet = LoadFile(filename);
        var result = CalcExpressionRecurse(packet);
        return (long)result;
    }

    static Packet LoadFile(string filename)
    {
        var hex = File.ReadAllLines(filename).First();

        var bytes = Convert.FromHexString(hex);
        var br = new BitReader(bytes);

        var packet = Parse(br);

        return packet;
    }

    static int CalcSumRecurse(Packet packet)
    {
        var sum = (int)packet.Version;

        if (packet is PacketParent parent)
            sum += parent.SubPackets.Select(CalcSumRecurse).Sum();

        return sum;
    }

    static BigInteger CalcExpressionRecurse(Packet packet)
    {
        switch (packet)
        {
            case PacketLiteral literal:
                return literal.Value;

            case PacketParent parent:
                var subs = parent.SubPackets.ToArray(CalcExpressionRecurse);

                return parent.TypeId switch
                {
                    0 => subs.Sum(),
                    1 => subs.Mul(),
                    2 => subs.Min(),
                    3 => subs.Max(),
                    5 => subs[0] > subs[1] ? 1 : 0,
                    6 => subs[0] < subs[1] ? 1 : 0,
                    7 => subs[0] == subs[1] ? 1 : 0,
                };
        }

        throw new ArgumentOutOfRangeException(paramName: nameof(packet));
    }

    static Packet Parse(BitReader br)
    {
        var version = br.ReadToByte(3);
        var typeId = br.ReadToByte(3);

        if (typeId == 4)
        {
            var value = ParseValue(br);

            return new PacketLiteral(version, typeId, value);
        }
        else
        {
            var subPackets = new List<Packet>();

            var lengthTypeId = br.ReadToBool();

            if (lengthTypeId == false)
            {
                var length = (int)br.ReadToBigInteger(15);
                var startSubPackets = br.CurrentOffset;

                do
                {
                    subPackets.Add(Parse(br));
                }
                while (br.CurrentOffset - startSubPackets < length);
            }
            else
            {
                var num = (int)br.ReadToBigInteger(11);

                for (var n = 0; n < num; ++n)
                {
                    subPackets.Add(Parse(br));
                }
            }

            return new PacketParent(version, typeId, subPackets.ToArray());
        }
    }

    static BigInteger ParseValue(BitReader br)
    {
        var bools = new List<bool>();
        var flag = false;

        do
        {
            flag = br.ReadToBool();

            for (var i = 0; i < 4; ++i)
                bools.Add(br.ReadToBool());
        }
        while (flag);

        var ba = new BitArray(bools.ToArray());
        var result = ba.GetBigInteger();

        return result;
    }
}

record Packet(byte Version, byte TypeId);
record PacketLiteral(byte Version, byte TypeId, BigInteger Value) : Packet(Version, TypeId);
record PacketParent(byte Version, byte TypeId, Packet[] SubPackets) : Packet(Version, TypeId);
