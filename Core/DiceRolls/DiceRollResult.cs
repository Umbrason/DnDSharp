namespace DnDSharp.Core
{
    public readonly struct DiceRollResult
    {
        public readonly int[] Rolls { get; }
        public readonly ModifyableValue<int> Roll { get; }
        public readonly ModifyableValue<int> Modifiers { get; }
        public readonly ModifyableValue<int> Value { get; }
        public readonly bool IsCrit { get; }
        public DiceRollResult(int[] rolls, Func<int[], int> rollPicker, bool isCrit)
        {
            this.Rolls = rolls;
            this.Roll = new(rollPicker.Invoke(rolls));
            this.Modifiers = new(0);
            this.Value = Roll + Modifiers;
            this.IsCrit = isCrit;
        }
    }
}