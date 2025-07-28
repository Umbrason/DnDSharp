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
        
/* 
        Fireball    CounterSpellA   CounterSpellB   CounterSpellC
        
        BEGIN
        
                        BEGIN
                                        BEGIN

                                        Negate

                                        END

                                                        BEGIN

                                                        Negate

                                                        END
                        
                        ~N~e~g~a~t~e~
                         was negated

                        END
    
        Roll Damage
        
        Roll Dex Saves

        Deal Damage According to Saves

        END
         */
        
        
        
        
        
        
        
        Stack<EventFrame> EventFrameStack;
        public struct EventFrame {
            IEnumerator<TimelineEvent> Executor;
        }

    }
}