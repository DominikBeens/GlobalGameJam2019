using UnityEngine;

namespace DB.CommandConsole
{
    [CreateAssetMenu(fileName = "ConsoleConfig")]
    public class ConsoleConfig : ScriptableObject
    {

        public const int LOG_POOL_START_COUNT = 50;
        public const int COMMAND_POOL_START_COUNT = 50;

        public const int MAX_COMMAND_REPEATS = 100;

        // Dictionary doesn't save when exiting play-mode.
        //private Dictionary<MessageFilterMode, bool> messageFilterModeState;
        public enum MessageFilterMode { Log, Error, Warning, Command };
        public enum StackTraceFormat { Unity, System_Experimental };

        [Header("Settings")]
        public KeyCode toggleKey = KeyCode.F1;
        public bool autoInitConsoleOnStartGame = true;
        public bool startOpen;
        public StackTraceFormat stackTraceFormat;
        public bool stackTraceLineNumberHighlights = true;
        public bool disableLogsInBuild;

        [Header("KeyCommand Settings")]
        public bool detectInputWhenConsoleIsOpen;

        [Header("Log Message")]
        [SerializeField] private Color logLogColor = Color.white;
        [SerializeField] private Color warningLogColor = Color.yellow;
        [SerializeField] private Color assertLogColor = Color.yellow;
        [SerializeField] private Color errorLogColor = Color.red;
        [SerializeField] private Color exceptionLogColor = Color.red;

        [Space(5)]

        public Color stackTraceHighlightColor = Color.red;

        [Header("Command Message")]
        public Color commandTextColor = Color.white;
        public Color commandTriggerColor = Color.green;
        public Color commandObjectCountColor = Color.blue;
        public Color commandRunCountColor = Color.red;

        public Color GetLogColor(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    return errorLogColor;

                case LogType.Assert:
                    return assertLogColor;

                case LogType.Warning:
                    return warningLogColor;

                case LogType.Log:
                    return logLogColor;

                case LogType.Exception:
                    return exceptionLogColor;

                default:
                    return Color.white;
            }
        }

        public void SetFilterState(MessageFilterMode filterMode, bool toggle)
        {
            switch (filterMode)
            {
                case MessageFilterMode.Log:
                    PlayerPrefs.SetInt("LogFilterModeState", toggle ? 1 : 0);
                    break;

                case MessageFilterMode.Error:
                    PlayerPrefs.SetInt("ErrorFilterModeState", toggle ? 1 : 0);
                    break;

                case MessageFilterMode.Warning:
                    PlayerPrefs.SetInt("WarningFilterModeState", toggle ? 1 : 0);
                    break;

                case MessageFilterMode.Command:
                    PlayerPrefs.SetInt("CommandFilterModeState", toggle ? 1 : 0);
                    break;
            }
        }

        public bool GetFilterState(MessageFilterMode filterMode)
        {
            switch (filterMode)
            {
                case MessageFilterMode.Log:
                    return PlayerPrefs.HasKey("LogFilterModeState") ? (PlayerPrefs.GetInt("LogFilterModeState") == 1 ? true : false) : true;

                case MessageFilterMode.Error:
                    return PlayerPrefs.HasKey("ErrorFilterModeState") ? (PlayerPrefs.GetInt("ErrorFilterModeState") == 1 ? true : false) : true;

                case MessageFilterMode.Warning:
                    return PlayerPrefs.HasKey("WarningFilterModeState") ? (PlayerPrefs.GetInt("WarningFilterModeState") == 1 ? true : false) : true;

                case MessageFilterMode.Command:
                    return PlayerPrefs.HasKey("CommandFilterModeState") ? (PlayerPrefs.GetInt("CommandFilterModeState") == 1 ? true : false) : true;

                default:
                    return true;
            }
        }

        public void SetCollapseState(bool toggle)
        {
            PlayerPrefs.SetInt("CollapseState", toggle ? 1 : 0);
        }

        public bool GetCollapseState()
        {
            return PlayerPrefs.HasKey("CollapseState") ? (PlayerPrefs.GetInt("CollapseState") == 1 ? true : false) : false;
        }
    }
}