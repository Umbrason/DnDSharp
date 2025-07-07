using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public class DnDCharacter : Character
    {
        private static readonly Dictionary<ResourceID, int> BaseResources = new() { { Resource.ACTION, 1 }, { Resource.BONUS_ACTION, 1 }, { Resource.REACTION, 1 } };
        public DnDCharacter(IReadOnlyDictionary<AbilityID, int> abilityScores, int baseHitPoints, IReadOnlyDictionary<DamageElementID, int> damageMultipliers, int baseArmorClass, GroupID groupAlignment) : base(abilityScores, baseHitPoints, damageMultipliers, baseArmorClass, Ability.DEXTERITY, Ability.DEXTERITY, BaseResources, groupAlignment, new EXPCurveLevelProvider())
        {
            this.OnBeginTurn += RestoreBaseResources;
            this.OnShortRest += RestoreHitpointsOnShortRest;
            this.OnLongRest += RestoreHitpointsOnLongRest;
        }

        private void RestoreBaseResources()
        {
            foreach (var resource in BaseResources.Keys)
                this.Resources[resource].Refill();
        }

        private void RestoreHitpointsOnShortRest()
        {
            this.HitPoints.Add((HitPoints.Capacity.Current + 1) / 2);
        }

        private void RestoreHitpointsOnLongRest()
        {
            this.HitPoints.Refill();
        }
    }
}