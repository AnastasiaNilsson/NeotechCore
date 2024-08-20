using Neotech.Models;
using Neotech.Extensions;

namespace Neotech.Services;

public static class RollActions
{
    public static DiceSet AutomatedRoll(RollType rollType = RollType.Basic, uint extraDice = 0, uint attributeAndEdgeBonus = 0, uint difficulty = 20)
    {
        switch (rollType)
        {
            case RollType.Basic when extraDice is not 0:
                throw new ArgumentException($"No extra dice are allowed for {rollType}.");

            case RollType.Auto or RollType.Flow when extraDice is 0:
                throw new ArgumentException($"At least one extra die is required for {rollType}.");       
        }

        var rolledDice = new DiceSet(2 + extraDice, attributeAndEdgeBonus, difficulty);
        var baseDice = rollType == RollType.Flow ?
                       rolledDice.BestToKeep() :
                       rolledDice.HighestTwo() ;

        return baseDice + baseDice.Explosion();
    }
}
