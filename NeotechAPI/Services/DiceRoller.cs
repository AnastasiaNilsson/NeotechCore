
namespace Services.Dice;

public class DiceRoller
{
    private Random _random;
    public DiceRoller() => _random = new Random();

    public int RollOne(Die numberOfSides) => _random.Next(1, (int)numberOfSides + 1);

    public PreliminaryRoll RollDice(int? extraDice = null, bool useAutoDice = false)
    {
        var diceToRoll = 2 + (extraDice ?? 0);

        var diceRolls = Enumerable.Range(1, diceToRoll).Select(_ => {
            var rolls = new List<int>();

            int roll;
            do {
                roll = RollOne(Die.d10);
                rolls.Add(roll);
            } while (roll == 10);

            return rolls;
        }).ToList();

        return new PreliminaryRoll(diceRolls, useAutoDice);
    }
}
