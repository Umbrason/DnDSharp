namespace DnDSharp.Core
{
    public interface IEnvironment
    {
        public bool CheckLineOfSight(IPosition from, IPosition to);
        public bool FitsAt(Character character, IPosition position);
    }
}