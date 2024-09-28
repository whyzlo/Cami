using Moq;

namespace Cami.Recognize.Tests
{
    public class HaarCascadeAlgorithmTests
    {
        [Fact]
        public void Constructor_InitializesCascades()
        {
            // Arrange
            var cascades = new List<(string objectTypeName, string trainingDataFile)>
            {
                (It.IsAny<string>(), "Resources/HaarCascade/TrainingData/face.xml")
            };

            // Act
            var algorithm = new HaarCascadeAlgorithm(cascades);

            // Assert
            Assert.NotNull(algorithm);
        }
        
        
        [Fact]
        public void ExtractObjectsCoordinatesFromImage_ReturnsCorrectCoordinates()
        {
            // Arrange
            var cascades = new List<(string objectTypeName, string trainingDataFile)>
            {
                ("face", "Resources/HaarCascade/TrainingData/face.xml"),
                ("body", "Resources/HaarCascade/TrainingData/full_body.xml")
            };

            // Act
            var algorithm = new HaarCascadeAlgorithm(cascades);
            using var stream = File.OpenRead("Resources/HaarCascade/TestedData/people-standing.webp");
            var result = algorithm.ExtractObjectsCoordinatesFromImage(stream);

            // Assert
            Assert.Equal(11, result.Count);
            Assert.Equal(7, result.Count(q => q.ObjectTypeName == "face"));
            Assert.Equal(4, result.Count(q => q.ObjectTypeName == "body"));
        }
    }
}