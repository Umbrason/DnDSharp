namespace DnDSharp.Core
{
    public interface IClass
    {
        public ClassID ClassID { get; }
        public IClassLevel GetClassLevelBuilder(int index);
    }
}