using UnityEditor;
using UnityEngine;

namespace DB.CommandConsole
{
    [InitializeOnLoad]
    public class ConsoleConfigEditorHighlighter
    {

        [MenuItem("Command Console/Highlight Config File")]
        private static void HighlightConfig()
        {
            ConsoleConfig config = Resources.Load<ConsoleConfig>("ConsoleConfig");
            if (!config)
            {
                Debug.LogWarning("Couldn't find ConsoleWindow Config file! Make sure there's one called 'ConsoleConfig' in a resources folder.");
                return;
            }

            Selection.SetActiveObjectWithContext(config, config);
        }
    }
}