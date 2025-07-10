using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion : Fighter, IClassLevelProvider
    {

        public new IClassLevel GetClassLevelBuilder(int index)
            => index switch
            {
                3 => new Level3(),
                _ => base.GetClassLevelBuilder(index)
                // on level 3:
            };
    }
}