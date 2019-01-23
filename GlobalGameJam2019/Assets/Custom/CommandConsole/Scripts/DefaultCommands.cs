using UnityEngine;

namespace DB.CommandConsole
{
    public class DefaultCommands : MonoBehaviour
    {

        [ConsoleCommand("quit")]
        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_EDITOR
            UnityEngine.Application.Quit();
#endif
        }
    }
}