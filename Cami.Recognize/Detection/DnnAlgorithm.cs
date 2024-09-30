using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace Cami.Recognize.Detection
{
    public class DnnAlgorithm
    {
        private readonly List<(string objectTypeName, Net neuralNetwork)> _objectDetectorConfigurations =
            new List<(string objectTypeName, Net neuralNetwork)>();

        public DnnAlgorithm(List<(string objectTypeName, string prototxt, string caffeModel)> configs)
        {
            foreach (var config in configs)
            {
                _objectDetectorConfigurations.Add(new ValueTuple<string, Net>(config.objectTypeName, CvDnn.ReadNetFromCaffe(config.prototxt, config.caffeModel)));
            }
        }

        public List<DetectedObject> ExtractObjectsCoordinatesFromImage(Stream imageStream, float confidenceThreshold = 0.1f)
        {
            var result = new List<DetectedObject>();

            foreach (var config in _objectDetectorConfigurations)
            {
                var net = config.neuralNetwork;
                imageStream.Seek(0, SeekOrigin.Begin);

                using (var image = Mat.FromStream(imageStream, ImreadModes.AnyColor))
                {
                //    var imageToDelete = Mat.FromStream(File.Open("asd", FileMode.Open), ImreadModes.AnyColor);
                    using (var inputBlob = CvDnn.BlobFromImage(image))
                    {
                        net.SetInput(inputBlob); //, "data");

                        using (var detection = net.Forward())//"detection_out"))
                        {
                            // Assuming detection is a 4D blob
                            var detectionMat = detection.Reshape(1, detection.Size(2)); // Reshape for easier iteration
                            int numDetections = detectionMat.Size(0);

                            for (var i = 0; i < numDetections; i++)
                            {
                                var confidence = detectionMat.At<float>(i, 2);

                                if (confidence >= confidenceThreshold)
                                {
                                    int x1 = (int)(detectionMat.At<float>(i, 3) * image.Width);
                                    int y1 = (int)(detectionMat.At<float>(i, 4) * image.Height);
                                    int x2 = (int)(detectionMat.At<float>(i, 5) * image.Width);
                                    int y2 = (int)(detectionMat.At<float>(i, 6) * image.Height);

                                    // Create a rectangle for the detected object
                                    var detectedObject = new DetectedObject(
                                        config.objectTypeName,
                                        new ObjectCoordinates(x1, y1, x2, y2)
                                    );

                                    result.Add(detectedObject);
                                }
                            }
                        }
                    }
                }
            }
            

            return result;
        }

        
        /*
        public List<DetectedObject> ExtractObjectsCoordinatesFromImage(Stream imageStream)
        {
            var result = new List<DetectedObject> ();
            imageStream.Seek(0, SeekOrigin.Begin);
            using (var image = Mat.FromStream(imageStream, ImreadModes.AnyColor))
            {
                using (var inputBlob = CvDnn.BlobFromImage(image)) //, 1.0, _inputSize, new Scalar(104, 177, 123),
                           //swapRB: false, crop: false))
                {
                    _net.SetInput(inputBlob, "data");

                    using (var detection = _net.Forward("detection_out"))
                    {
                        var objectsRectangles = new List<Rectangle>();
                        
                        detection.

                        for (int i = 0; i < detectionMat.Rows; i++)
                        {
                            float confidence = detectionMat.At<float>(i, 2);

                            if (confidence > _confidenceThreshold)
                            {
                                int x1 = (int)(detectionMat.At<float>(i, 3) * image.Width);
                                int y1 = (int)(detectionMat.At<float>(i, 4) * image.Height);
                                int x2 = (int)(detectionMat.At<float>(i, 5) * image.Width);
                                int y2 = (int)(detectionMat.At<float>(i, 6) * image.Height);

                                faces.Add(new Rectangle(x1, y1, x2 - x1, y2 - y1));
                            }
                        }
                    }
                }
            }

            return result;
        }
        
        public IEnumerable<DetectedObject> ExtractObjectsCoordinatesFromImage(Stream imageStream)
        {
            var inputBlob = Mat.FromStream(imageStream, ImreadModes.Color);// CvDnn.BlobFromImage(image, 1.0, _inputSize, new Scalar(104, 177, 123), swapRB: false, crop: false);
            _net.SetInput(inputBlob, "data");

            using var detection = _net.Forward("detection_out");

            var objects = new List<DetectedObject>();

            for (int i = 0; i < detectionMat.Rows; i++)
            {
                float confidence = detectionMat.At<float>(i, 2);

                if (confidence > _confidenceThreshold)
                {
                    int x1 = (int)(detectionMat.At<float>(i, 3) * image.Width);
                    int y1 = (int)(detectionMat.At<float>(i, 4) * image.Height);
                    int x2 = (int)(detectionMat.At&lt;float>(i, 5) * image.Width);
                    int y2 = (int)(detectionMat.At<float>(i, 6) * image.Height);

                    faces.Add(new Rectangle(x1, y1, x2 - x1, y2 - y1));
                }
            }

            return faces;
        }*/
    }
}
