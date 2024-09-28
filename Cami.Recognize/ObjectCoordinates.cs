namespace Cami.Recognize
{
    public class ObjectCoordinates
    {

        public int PointTopLeftX { get; }
        public int PointTopLeftY { get; }
        public int PointBottomRightX { get; }
        public int PointBottomRightY { get; }

        public ObjectCoordinates(int pointTopLeftX, int pointTopLeftY, int pointBottomRightX, int pointBottomRightY)
        {
            PointTopLeftX = pointTopLeftX;
            PointTopLeftY = pointTopLeftY;
            PointBottomRightX = pointBottomRightX;
            PointBottomRightY = pointBottomRightY;
        }
    }
}