
namespace Services.Dice;

public class DiceRoller
{
    private Random _random;
    public DiceRoller() => _random = new Random();

    public int RollOne(DiceType numberOfSides) => _random.Next(1, (int)numberOfSides + 1);

    public void ExplodeDice(List<int> dice, bool doubleChance = false)
    {
        var numberOfTens = dice.Where(die => die == 10).Count();
        numberOfTens += doubleChance ?
                        dice.Where(die => die == 9).Count() :
                        0;
        
        for (var iteration = 1; iteration <= numberOfTens; iteration++)
        {
            var result = RollOne(DiceType.d10);
            if (result == 10 || (doubleChance && result == 9))
            {
                numberOfTens++;
            }
            dice.Add(result);
        }
    }

    public PreliminaryRoll RollDice(int? extraDice = null, bool useAutoDice = false)
    {
        var diceToRoll = 2 + (extraDice ?? 0);

        var diceRolls = Enumerable.Range(1, diceToRoll).Select(_ => {
            var rolls = new List<int>();

            int roll;
            do {
                roll = RollOne(DiceType.d10);
                rolls.Add(roll);
            } while (roll == 10);

            return rolls.ToArray();
        }).ToArray();

        return new PreliminaryRoll(diceRolls, useAutoDice);
    }
}
