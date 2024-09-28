namespace Cami.Recognize
{
    public class DetectedObject
    {
        public string ObjectTypeName { get; }
        public ObjectCoordinates Coordinates { get; }

        public DetectedObject(string objectTypeName, ObjectCoordinates coordinates)
        {
            ObjectTypeName = objectTypeName;
            Coordinates = coordinates;
        }
    }
}