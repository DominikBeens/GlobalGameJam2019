using UnityEngine;

namespace DB.CommandConsole
{
    public class ConsoleWindowInitialiser
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitConsole()
        {
            ConsoleConfig config = Resources.Load<ConsoleConfig>("ConsoleConfig");
            if (!config)
            {
                Debug.LogWarning("Couldn't find ConsoleWindow Config file! Make sure there's one called 'ConsoleConfig' in a resources folder.");
                return;
            }

            if (!config.autoInitConsoleOnStartGame)
            {
                return;
            }

            // Only execute if there's no consolewindow yet.
            if (!Object.FindObjectOfType<ConsoleWindow>())
            {
                // Find consolewindow in resources.
                GameObject consoleGO = Resources.Load<GameObject>("ConsoleWindow");
                if (consoleGO)
                {
                    // Instantiate consolewindow object and hide in hierarchy.
                    GameObject newConsoleWindow = Object.Instantiate(consoleGO, Vector3.zero, Quaternion.identity);
                    newConsoleWindow.hideFlags = HideFlags.HideInHierarchy;
                }
                else
                {
                    Debug.LogWarning("Couldn't find a ConsoleWindow GameOject in Resources.");
                }
            }
        }
    }
}
