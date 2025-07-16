using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public readonly static ClassID ClassID = Class.Fighter;
        public readonly static ActivityGroupID ActivityGroup = new(nameof(Fighter));
    }
}