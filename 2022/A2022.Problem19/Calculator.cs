namespace A2022.Problem19;

class Calculator(int maxLevel)
{
    public int Calculate(Item item)
    {
        var resources = new Resources(0, 0, 0, 0);
        var pack = new RobotsPack(1, 0, 0, 0);
        var context = new Context();

        return RecurseBuild(context, item, 0, resources, pack);
    }

    int RecurseBuild(Context context, Item item, int level, Resources resources, RobotsPack pack)
    {
        var geodes = resources.Geode;

        if (context.MaxGeodes < geodes)
            context.MaxGeodes = geodes;

        if (level >= maxLevel)
            return geodes;

        var maxCanProduce = QuickEvaluate(level, item, resources, pack);

        if (maxCanProduce <= context.MaxGeodes)
            return geodes;

        geodes = Build(context, item, level, resources, pack, geodes);

        geodes = Math.Max(geodes, RecurseDig(context, item, level, resources, pack, pack));

        return geodes;
    }

    int Build(Context context, Item item, int level, Resources resources, RobotsPack pack, int geodes)
    {
        if (resources.Ore >= item.GeodeOreCost && resources.Obsidian >= item.GeodeObsidianCost)
        {
            geodes = Math.Max(geodes, RecurseDig(context, item, level,
                resources with { Ore = resources.Ore - item.GeodeOreCost, Obsidian = resources.Obsidian - item.GeodeObsidianCost },
                pack,
                pack with { GeodeRobot = pack.GeodeRobot + 1 }
            ));
        }

        if (resources.Ore >= item.ObsidianOreCost && resources.Clay >= item.ObsidianClayCost)
        {
            geodes = Math.Max(geodes, RecurseDig(context, item, level,
                resources with { Ore = resources.Ore - item.ObsidianOreCost, Clay = resources.Clay - item.ObsidianClayCost },
                pack,
                pack with { ObsidianRobot = pack.ObsidianRobot + 1 }
            ));
        }

        if (resources.Ore >= item.ClayOreCost)
        {
            geodes = Math.Max(geodes, RecurseDig(context, item, level,
                resources with { Ore = resources.Ore - item.ClayOreCost },
                pack,
                pack with { ClayRobot = pack.ClayRobot + 1 }
            ));
        }

        if (resources.Ore >= item.OreOreCost)
        {
            geodes = Math.Max(geodes, RecurseDig(context, item, level,
                resources with { Ore = resources.Ore - item.OreOreCost },
                pack,
                pack with { OreRobot = pack.OreRobot + 1 }
            ));
        }

        return geodes;
    }

    int QuickEvaluate(int level, Item item, Resources resources, RobotsPack pack)
    {
        var ore = resources.Ore;
        var obsidian = resources.Obsidian;
        var geode = resources.Geode;

        var oreRobot = pack.OreRobot;
        var obsidianRobot = pack.ObsidianRobot;
        var geodeRobot = pack.GeodeRobot;

        var geodeRobotDelta = 0;

        for (var i = level; i < maxLevel; ++i)
        {
            oreRobot++;
            obsidianRobot++;
            geodeRobot += geodeRobotDelta;

            if (obsidian >= item.GeodeObsidianCost && ore >= item.GeodeOreCost)
            {
                ore -= item.GeodeOreCost;
                obsidian -= item.GeodeObsidianCost;
                geodeRobotDelta = 1;
            }
            else
            {
                geodeRobotDelta = 0;
            }

            ore += oreRobot;
            obsidian += obsidianRobot;
            geode += geodeRobot;
        }

        return geode;
    }

    int RecurseDig(Context context, Item item, int level, Resources resources, RobotsPack pack, RobotsPack newPack)
    {
        var newResources = new Resources(
            Ore: resources.Ore + pack.OreRobot,
            Clay: resources.Clay + pack.ClayRobot,
            Obsidian: resources.Obsidian + pack.ObsidianRobot,
            Geode: resources.Geode + pack.GeodeRobot);

        return RecurseBuild(context, item, level + 1, newResources, newPack);
    }

    class Context
    {
        public int MaxGeodes { get; set; }
    }
}
