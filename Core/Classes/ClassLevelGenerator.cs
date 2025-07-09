using System.Diagnostics.CodeAnalysis;
using DnDSharp.Core;

/*

Nice to have:
just write a ctor in IClassLevel
ctor gets parsed via reflections to create the ClassLevelGenerator
can ctor params have attributes? HOLY SHIT YES THEY CAN!

ok this is the plan:
0. always needs a Character to reference to check for existing classes
1. determine available subclasses if is abstract IClassLevel
2. read option metadata from ctor parameter attributes (i.e. [Options(Wizard.Spelllist(1).Concat(Wizard.Spelllist(2)))]IActivity[] spells) 
*/
public class ClassLevelGenerator<T>
{
    public ClassLevelGenerator([NotNull()]int value)
    {
        if (typeof(T).IsAbstract)
        {
            //is subclass
        }
    }

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

    public IClassLevel Create()
    {
        return null;
    }
}