namespace DnDSharp.Core
{
    public class Character : Entity
    {
        public Character(IReadOnlyDictionary<AbilityID, int> abilityScores, int baseHitPoints, IReadOnlyDictionary<DamageElementID, int>  damageMultipliers, int baseArmorClass, AbilityID armorClassBonusAbility, AbilityID initiativeBonusAbility, IReadOnlyDictionary<ResourceID, int> baseResources, GroupID groupAlignment) : base(baseHitPoints, baseArmorClass, damageMultipliers)
        {
            foreach (var (abilityID, score) in abilityScores)
            {
                m_AbilityScores[abilityID] = new(score);
                m_AbilityModifiers[abilityID] = m_AbilityScores[abilityID].GetDeriving((AbilityIDScore) => AbilityIDScore / 2 - 5);
                m_SavingThrows[abilityID] = m_AbilityModifiers[abilityID].GetDeriving();
                m_MeleeWeaponAttacks[abilityID] = m_AbilityModifiers[abilityID].GetDeriving();
                m_RangedWeaponAttacks[abilityID] = m_AbilityModifiers[abilityID].GetDeriving();
                m_SpellAttacks[abilityID] = m_AbilityModifiers[abilityID].GetDeriving();
                m_SpellSaveDCs[abilityID] = m_AbilityModifiers[abilityID].GetDeriving();
            }
            CharacterLevel = new(0);
            ProficiencyBonus = CharacterLevel.GetDeriving(level => level / 4 + 2);
            ArmorClassBonus = m_AbilityModifiers[armorClassBonusAbility].GetDeriving();
            InitiativeBonus = m_AbilityModifiers[initiativeBonusAbility].GetDeriving();
            this.ArmorClass = ArmorClassBase + ArmorClassBonus;

            foreach (var (resourceID, capacity) in baseResources)
                m_Resources[resourceID] = new(capacity, capacity);

            GroupAlignment = groupAlignment;
        }

        private Dictionary<AbilityID, ModifyableValue<int>> m_AbilityScores = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_AbilityModifiers = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_SavingThrows = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_MeleeWeaponAttacks = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_RangedWeaponAttacks = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_SpellAttacks = [];
        private Dictionary<AbilityID, ModifyableValue<int>> m_SpellSaveDCs = [];
        private Dictionary<SkillID, CappedValue<int>> m_SkillProficiencies = [];
        private Dictionary<AbilityID, CappedValue<int>> m_SavingThrowProficiencies = [];
        private Dictionary<EquipmentTypeID, CappedValue<int>> m_EquipmentProficiencies = [];
        private Dictionary<ResourceID, CappedValue<int>> m_Resources = [];

        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> AbilityScores => m_AbilityScores;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> AbilityModifiers => m_AbilityModifiers;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> SavingThrows => m_SavingThrows;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> MeleeWeaponAttacks => m_MeleeWeaponAttacks;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> RangedWeaponAttacks => m_RangedWeaponAttacks;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> SpellAttacks => m_SpellAttacks;
        public IReadOnlyDictionary<AbilityID, ModifyableValue<int>> SpellSaveDCs => m_SpellSaveDCs;
        public IReadOnlyDictionary<SkillID, CappedValue<int>> SkillProficiencies => m_SkillProficiencies; //0, 1, 2
        public IReadOnlyDictionary<AbilityID, CappedValue<int>> SavingThrowProficiencies => m_SavingThrowProficiencies; //0, 1, 2
        public IReadOnlyDictionary<EquipmentTypeID, CappedValue<int>> EquipmentProficiencies => m_EquipmentProficiencies; //0, 1, 2
        public IReadOnlyDictionary<ResourceID, CappedValue<int>> Resources => m_Resources;

        public ModifyableValue<int> ProficiencyBonus { get; private set; }
        public ModifyableValue<int> CharacterLevel { get; private set; }

        public ModifyableValue<int> InitiativeBonus { get; private set; } //derives from AbilityModifiers
        public ModifyableValue<int> ArmorClassBonus { get; private set; } //derives from AbilityModifiers

        public GroupID GroupAlignment { get; private set; }

        private Dictionary<ClassID, List<IClassLevel>> m_ClassLevels = [];
        public IReadOnlyDictionary<ClassID, List<IClassLevel>> ClassLevels => m_ClassLevels;

        public int GetClassLevel(ClassID classID) => ClassLevels.GetValueOrDefault(classID)?.Count ?? 0;
        public void AddClassLevel(IClassLevel level)
        {
            var classID = level.classID;
            ClassLevels[classID].Add(level);
            CharacterLevel.Base += 1;
        }

        public void BeginTurn() => OnBeginTurn?.Invoke();
        public event Action? OnBeginTurn;
        public void EndTurn() => OnEndTurn?.Invoke();
        public event Action? OnEndTurn;
        public void ShortRest() => OnShortRest?.Invoke();
        public event Action? OnShortRest;
        public void LongRest() => OnLongRest?.Invoke();
        public event Action? OnLongRest;

        #region Rolling
        public Action<DiceRollResult>? OnRollInitiative;
        public DiceRollResult RollInitiative(int reRolls = 0)
        {
            var roll = IDiceRollProvider.Default.Roll(DiceType.D20, new(this, true), reRolls);

            roll.Modifiers.RegisterAdd(InitiativeBonus.Current);

            OnRollInitiative?.Invoke(roll);
            return roll;
        }

        public Action<DiceRollResult>? OnRollWeaponAttack;
        public DiceRollResult RollWeaponAttack(AbilityID weaponAbility, WeaponTypeID weaponType, bool isRanged, int reRolls = 0)
        {
            var roll = IDiceRollProvider.Default.Roll(DiceType.D20, new(this, true), reRolls);
            var attackModifier = (isRanged ? RangedWeaponAttacks : MeleeWeaponAttacks).GetValueOrDefault(weaponAbility)?.Current ?? 0;
            var weaponProficiency = EquipmentProficiencies.GetValueOrDefault(weaponType)?.Current ?? 0;

            roll.Modifiers.RegisterAdd(attackModifier);
            roll.Modifiers.RegisterAdd(weaponProficiency);
            roll.Modifiers.RegisterAdd(ProficiencyBonus.Current);

            OnRollWeaponAttack?.Invoke(roll);
            return roll;
        }

        public Action<DiceRollResult>? OnRollSpellAttack;
        public DiceRollResult RollSpellAttack(AbilityID spellcastingAbility, int reRolls = 0)
        {
            var roll = IDiceRollProvider.Default.Roll(DiceType.D20, new(this, true), reRolls);
            var abilityModifier = SpellAttacks.GetValueOrDefault(spellcastingAbility)?.Current ?? 0;

            roll.Modifiers.RegisterAdd(abilityModifier);
            roll.Modifiers.RegisterAdd(ProficiencyBonus.Current);

            OnRollSpellAttack?.Invoke(roll);
            return roll;
        }

        public Action<DiceRollResult>? OnRollSavingThrow;
        public DiceRollResult RollSavingThrow(AbilityID saveAbility, int reRolls = 0)
        {
            var roll = IDiceRollProvider.Default.Roll(DiceType.D20, new(this, true), reRolls);
            var abilityModifier = SavingThrows.GetValueOrDefault(saveAbility)?.Current ?? 0;
            var savingThrowProficiency = SavingThrowProficiencies.GetValueOrDefault(saveAbility)?.Current ?? 0;

            roll.Modifiers.RegisterAdd(abilityModifier);
            roll.Modifiers.RegisterAdd(ProficiencyBonus.Current * savingThrowProficiency);

            OnRollSavingThrow?.Invoke(roll);
            return roll;
        }

        public Action<DiceRollResult>? OnRollSkillCheck;
        public DiceRollResult RollSkillCheck(SkillID skillID, int reRolls = 0)
        {
            var roll = IDiceRollProvider.Default.Roll(DiceType.D20, new(this, true), reRolls);
            var abilityModifier = AbilityModifiers.GetValueOrDefault(skillID.baseAbility)?.Current ?? 0;
            var skillProficiency = SkillProficiencies.GetValueOrDefault(skillID)?.Current ?? 0;

            roll.Modifiers.RegisterAdd(abilityModifier);
            roll.Modifiers.RegisterAdd(ProficiencyBonus.Current * skillProficiency);

            OnRollSkillCheck?.Invoke(roll);
            return roll;
        }

        public Action<DiceRollResult>? OnRollDamage;
        public DamageRollResult RollDamage(DamageRollDescription damageDescription, Action<DiceRollResult>? OnRollDamage, int reRolls)
            => damageDescription.Roll((roll) => { this.OnRollDamage?.Invoke(roll); OnRollDamage?.Invoke(roll); }, new(this, true), reRolls);
        public Action<DiceRollResult>? OnRollSpellDamage;
        public DamageRollResult RollSpellDamage(DamageRollDescription damageDescription, int reRolls)
            => RollDamage(damageDescription, OnRollSpellDamage, reRolls);
        public Action<DiceRollResult>? OnRollRangedDamage;
        public DamageRollResult RollRangedDamage(DamageRollDescription damageDescription, int reRolls)
            => RollDamage(damageDescription, OnRollRangedDamage, reRolls);
        public Action<DiceRollResult>? OnRollMeleeDamage;
        public DamageRollResult RollMeleeDamage(DamageRollDescription damageDescription, int reRolls)
            => RollDamage(damageDescription, OnRollMeleeDamage, reRolls);
        #endregion
    }
}
