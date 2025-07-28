using System.Collections;

namespace DnDSharp.Core
{
    public class ReactableEvent<T> where T : ITimelineEvent
    {
        private readonly Dictionary<Character, HashSet<IReaction>> m_reactions = [];

        bool invoking = false;
        public IEnumerator Invoke(T context)
        {
            invoking = true;
            Dictionary<Character, Dictionary<IReaction, bool>> reactionTriggerDecisions = [];
            foreach (var (character, reactions) in m_reactions)
            {
                reactionTriggerDecisions[character] = [];
                foreach (var r in reactions)
                {
                    bool canTrigger = r.CanTrigger(character, context);
                    if (!canTrigger)
                    {
                        reactionTriggerDecisions[character][r] = false;
                        continue;
                    }
                    switch (r.TriggerBehaviour)
                    {
                        case ReactionTriggerBehaviour.Always:
                            reactionTriggerDecisions[character][r] = true;
                            break;
                        case ReactionTriggerBehaviour.Never:
                            reactionTriggerDecisions[character][r] = false;
                            break;
                    }
                }
            }

            bool AllReactionDecisionsMade(KeyValuePair<Character, Dictionary<IReaction, bool>> pair)
            {
                var (character, decisions) = (pair.Key, pair.Value);
                return decisions.Count < m_reactions[character].Count;
            }
            while (!reactionTriggerDecisions.All(AllReactionDecisionsMade))
                yield return null;

            foreach (var (character, reactionTriggerDecision) in reactionTriggerDecisions)
            {
                foreach (var (r, shouldTrigger) in reactionTriggerDecision)
                {
                    if (!shouldTrigger) continue;
                    r.Trigger(character, context);
                }
            }
            invoking = false;
        }

        public static ReactableEvent<T> operator +(ReactableEvent<T> @event, (Character character, IReaction reaction) pair)
        {
            if (@event.invoking) throw new Exception("Cannot register new IReaction to event while event is being invoked.");
            @event.m_reactions[pair.character].Add(pair.reaction);
            return @event;
        }

        public static ReactableEvent<T> operator -(ReactableEvent<T> @event, (Character character, IReaction reaction) pair)
        {
            if (@event.invoking) throw new Exception("Cannot unregister IReaction from event while event is being invoked.");
            @event.m_reactions[pair.character].Remove(pair.reaction);
            return @event;
        }
    }

}