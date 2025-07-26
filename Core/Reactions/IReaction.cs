namespace DnDSharp.Core
{
    public interface IReaction
    {
        public bool CanTrigger(Character character, ITimelineEvent @event);
        public void Trigger(Character character, ITimelineEvent @event);
        public ReactionTriggerBehaviour TriggerBehaviour { get; }
    }
}