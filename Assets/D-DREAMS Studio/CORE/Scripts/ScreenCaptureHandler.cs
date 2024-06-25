
using UnityEngine;
using System.IO;

namespace DDStudio.CORE
{
    public class ScreenCaptureHandler : MonoBehaviour
    {
        private static Camera _camera;
        private static int _screenWidth;
        private static int _screenHeight;
        private static int _depthBuffer;
        private static string _filePath;
        private static bool _captureScreen;

        private void LateUpdate()
        {
            if (_captureScreen)
            {
                Texture2D screenTexture = CaptureScreen(_screenWidth, _screenHeight);
                SaveScreenshotToFile(screenTexture, _filePath);

                _captureScreen = false;
            }
        }

        public static void SaveScreenshot(Camera camera, string filePath, int screenWidth = 1000, int screenHeight = 1000, int depthBuffer = 24)
        {
            _camera = camera;
            _filePath = filePath;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _depthBuffer = depthBuffer;
            _captureScreen = true;
        }

        private static Texture2D CaptureScreen(int captureWidth, int captureHeight)
        {
            Texture2D screenTexture = new(captureWidth, captureHeight, GetTextureFormat(_depthBuffer), false);

            RenderTexture _renderTexture = new(captureWidth, captureHeight, _depthBuffer)
            {
                antiAliasing = 4
            };

            _camera.targetTexture = _renderTexture;
            _camera.Render();

            RenderTexture.active = _renderTexture;

            screenTexture.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            screenTexture.Apply();

            _camera.targetTexture = null;

            RenderTexture.active = null;

            DestroyImmediate(_renderTexture);

            return screenTexture;
        }

        private static void SaveScreenshotToFile(Texture2D screenTexture, string filePath)
        {
            if (screenTexture == null) return;

            byte[] bytes = screenTexture.EncodeToPNG();

            File.WriteAllBytes(filePath, bytes);

            DestroyImmediate(screenTexture);
        }

        private static TextureFormat GetTextureFormat(int depthBuffer = 24, bool alpha = false)
        {
            TextureFormat textureFormat;

            if (alpha)
            {
                textureFormat = depthBuffer switch
                {
                    64 => TextureFormat.RGBA64,
                    _ => TextureFormat.RGBA32,
                };
            }
            else
            {
                textureFormat = depthBuffer switch
                {
                    48 => TextureFormat.RGB48,
                    _ => TextureFormat.RGB24,
                };
            }

            return textureFormat;
        }
    }
}
