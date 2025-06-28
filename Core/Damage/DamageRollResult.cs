namespace DnDSharp.Core
{
    public readonly struct DamageRollResult(IReadOnlyDictionary<DamageElementID, ModifyableValue<int>> damage)
    {
        public readonly IReadOnlyDictionary<DamageElementID, ModifyableValue<int>> Damage { get; } = damage;
    }
}