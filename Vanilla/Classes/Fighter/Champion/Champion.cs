using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion : Fighter
    {
        public readonly static new ActivityGroupID ActivityGroup = new(nameof(Champion));
    }
}