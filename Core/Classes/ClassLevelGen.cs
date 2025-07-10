namespace DnDSharp.Core
{
    public class ClassLevelGen
    {
        public static ClassLevelGen? Get(ClassID classID, Character character)
        {
            return null;
        }
        public ClassLevelGen(Type targetCharacterLevelType)
        {
            TargetCharacterLevelType = targetCharacterLevelType;
        }


        private Type TargetCharacterLevelType;
        public ChoiceDescription CurrentChoice => requiredChoices[choiceDescriptionIndex];
        private List<ChoiceDescription> requiredChoices;
        private List<object[]> choices;


        int choiceDescriptionIndex = 0;
        public struct ChoiceDescription
        {
            public string ID;
            public int minPicks;
            public int maxPicks;
            public object[] options;
            public Type optionType;
        }

        public bool IsChoiceValid(int[] indices)
        {
            if (indices.Length > CurrentChoice.maxPicks) return false;
            if (indices.Length < CurrentChoice.minPicks) return false;
            if (indices.Distinct().Count() != indices.Length) return false;
            if (indices.Any(i => i < 0 || i >= CurrentChoice.options.Length)) return false;
            return true;
        }

        public IClassLevel GetClassLevelForCurrentChoices()
        {
            return null;
        }
    }
}