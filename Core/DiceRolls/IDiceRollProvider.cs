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
            int roll = Roll(dice, context);
            for (int i = 0; i < reRolls; i++)
            {
                var reRoll = Roll(dice, context);
                if (reRoll * opSign > roll * opSign) roll = reRoll;
            }
            return new(rolls, roll, roll == 20 || roll == 1);
        }
    }
}