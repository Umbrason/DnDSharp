namespace DnDSharp.Core
{
    public struct DiceRollContext
    {
        public readonly Character character;
        public bool higherIsBetter;

        public DiceRollContext(Character character, bool higherIsBetter)
        {
            this.character = character;
            this.higherIsBetter = higherIsBetter;
        }
    }
}