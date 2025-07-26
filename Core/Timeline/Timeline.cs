using System.Collections;

namespace DnDSharp.Core
{

    public class Timeline
    {
        ITimelineEvent[] History;
        ReactableEvent<ITimelineEvent>? OnAnyEvent;
        public ITimelineEvent CurrentEvent { get; }


        
        /*Melee Attack              Timeline                 Hellish Rebuke
                                BeginEventFrame
                                 [MeleeAttack]




                                 EndEventFrame
                                 [MeleeAttack]
        
        
        */
        
        
        
        
        
        
        
        
        
        Stack<EventFrame> EventFrameStack;
        public struct EventFrame {
            
        }

        private IEnumerator<Dictionary<Character, IReaction>> CurrentEventExecutor;
    }
    public interface ITimelineEvent
    {
        IEnumerator<ITimelineEvent> Executor { get; }
    }
}