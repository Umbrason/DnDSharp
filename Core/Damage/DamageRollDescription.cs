namespace DnDSharp.Core
{
    public readonly struct DamageRollDescription
    {
        public readonly Dictionary<DamageElementID, RollDescriptor> Rolls = [];

        public DamageRollDescription(params (DamageElementID, RollDescriptor)[] rollDescriptors)
        {
            foreach (var rollDescriptor in rollDescriptors)
                Rolls.Add(rollDescriptor.Item1, rollDescriptor.Item2);
        }

        public readonly struct RollDescriptor(DiceType[] dice, int flatBonus)
        {
            public readonly DiceType[] dice = dice;
            public readonly int flatBonus = flatBonus;
        }

        public readonly DamageRollResult Roll(Action<DiceRollResult> OnRollDamage, DiceRollContext context, int reRolls = 0)
        {
            var results = new Dictionary<DamageElementID, ModifyableValue<int>>();
            foreach (var element in Rolls.Keys)
            {
                var rollDescriptor = Rolls[element];
                var damageSum = new ModifyableValue<int>(rollDescriptor.flatBonus);
                foreach (var die in rollDescriptor.dice)
                {
                    var roll = IDiceRollProvider.Default.Roll(die, context, reRolls);
                    OnRollDamage?.Invoke(roll);
                    damageSum += roll.Value;
                }
                results[element] = damageSum.GetDeriving();
            }
            return new(results);
        }
    }
}