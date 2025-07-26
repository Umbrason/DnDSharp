//A thing a character can do, that manipulates its/others state
using System.Collections;

namespace DnDSharp.Core
{
    public interface IActivity
    {
        Character Owner { get; set; }
        bool CanUse { get; }
        ActivityParameterDescriptor[] Parameters { get; }
        IEnumerator Execute(Dictionary<ActivityParameterDescriptor, object[]> arguments);
    }
}