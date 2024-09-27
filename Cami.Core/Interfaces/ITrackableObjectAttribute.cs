namespace Cami.Core.Interfaces
{
    public interface ITrackableObjectAttribute
    {
        string Name { get; set; }
        object Value { get; set; }
    }
}