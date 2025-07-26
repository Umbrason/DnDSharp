namespace DnDSharp.Core
{
    public class Combat
    {
        public static Combat Initiate(IEnumerable<Character> characters) => new(characters);
        private Combat(IEnumerable<Character> characters)
        {
            foreach (var cha in characters)
                AddCharacter(cha);
        }

        public Character? ActiveCharacter { get; private set; }
        private readonly SortedDictionary<int, List<Character>> InitiativeOrder = [];

        struct InitiativePtr(int initiativeGroup, int groupIndex)
        {
            public int initiative = initiativeGroup;
            public int groupIndex = groupIndex;
        }
        private InitiativePtr initPtr;
        public void MoveNext()
        {
            if (InitiativeOrder.Count == 0) throw new Exception("Combat contains no Combatants!");
            if (ActiveCharacter != null)
                EndTurn(ActiveCharacter);
            if (initPtr.groupIndex < InitiativeOrder[initPtr.initiative].Count)
                initPtr.groupIndex++;
            else while (!InitiativeOrder.ContainsKey(initPtr.initiative))
                {
                    initPtr.initiative--;
                    if (initPtr.initiative == 0)
                        initPtr.initiative = InitiativeOrder.Keys.Last();
                }
            BeginTurn(InitiativeOrder[initPtr.initiative][initPtr.groupIndex]);
        }

        private void BeginTurn(Character character)
        {
            if (ActiveCharacter != null) throw new Exception($"Need to end {character}'s turn first!");
            ActiveCharacter = character;
            character.BeginTurn();
        }

        private void EndTurn(Character character)
        {
            if (ActiveCharacter != character) throw new Exception($"It's not {character}'s turn!");
            ActiveCharacter = null;
            character.EndTurn();
        }

        public void AddCharacter(Character character)
        {
            var initiative = character.RollInitiative();
            var initGroup = InitiativeOrder.GetOrAdd(initiative.Value.Current);
            int groupIndex;
            for (groupIndex = 0; groupIndex < initGroup.Count; groupIndex++)
            {
                Character? other = initGroup[groupIndex];
                if (other.InitiativeBonus.Current < character.InitiativeBonus.Current) break;
            }
            if (initPtr.initiative == initiative.Value.Current && initPtr.groupIndex > groupIndex)
                initPtr.groupIndex++;
            initGroup.Insert(groupIndex, character);
        }
    }
}