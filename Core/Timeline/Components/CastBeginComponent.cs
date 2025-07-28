using DnDSharp.Core;

namespace DnDSharp.Core
{
    public class CastBeginComponent : TimelineEvent.IComponent
    {
        public bool IsCanceled;
        IActivity TargetActivity;
    }
}