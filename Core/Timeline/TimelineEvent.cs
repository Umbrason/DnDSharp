public class TimelineEvent
{
    private readonly List<IComponent> m_Components = new();
    public IReadOnlyCollection<IComponent> TimelineEventComponents => m_Components;
    public interface IComponent
    {

    }
}