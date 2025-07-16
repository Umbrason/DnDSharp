using System.Diagnostics;
using System.Reflection;

namespace DnDSharp.Core
{
    public class ClassLevelGenerator
    {
        private static readonly LazyDictionary<ClassID, LazyDictionary<Type, SortedList<int, Type>>> IClassLevelTypes = [];
        public static ClassLevelGenerator Get(ClassID classID, Character character)
        {
            if (IClassLevelTypes[classID].Count == 0) throw new Exception($"Unknown class '{classID}'");
            var lastLevelImplementation = ((LazyDictionary<ClassID, List<IClassLevel>>)character.ClassLevels)[classID];
            var subclassType = lastLevelImplementation?.GetType().DeclaringType;
            subclassType ??= IClassLevelTypes[classID].First((pair) => pair.Key.BaseType == typeof(object)).Key;
            var nextLevelID = character.GetClassLevel(classID) + 1;
            var nextLevelType = IClassLevelTypes[classID][subclassType].GetValueOrDefault(nextLevelID) ??
                                throw new NotImplementedException($"Subclass {subclassType.Name} of class '{classID}' doesn't implement level {nextLevelID}!");
            return new(classID, nextLevelID, nextLevelType);
        }


        static ClassLevelGenerator() => FetchIClassLevelTypes();
        public static void FetchIClassLevelTypes()
        {
            var qualifyingTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(assembly => assembly.GetTypes())
                                .Where(type => type.GetInterfaces().Contains(typeof(IClassLevel))).ToArray();
            foreach (var IClassLevelType in qualifyingTypes)
            {
                var levelAttribute = IClassLevelType.GetBaseTypeImplementingInterface<IClassLevel>()!.GetCustomAttribute<ClassLevelAttribute>();
                if (levelAttribute == null)
                {
                    Trace.TraceWarning($"Level implementation for {IClassLevelType.FullName} cannot be registered! It is missing a ClassLevelAttribute.");
                    continue;
                }
                var level = levelAttribute.LevelID;
                var subclassType = IClassLevelType.DeclaringType;
                if (subclassType == null)
                {
                    Trace.TraceWarning($"Level implementation for {IClassLevelType.FullName} cannot be registered! It is not part of a declaring subclass type.");
                    continue;
                }
                var classID = IClassLevelType.GetFieldOrPropertyValueOrDefault<ClassID>(nameof(IClassLevel.ClassID), BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                classID ??= IClassLevelType.DeclaringType?.GetFieldOrPropertyValueOrDefault<ClassID>(nameof(IClassLevel.ClassID), BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
                if (classID == null)
                {
                    Trace.TraceWarning($"Level implementation for {IClassLevelType.FullName} cannot be registered! It has a ClassID of 'null'.");
                    continue;
                }
                var existing = IClassLevelTypes?.GetValueOrDefault(classID!)?.GetValueOrDefault(subclassType)?.GetValueOrDefault(level);
                if (existing != null && existing != IClassLevelType)
                {
                    Trace.TraceWarning($"Duplicate level implementation for '{subclassType.Name} - {classID}' (subclass - class) found! Tried to register type '{IClassLevelType.FullName}' but type '{existing.FullName}' was already registered.");
                    continue;
                }
                IClassLevelTypes![classID][subclassType][level] = IClassLevelType;
            }
        }

        public ClassLevelGenerator(ClassID classID, int levelID, Type targetCharacterLevelType)
        {
            TargetCharacterLevelType = targetCharacterLevelType;
            this.classID = classID;
            this.levelID = levelID;
            AddChoicesForIClassLevel(targetCharacterLevelType);
        }

        private readonly ClassID classID;
        private readonly int levelID;

        private void AddChoicesForIClassLevel(Type IClassLevelType)
        {
            if (IClassLevelType.IsAbstract)
            {
                var subclassTypes = IClassLevelTypes[classID].Keys.ToArray();
                var levelTypeCandidates = subclassTypes.Select(subclass => IClassLevelTypes[classID][subclass].GetValueOrDefault(levelID)).Where(level => !(level?.IsAbstract ?? true)).ToArray();
                if (levelTypeCandidates.Length == 0) throw new NotImplementedException($"Could not find any subclass implementations for class '{classID.ID}' at level {levelID}, but the level implementation '{IClassLevelType.FullName}' is marked abstract!");
                void onPick(int[] choiceIndices)
                {
                    var subclassType = levelTypeCandidates[choiceIndices[0]];
                    if (subclassType != null) AddChoicesForIClassLevel(subclassType);
                }
                void onDiscard(object? choice)
                {
                    if (choice != null)
                        RemoveSubclassChoices((Type)choice);
                }
                requiredChoices.Add(new ChoiceDescription("Subclass Choice", 1, 1, levelTypeCandidates!, typeof(Type), onPick, onDiscard));
            }
            else
            {
                var ctors = IClassLevelType.GetConstructors();
                if (ctors.Length == 0) throw new Exception($"No constructor found for subclass '{IClassLevelType.FullName}' level {levelID} of class '{classID.ID}'! IClassLevel implementations must have a valid public constructor.");
                if (ctors.Length > 1) throw new Exception($"Multible constructors found for subclass '{IClassLevelType.FullName}' level {levelID} of class '{classID.ID}'! IClassLevel implementations may only have one valid constructor to avoid ambiguity.");
                var ctor = ctors[0];
                foreach (var param in ctor.GetParameters())
                    requiredChoices.Add(ChoiceDescription.FromConstructorParameter(IClassLevelType, param));
            }
        }

        private void RemoveSubclassChoices(Type IClassLevelType)
        {
            if (IClassLevelType.IsAbstract)
                requiredChoices.RemoveAt(requiredChoices.Count - 1);
            else
            {
                var ctors = IClassLevelType.GetConstructors();
                if (ctors.Length == 0) throw new Exception($"No constructor found for subclass '{IClassLevelType.FullName}' level {levelID} of class '{classID.ID}'! IClassLevel implementations must have a valid public constructor.");
                if (ctors.Length > 1) throw new Exception($"Multible constructors found for subclass '{IClassLevelType.FullName}' level {levelID} of class '{classID.ID}'! IClassLevel implementations may only have one valid constructor to avoid ambiguity.");
                var ctor = ctors[0];
                foreach (var _ in ctor.GetParameters())
                    requiredChoices.RemoveAt(requiredChoices.Count - 1);
            }
        }


        private readonly Type TargetCharacterLevelType;
        public ChoiceDescription? CurrentChoice => ChoiceDescriptionIndex >= requiredChoices.Count ? null : requiredChoices[ChoiceDescriptionIndex];
        private readonly List<ChoiceDescription> requiredChoices = [];
        private readonly List<object?> choiceValues = [];
        private int ChoiceDescriptionIndex => choiceValues.Count;


        public class ChoiceDescription
        {
            public string ID;
            public int minPicks;
            public int maxPicks;
            public bool isSinglePick;
            public object?[] options;
            public Type optionType;
            public Action<int[]>? OnPickChoice;
            public Action<object?>? OnDiscardChoice;

            public ChoiceDescription(string iD, int minPicks, int maxPicks, object?[] options, Type optionType, Action<int[]>? onPickChoice, Action<object?>? onDiscardChoice)
            {
                ID = iD;
                this.minPicks = minPicks;
                this.maxPicks = maxPicks;
                this.isSinglePick = minPicks == 1 && maxPicks == 1;
                this.options = options;
                this.optionType = optionType;
                if (maxPicks > options.Length) throw new ArgumentOutOfRangeException($"Maximum number of allowed picks ({maxPicks}) exceeds the amount of options offered ({options.Length}) to pick from.");
                isSinglePick = minPicks == 1 && maxPicks == 1;
                OnPickChoice = onPickChoice;
                OnDiscardChoice = onDiscardChoice;
            }

            public static ChoiceDescription FromConstructorParameter(Type classLevelType, ParameterInfo parameter)
            {
                var attribute = parameter.GetCustomAttribute<ChoicesAttribute>() ?? throw new Exception($"parameter {classLevelType}_ctor.{parameter.Name} is missing a 'Choices' Attribute");
                var paramName = parameter.Name!;
                var paramType = parameter.ParameterType;
                if (!classLevelType.TryGetFieldOrPropertyValue<Array>(attribute.optionsFieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy, out var options))
                    throw new Exception($"Could not find field or property {classLevelType.Name}.{attribute.optionsFieldName}");
                if (options == null)
                    throw new NullReferenceException($"Field or property {classLevelType.Name}.{attribute.optionsFieldName} returned null!");
                var optionType = options.GetType().GetElementType()!;
                options = options.Cast<object?>().ToArray();
                if ((attribute.minPicks != 1 || attribute.maxPicks != 1) && (paramType != optionType.MakeArrayType()))
                    throw new ArrayTypeMismatchException($"Cannot create ChoiceDescription for parameter '{paramName}'. Parameter attribute allows for multiple choices but parameter type '{paramType}' is not an array of type '{optionType}'");
                var isSinglePick = attribute.minPicks == 1 && attribute.maxPicks == 1;
                if (isSinglePick && (paramType != optionType))
                    throw new ArrayTypeMismatchException($"Cannot create ChoiceDescription for parameter '{paramName}'. Parameter attribute expects a single choice but parameter type '{paramType}' doesn't match option type '{optionType}'");
                return new(paramName, attribute.minPicks, attribute.maxPicks, (object?[])options, optionType, null, null);
            }
        }

        public bool IsChoiceValid(int[] choice)
        {
            if (CurrentChoice == null) return false;
            if (choice.Length > CurrentChoice.maxPicks) return false;
            if (choice.Length < CurrentChoice.minPicks) return false;
            if (choice.Distinct().Count() != choice.Length) return false;
            if (choice.Any(i => i < 0 || i >= CurrentChoice.options.Length)) return false;
            return true;
        }

        public bool TryMakeChoice(int[] choiceIndices)
        {
            if (CurrentChoice == null) return false;
            if (!IsChoiceValid(choiceIndices)) return false;

            object? finalChoiceValue;
            if (CurrentChoice.isSinglePick)
                finalChoiceValue = CurrentChoice.options[choiceIndices[0]];
            else
            {
                var choiceValueArray = Array.CreateInstance(CurrentChoice.optionType, choiceIndices.Length);
                for (int i = 0; i < choiceIndices.Length; i++) choiceValueArray.SetValue(CurrentChoice.options[choiceIndices[i]], i);
                finalChoiceValue = choiceValueArray;
            }
            CurrentChoice.OnPickChoice?.Invoke(choiceIndices);
            this.choiceValues.Add(finalChoiceValue);
            return true;
        }

        public bool TryRemoveChoice()
        {
            if (choiceValues.Count == 0) return false;
            var discardedChoice = choiceValues[^1];
            choiceValues.RemoveAt(choiceValues.Count - 1);
            CurrentChoice?.OnDiscardChoice?.Invoke(discardedChoice);
            return true;
        }

        public bool TryGetClassLevelForCurrentChoices(out IClassLevel? levelInstance)
        {
            levelInstance = default;
            if (CurrentChoice != null) return false;
            var type = TargetCharacterLevelType;
            var ctorChoicesStart = 0;
            while (type?.IsAbstract ?? true)
            {
                if (type == null) throw new Exception("Choice Values should contain a valid subclass type, however it did not... How did we end up here?");
                Console.WriteLine(choiceValues[ctorChoicesStart]);
                type = choiceValues[ctorChoicesStart++] as Type;
            }
            var ctorParams = choiceValues[ctorChoicesStart..].ToArray();
            levelInstance = Activator.CreateInstance(type, ctorParams) as IClassLevel;
            if (levelInstance == null) return false;
            return true;
        }
    }
}