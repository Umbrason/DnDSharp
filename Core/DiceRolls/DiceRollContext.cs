namespace DnDSharp.Core
{
    public readonly struct DiceRollContext
    {
        public readonly Character character;
        public readonly bool higherIsBetter;

        public DiceRollContext(Character character, bool higherIsBetter)
        {
            this.character = character;
            this.higherIsBetter = higherIsBetter;
        }
    }
}