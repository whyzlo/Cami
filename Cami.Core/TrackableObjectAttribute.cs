using Cami.Core.Interfaces;

namespace Cami.Core
{
    public class TrackableObjectAttribute<T> : ITrackableObjectAttribute
    {
        private T Value { get; set; }
        public string Name { get; set; }

        object ITrackableObjectAttribute.Value
        {
            get => Value;
            set => Value = (T)value;
        }
    }
}