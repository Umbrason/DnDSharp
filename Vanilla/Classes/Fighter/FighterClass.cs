using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter : IClass
    {
        public static ClassID ClassID => Class.Fighter;
        public readonly static ActivityGroupID ActivityGroup = new(nameof(Fighter));
        public IClassLevel GetClassLevel(int index)
        => index switch
        {
            1 => new Level1(),
            2 => new Level2(),
            _ => throw new NotImplementedException($"{ClassID.ID} does not implement level {index}")
        };
    }
}