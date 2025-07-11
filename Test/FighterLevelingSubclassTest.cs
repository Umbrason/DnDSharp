using DnDSharp.Core;
using DnDSharp.Vanilla;

namespace DnDSharp.Test
{
    public static class FighterLevelingSubclassTest
    {
        public static void Test()
        {
            var character = new DnDCharacter(new Dictionary<AbilityID, int>() {
            {Ability.CHARISMA, 10},
            {Ability.CONSTITUTION, 10},
            {Ability.DEXTERITY, 10},
            {Ability.INTELLIGENCE, 10},
            {Ability.STRENGTH, 10},
            {Ability.WISDOM, 10},
        }, 0, new Dictionary<DamageElementID, int>(), 10, new GroupID("Neutral"));

            var rndm = new Random();

            const int levels = 3;
            for (int i = 0; i < levels; i++)
            {
                var generator = ClassLevelGenerator.Get(Class.Fighter, character);
                while (generator.CurrentChoice != null)
                {
                    var indexOptions = Enumerable.Range(0, generator.CurrentChoice.options.Length).ToList();
                    var pickCount = rndm.Next(generator.CurrentChoice.minPicks, generator.CurrentChoice.maxPicks);
                    var picks = Enumerable.Range(0, pickCount).Select(i =>
                    {
                        var opt = rndm.Next(0, indexOptions.Count);
                        var index = indexOptions[opt];
                        indexOptions.RemoveAt(opt);
                        return index;
                    }).ToArray();
                    if (!generator.TryMakeChoice(picks)) throw new Exception("something went wrong trying to make a choice");
                }
                if (generator.TryGetClassLevelForCurrentChoices(out var level))
                    character.AddClassLevel(level!);
                else throw new Exception($"Failed to create level {i} for Fighter");
            }
            var fighterLevels = character.ClassLevels[Class.Fighter];
            Console.WriteLine($"[{string.Join(", ", fighterLevels.Select(level => level.GetType().FullName))}]");
        }
    }
}