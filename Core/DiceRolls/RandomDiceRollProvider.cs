namespace DnDSharp.Core
{
    /// <summary>
    /// Defualt DiceRollProvider. Ignores context and just returns the next random number from System.Random
    /// </summary>
    public class RandomDiceRollProvider : IDiceRollProvider
    {
        private readonly Random Random = new();
        public int Roll(DiceType dice, DiceRollContext? context = null)
        {
            return Random.Next((int)dice) + 1;
        }
    }
}