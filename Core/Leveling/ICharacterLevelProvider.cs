using DnDSharp.Core;

public interface ICharacterLevelProvider
{
    public ModifyableValue<int> AvailableCharacterLevels { get; }
}