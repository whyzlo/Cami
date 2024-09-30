using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;

namespace Cami.Recognize.Detection
{
    // TODO: Rename this to CascadeAlgorithm as this is applicable to LBP too
    public class HaarCascadeAlgorithm
    {
        private readonly List<(string objectTypeName, CascadeClassifier cascadeClassifier)> _cascades =
            new List<(string objectTypeName, CascadeClassifier cascadeClassifier)>();

        public HaarCascadeAlgorithm(List<(string objectTypeName, string trainingDataFile)> cascades)
        {
            foreach (var cascade in cascades)
            {
                _cascades.Add(new ValueTuple<string, CascadeClassifier>(cascade.objectTypeName, new CascadeClassifier(cascade.trainingDataFile)));
            }
        }

        public List<DetectedObject> ExtractObjectsCoordinatesFromImage(Stream imageStream)
        {
            var result = new List<DetectedObject> ();
            imageStream.Seek(0, SeekOrigin.Begin);
            using (var mat = Mat.FromStream(imageStream, ImreadModes.AnyColor))
            {
                // TODO: consider running this in parallel. Do not forget and check if there is no problem with the stream which is used due to the use in parallel.
                foreach (var cascade in _cascades)
                {
                    var detectedObjects = cascade.cascadeClassifier.DetectMultiScale(mat);
                    foreach (var detectedObject in detectedObjects)
                    {
                        result.Add(new DetectedObject(
                            cascade.objectTypeName,
                            new ObjectCoordinates(
                                detectedObject.TopLeft.X,
                                detectedObject.TopLeft.Y,
                                detectedObject.BottomRight.X,
                                detectedObject.BottomRight.Y
                                )
                            ));
                    }
                }
            }

            return result;
        }
    }
}
