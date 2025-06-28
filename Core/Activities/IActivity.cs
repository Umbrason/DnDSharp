//A thing a character can do, that manipulates its/others state
namespace DnDSharp.Core
{
    public interface IActivity
    {
        Character Owner { get; set; }
        bool CanUse { get; }
        ActivityParameterDescriptor[] Parameters { get; }
        void DoUse(Dictionary<ActivityParameterDescriptor, object[]> arguments);
    }
}