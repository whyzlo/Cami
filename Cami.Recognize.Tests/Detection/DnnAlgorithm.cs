using Cami.Recognize.Detection;
using Moq;

namespace Cami.Recognize.Tests.Detection
{
    public class DnnAlgorithmTests
    {
        [Fact]
        public void Constructor_InitializesCascades()
        {
            // Arrange
            var cascades = new List<(string objectTypeName, string prototxt, string cafeeModel)>
            {
                (It.IsAny<string>(),
                    "Resources/Dnn/TrainingData/deploy.prototxt",
                    "Resources/Dnn/TrainingData/res10_300x300_ssd_iter_140000.caffemodel"
                    )
            };

            // Act
            var algorithm = new DnnAlgorithm(cascades);

            // Assert
            Assert.NotNull(algorithm);
        }
        
        
        [Fact]
        public void ExtractObjectsCoordinatesFromImage_ReturnsCorrectCoordinates()
        {
            // Arrange
            var cascades = new List<(string objectTypeName, string protoTxt, string cafeeModel)>
            {
                //("face", "Resources/Dnn/TrainingData/deploy.prototxt", "Resources/Dnn/TrainingData/res10_300x300_ssd_iter_140000.caffemodel"),
                //("face", "Resources/Dnn/TrainingData/fullface_deploy.prototxt", "Resources/Dnn/TrainingData/fullfacedetection.caffemodel"),
                ("face", "Resources/Dnn/TrainingData/weights-prototxt.txt", "Resources/Dnn/TrainingData/res_ssd_300Dim.caffeModel"),
            };

            // Act
            var algorithm = new DnnAlgorithm(cascades);
            using var stream = File.OpenRead("Resources/Dnn/TestedData/faces.jpg");
            var result = algorithm.ExtractObjectsCoordinatesFromImage(stream);

            // Assert
            Assert.Equal(123, result.Count);
          /*  Assert.Equal(7, result.Count(q => q.ObjectTypeName == "face"));
            Assert.Equal(4, result.Count(q => q.ObjectTypeName == "body")); */



            // TODO: remove later (now it is for testing purposes)
            
            var marker = new OpenCvObjectMarker();
            var markedStream = marker.MarkRectanglesInImageStream(stream, result.Where(q => q.ObjectTypeName == "face").Select(q => q.Coordinates).ToList(), 2, OpenCvObjectMarker.Colors.Red);
            markedStream = marker.MarkRectanglesInImageStream(markedStream, result.Where(q => q.ObjectTypeName == "body").Select(q => q.Coordinates).ToList(), 2, OpenCvObjectMarker.Colors.Green);
            using var fileStream = File.Create("vavava.jpg");
            markedStream.Seek(0, SeekOrigin.Begin);
            markedStream.CopyTo(fileStream);
            
        }
    }
}