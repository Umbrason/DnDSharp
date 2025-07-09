using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion : Fighter, IClass
    {

        public new IClassLevel? GetClassLevelBuilder(int index)
            => index switch
            {
                0 => null,
                _ => base.GetClassLevelBuilder(index)
                // on level 3:
            };
    }
}