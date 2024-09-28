using System.Collections.Generic;
using System.IO;
using OpenCvSharp;

namespace Cami.Recognize
{
    public class OpenCvObjectMarker
    {
        public static class Colors
        {
            public static readonly (int R, int G, int B) Red = (255, 0, 0);
            public static readonly (int R, int G, int B) Green = (0, 255, 0);
            public static readonly (int R, int G, int B) Blue = (0, 0, 255);
            public static readonly (int R, int G, int B) Yellow = (255, 255, 0);
            public static readonly (int R, int G, int B) White = (255, 255, 255);
            public static readonly (int R, int G, int B) Black = (0, 0, 0);
        }

        public Stream MarkRectanglesInImageStream(Stream imageStream, List<ObjectCoordinates> objectCoordinatesList, int thickness, (int R, int G, int B) color)
        {
            var sourceImage = Mat.FromStream(imageStream, ImreadModes.AnyColor);

            foreach (var objectCoordinates in objectCoordinatesList)
            {
                Cv2.Rectangle(sourceImage, CreateRectFromObjectCoordinates(objectCoordinates), new Scalar(color.B, color.G, color.R), thickness);
            }

            return sourceImage.ToMemoryStream();
        }

        private static Rect CreateRectFromObjectCoordinates(ObjectCoordinates coordinates)
        {
            return new Rect(
                X: coordinates.PointTopLeftX,
                Y: coordinates.PointTopLeftY,
                Width: coordinates.PointBottomRightX - coordinates.PointTopLeftX,
                Height: coordinates.PointBottomRightY - coordinates.PointTopLeftY
            );
        }
    }
}