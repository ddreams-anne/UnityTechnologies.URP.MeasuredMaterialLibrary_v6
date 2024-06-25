using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace DDStudio.CORE
{
    [RequireComponent(typeof(UIDocument))]
    public class ScreenCaptureEventHandler : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private string _fileName = "Screenshot_";

        [SerializeField]
        private int _screenWidth = 1000;

        [SerializeField]
        private int _screenHeight = 1000;

        private Button _screenCaptureButton;

        private void Start()
        {
            if (_camera == null)
            {
                if (Application.isEditor) Debug.LogError("No camera has been selected. The application will quit.");

                ApplicationHandler.QuitApplication();
            }

            var rootElement = gameObject.GetComponent<UIDocument>().rootVisualElement;

            _screenCaptureButton = rootElement.Q<Button>("ScreenCaptureButton");

            if (_screenCaptureButton != null) _screenCaptureButton.clickable.clicked += OnScreenCaptureButtonClicked;
        }

        private void OnDestroy()
        {
            _screenCaptureButton.clickable.clicked -= OnScreenCaptureButtonClicked;
        }

        private void OnScreenCaptureButtonClicked()
        {
            string folderPath = $"{Application.persistentDataPath}/Screenshots";
            string fileName = $"{_fileName}.png";
            string filePath = $"{folderPath}/{fileName}";

            if (!Directory.Exists(folderPath))
            {
                if (Application.isEditor) print($"Creating a new folder at {folderPath}.");

                try
                {
                    Directory.CreateDirectory(folderPath);

                    if (Application.isEditor) print($"Succesfully created a folder at {folderPath}");
                }
                catch (Exception)
                {
                    Debug.LogError($"Failed to created a new folder at {folderPath}");
                }
            }

            ScreenCaptureHandler.SaveScreenshot(_camera, filePath, _screenWidth, _screenHeight);
            ApplicationHandler.QuitApplication();
        }
    }
}
