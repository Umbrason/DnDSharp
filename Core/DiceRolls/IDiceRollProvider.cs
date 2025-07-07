namespace DnDSharp.Core
{
    public interface IDiceRollProvider
    {
        public static IDiceRollProvider Default => new RandomDiceRollProvider();
        public int Roll(DiceType dice, DiceRollContext? context = null);

        public DiceRollResult Roll(DiceType dice, DiceRollContext? context = null, int reRolls = 0)
        {
            var opSign = Math.Sign(reRolls);
            reRolls *= opSign;
            int[] rolls = new int[reRolls + 1];
            rolls[0] = Roll(dice, context);
            for (int i = 1; i <= reRolls; i++)
                rolls[i] = Roll(dice, context);
            int rollPicker(int[] rolls) => rolls.Aggregate((a, b) => (a * opSign < b * opSign) ? a : b);
            return new(rolls, rollPicker);
        }
    }
}