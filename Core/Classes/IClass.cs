namespace DnDSharp.Core
{
    public interface IClassLevelProvider
    {
        public ClassID ClassID { get; }
        public IClassLevel GetClassLevelBuilder(int index);
    }
}