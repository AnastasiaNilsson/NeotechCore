namespace Services.Dice;

public static class DiceRoller
{
    private static Random _random = new Random();

    public static int RollOne(DiceType numberOfSides) => _random.Next(1, (int)numberOfSides + 1);

    public static PreliminaryRoll Roll(int? extraDice = null, bool useAutoDice = false)
    {
        var diceToRoll = 2 + (extraDice ?? 0);
        var roll = Enumerable.Range(1, diceToRoll).Select(_ => RollOne(DiceType.d10)).ToArray();
   
        return new PreliminaryRoll(roll, useAutoDice);
    }

    public static int[] Explode(this PreliminaryRoll roll, bool doubleChance = false)
    {
        var numberOfTens = roll.Dice.Where(die => die == 10).Count();
            numberOfTens += doubleChance ? roll.Dice.Where(die => die == 9).Count() : 0;

        var explosions = new List<int>();
        for (var iteration = 1; iteration <= numberOfTens; iteration++)
        {
            var result = RollOne(DiceType.d10);
            if (result == 10 || (doubleChance && result == 9))
            {
                numberOfTens++;
            }
            explosions.Add(result);
        }
        return explosions.ToArray();
    }
}
