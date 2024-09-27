using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;

namespace Cami.Photo
{
    public class PhotoRecorder : IPhotoRecorder
    {
        public EventHandler<PhotoRecordedEventArgs> OnPhotoCreated { get; set; }

        public async Task StartRecording(string sourceAddress, CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                var dataStream = await GetFirstFrameAsStreamAsync(sourceAddress);
                OnPhotoCreated?.Invoke(this, new PhotoRecordedEventArgs(dataStream));
            }
        }

        private static async Task<Stream> GetFirstFrameAsStreamAsync(string streamUrl)
        {
            // Use HttpClient to read the MJPEG stream
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(streamUrl, HttpCompletionOption.ResponseHeadersRead))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new BinaryReader(stream))
            {
                var boundaryMarker = "--"; // MJPEG streams typically use "--" as a boundary marker
                var contentType = response.Content.Headers.ContentType.ToString();

                // Ensure the content type is multipart/x-mixed-replace (MJPEG)
                if (!contentType.StartsWith("multipart"))
                    throw new Exception("Invalid stream format. Expected MJPEG stream.");

                while (true)
                {
                    // Look for the boundary marker
                    string boundary = ReadLine(reader);
                    if (boundary.StartsWith(boundaryMarker))
                    {
                        // Parse headers to find the Content-Length or skip to the image data
                        var contentLength = -1;
                        while (true)
                        {
                            string header = ReadLine(reader);
                            if (header.StartsWith("Content-Length:"))
                                contentLength = int.Parse(header.Replace("Content-Length:", "").Trim());

                            if (string.IsNullOrWhiteSpace(header)) break; // End of headers, next is image data
                        }

                        if (contentLength > 0)
                        {
                            // Now read exactly `contentLength` bytes to ensure we capture the entire frame
                            var frameStream = new MemoryStream();
                            var buffer = new byte[4096];
                            var totalBytesRead = 0;

                            while (totalBytesRead < contentLength)
                            {
                                var bytesToRead = Math.Min(buffer.Length, contentLength - totalBytesRead);
                                var bytesRead = await reader.BaseStream.ReadAsync(buffer, 0, bytesToRead);
                                if (bytesRead == 0)
                                    // End of stream or network issue
                                    break;

                                totalBytesRead += bytesRead;
                                frameStream.Write(buffer, 0, bytesRead);
                            }

                            if (frameStream.Length > 0) return frameStream;
                        }
                    }
                }
            }

            // If no frame was captured or there's an issue, return null
            return null;
        }
        
        // Utility method to read a line from the stream (binary reader)
        private static string ReadLine(BinaryReader reader)
        {
            List<byte> lineBuffer = new List<byte>();
            char previousChar = '\0';

            while (true)
            {
                char currentChar = reader.ReadChar();
                lineBuffer.Add((byte)currentChar);

                // Detect newline (CRLF or LF)
                if (currentChar == '\n' && previousChar == '\r')
                {
                    break;
                }
                previousChar = currentChar;
            }

            return System.Text.Encoding.ASCII.GetString(lineBuffer.ToArray()).Trim();
        }


        /*
        private static async Task SaveFirstFrame(string streamUrl, string filePath)
        {
            // Use HttpClient to read the MJPEG stream
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(streamUrl, HttpCompletionOption.ResponseHeadersRead))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new BinaryReader(stream))
            {
                var boundaryMarker = "--"; // MJPEG streams typically use "--" as a boundary marker
                var contentType = response.Content.Headers.ContentType.ToString();

                // Ensure the content type is multipart/x-mixed-replace (MJPEG)
                if (!contentType.StartsWith("multipart"))
                {
                    return;
                    throw new Exception("Invalid stream format. Expected MJPEG stream.");
                }

                while (true)
                {
                    // Look for the boundary marker
                    string boundary = ReadLine(reader);
                    if (boundary.StartsWith(boundaryMarker))
                    {
                        // Parse headers to find the Content-Length or skip to the image data
                        var contentLength = -1;
                        while (true)
                        {
                            string header = ReadLine(reader);
                            if (header.StartsWith("Content-Length:"))
                                contentLength = int.Parse(header.Replace("Content-Length:", "").Trim());

                            if (string.IsNullOrWhiteSpace(header)) break; // End of headers, next is image data
                        }

                        if (contentLength > 0)
                        {
                            // Now read exactly `contentLength` bytes to ensure we capture the entire frame
                            var frameStream = new MemoryStream();
                            var buffer = new byte[4096];
                            var totalBytesRead = 0;

                            while (totalBytesRead < contentLength)
                            {
                                var bytesToRead = Math.Min(buffer.Length, contentLength - totalBytesRead);
                                var bytesRead = await reader.BaseStream.ReadAsync(buffer, 0, bytesToRead);
                                if (bytesRead == 0)
                                    // End of stream or network issue
                                    break;

                                totalBytesRead += bytesRead;
                                frameStream.Write(buffer, 0, bytesRead);
                            }

                            // Now save the complete frame
                            if (frameStream.Length > 0)
                            {
                                await SaveMemoryStreamToFileAsync(frameStream, filePath);
                                break; // Stop after saving one frame
                            }
                        }
                    }
                }
            }
        }

        // Utility method to read a line from the stream (binary reader)
        private static string ReadLine(BinaryReader reader)
        {
            List<byte> lineBuffer = new List<byte>();
            char previousChar = '\0';

            while (true)
            {
                char currentChar = reader.ReadChar();
                lineBuffer.Add((byte)currentChar);

                // Detect newline (CRLF or LF)
                if (currentChar == '\n' && previousChar == '\r')
                {
                    break;
                }
                previousChar = currentChar;
            }

            return System.Text.Encoding.ASCII.GetString(lineBuffer.ToArray()).Trim();
        }

        // Method to save a memory stream containing image data as a JPEG using SkiaSharp
        private static async Task SaveMemoryStreamToFileAsync(MemoryStream frameStream, string filePath)
        {
            // Ensure the memory stream is at the beginning
            frameStream.Seek(0, SeekOrigin.Begin);

            // Load the MemoryStream as a SkiaSharp bitmap
            using (SKBitmap bitmap = SKBitmap.Decode(frameStream))
            {
                if (bitmap != null)
                {
                    // Create an SKImage from the bitmap
                    using (SKImage image = SKImage.FromBitmap(bitmap))
                    {
                        // Encode the image to JPEG format
                        using (SKData encoded = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                        {
                            // Write the JPEG data asynchronously to the specified file
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                            {
                                // Asynchronously write the SKData to the file
                                await fileStream.WriteAsync(encoded.ToArray(), 0, (int)encoded.Size);
                            }

                            Console.WriteLine("Frame saved to: " + filePath);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed to decode bitmap from stream.");
                }
            }
        } */
    }
}