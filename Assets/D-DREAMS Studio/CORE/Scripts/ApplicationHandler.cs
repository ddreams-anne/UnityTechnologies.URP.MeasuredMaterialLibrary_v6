using UnityEngine;
using UnityEditor;

namespace DDStudio.CORE
{
    public class ApplicationHandler : MonoBehaviour
    {
        public static void QuitApplication()
        {
            if (Application.isEditor) EditorApplication.ExitPlaymode();
            else Application.Quit();
        }
    }
}
