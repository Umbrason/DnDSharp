namespace DnDSharp.Core
{
    public interface IClassLevel
    {
        ClassID classID { get; }
        void OnAdded(Character character);
        void OnRemoved(Character character);
    }
}