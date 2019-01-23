using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DB.CommandConsole
{
    public class ConsoleWindow : MonoBehaviour
    {

        private ConsoleConfig config;
        public ConsoleConfig Config { get { return config; } }

        public bool IsOpen { get { return windowCanvas.enabled; } }

        private List<ConsoleMessage> messages = new List<ConsoleMessage>();
        private List<Command> commands = new List<Command>();
        private List<string> commandHistory = new List<string>();
        private int historyIndex;

        [SerializeField] private Canvas windowCanvas;

        [Space]

        [SerializeField] private ScrollRect contentScrollView;
        [SerializeField] private TMP_InputField commandInputField;

        [Header("Log Info")]
        [SerializeField] private Canvas logInfoCanvas;
        [SerializeField] private TextMeshProUGUI logConditionText;
        [SerializeField] private TextMeshProUGUI logStackTraceText;
        [SerializeField] private ScrollRect logInfoScrollView;

        [Space]

        [SerializeField] private ConsolePool pool;

        public struct LogData
        {
            public string condition;
            public string stackTrace;
            public LogType logType;
            public TimeSpan logTime;
            public Color textColor;
        }

        public struct Command
        {
            public string commandTrigger;
            public MethodInfo commandMethod;
            public object[] defaultParameters;
        }

        private void Awake()
        {
            config = Resources.Load<ConsoleConfig>("ConsoleConfig");
            if (!config)
            {
                Debug.LogError("Couldn't find ConsoleWindow Config file! Make sure there's one called 'ConsoleConfig' in a resources folder.");
                gameObject.SetActive(false);
                return;
            }

            ToggleConsoleWindow(config.startOpen);
            pool.Init();
            HideLogInfo();

#if UNITY_EDITOR
            Application.logMessageReceived += Log;
#elif !UNITY_EDITOR
            if (!config.disableLogsInBuild)
            {
                Application.logMessageReceived += Log;
            }
#endif

            FindAllCommands();
            commandHistory.Insert(0, "");
        }

        private void Update()
        {
            if (Input.GetKeyDown(config.toggleKey))
            {
                ToggleConsoleWindow(!windowCanvas.enabled);
            }

            if (windowCanvas.enabled)
            {
                HandleCommandInputFieldNavigation();
            }
        }

        private void Log(string condition, string stackTrace, LogType type)
        {
            ConsoleLogMessage newMessage = (ConsoleLogMessage)pool.GetNewMessage(ConsolePool.PoolType.Log);

            string trace = stackTrace;
            if (config.stackTraceFormat == ConsoleConfig.StackTraceFormat.System_Experimental)
            {
                // Manually get stacktrace, Unity hides the stacktrace in normal builds.
                System.Diagnostics.StackTrace systemStackTrace = new System.Diagnostics.StackTrace(true);
                trace = systemStackTrace.ToString();

                trace = trace.Remove(0, 2);
                trace = trace.Replace("at ", "<b>at</b> ");

                // Remove hex values
                int start = trace.IndexOf("[0x");
                while (start > 0)
                {
                    trace = trace.Remove(start, 10);
                    start = trace.IndexOf("[0x");
                }
            }

            if (config.stackTraceLineNumberHighlights)
            {
                trace = trace.Replace(")\n", "</color>)\n");
                trace = trace.Replace(".cs:", $".cs:<color=#{ColorUtility.ToHtmlStringRGB(config.stackTraceHighlightColor)}>");
            }

            LogData newLogData = CreateLogData(condition, trace, type);
            newMessage.Init(newLogData);

            messages.Add(newMessage);

            newMessage.gameObject.SetActive(FilterMessage(newMessage));
            TryCollapseMessage(newMessage);

            StartCoroutine(UpdateWindowScrollPositions());
        }

        //[KeyCommand(KeyCode.G, PressType.KeyPressType.Down)]
        //public void TestError()
        //{
        //    Transform t = null;
        //    t.position = new Vector3(-413, 125123, 51234);
        //    //Debug.Log(commands[500]);
        //}

        private LogData CreateLogData(string condition, string stackTrace, LogType type)
        {
            return new LogData
            {
                condition = condition,
                stackTrace = stackTrace,
                logType = type,
                logTime = TimeSpan.FromSeconds(Time.time),
                textColor = config.GetLogColor(type)
            };
        }

        public static string GetTimeString(TimeSpan time)
        {
            return $"{(time.Hours).ToString("00")}" +
                   $":" +
                   $"{(time.Minutes).ToString("00")}" +
                   $":" +
                   $"{(time.Seconds).ToString("00")}";
        }

        private static TimeSpan GetCurrentTime()
        {
            return TimeSpan.FromSeconds(Time.time);
        }

        private static string ColorToString(Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }

        private IEnumerator UpdateWindowScrollPositions()
        {
            // Scrollviews need to be scrolled down when a new item gets added.
            // The ContentSizeFitter presumably resizes its transform after the canvas gets redrawn
            // which happens pretty late I assume because without waiting a frame the scrollview will get
            // scrolled down before the CSF resizes its transform resulting in the scrollview scrolling the
            // second to last item.

            yield return new WaitForEndOfFrame();
            contentScrollView.normalizedPosition = new Vector2(0, 0);
        }

        public void FilterMessages()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                messages[i].ResetCollapseCount();
                messages[i].gameObject.SetActive(FilterMessage(messages[i]));
                //TryCollapseMessage(messages[i]);
            }

            if (config.GetCollapseState())
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    if (!messages[i].gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    ConsoleMessage collapsedParent = null;
                    for (int ii = 0; ii < messages.Count; ii++)
                    {
                        if (!messages[ii].gameObject.activeInHierarchy)
                        {
                            continue;
                        }

                        // Check if message is the same and from the same source.
                        if (messages[i].IsTheSameAs(messages[ii]) && messages[i].IsFromTheSameSourceAs(messages[ii]))
                        {
                            // We need to leave one message active and collapse all other messages into it.
                            // Check if we already have a 'god message'.
                            if (!collapsedParent)
                            {
                                collapsedParent = messages[ii];
                            }
                            else
                            {
                                collapsedParent.IncrementCollapseCount();
                                messages[ii].gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }

            StartCoroutine(UpdateWindowScrollPositions());
        }

        private bool FilterMessage(ConsoleMessage message)
        {
            bool toggle = false;

            switch (message.GetMessageType())
            {
                case ConsoleMessage.MessageType.Log:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Log);
                    break;

                case ConsoleMessage.MessageType.Error:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Error);
                    break;

                case ConsoleMessage.MessageType.Exception:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Error);
                    break;

                case ConsoleMessage.MessageType.Warning:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Warning);
                    break;

                case ConsoleMessage.MessageType.Assert:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Warning);
                    break;

                case ConsoleMessage.MessageType.Command:
                    toggle = config.GetFilterState(ConsoleConfig.MessageFilterMode.Command);
                    break;
            }

            return toggle;
        }

        private void TryCollapseMessage(ConsoleMessage message)
        {
            if (!message.gameObject.activeInHierarchy || !config.GetCollapseState())
            {
                return;
            }

            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i] == message)
                {
                    continue;
                }

                if (messages[i].IsTheSameAs(message) && messages[i].IsFromTheSameSourceAs(message))
                {
                    messages[i].IncrementCollapseCount();
                    message.gameObject.SetActive(false);
                    return;
                }
            }
        }

        public void ClearMessageLog()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                ConsolePool.logPool.Enqueue(messages[i].gameObject);
                messages[i].gameObject.SetActive(false);
            }

            messages.Clear();
        }

        public void ToggleConsoleWindow(bool toggle)
        {
            windowCanvas.enabled = toggle;
            HideLogInfo();

            if (toggle)
            {
                StartCoroutine(UpdateWindowScrollPositions());
            }
        }

        private void FindAllCommands()
        {
            // Find all MonoBehaviours.
            MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                // Find all methods.
                MethodInfo[] methodInfos = behaviours[i].GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                // Find ConsoleCommand attribute.
                for (int ii = 0; ii < methodInfos.Length; ii++)
                {
                    if (Attribute.GetCustomAttribute(methodInfos[ii], typeof(ConsoleCommandAttribute)) is ConsoleCommandAttribute attribute)
                    {
                        Command newCommand = new Command { commandTrigger = attribute.CommandName, commandMethod = methodInfos[ii], defaultParameters = attribute.DefaultParameters };
                        if (!commands.Contains(newCommand))
                        {
                            commands.Add(newCommand);
                        }
                    }
                }
            }
        }

        public void ExecuteCommand(string commandOverride)
        {
            // Get the full command based on if we used a parameter with this function.
            // An input field doesn't use this parameter.
            string fullInput = "";
            fullInput = string.IsNullOrEmpty(commandOverride) || string.IsNullOrWhiteSpace(commandOverride) ? commandInputField.text : commandOverride;

            // Check if the command doesnt only consist of spaces or is empty.
            if (string.IsNullOrEmpty(fullInput) || string.IsNullOrWhiteSpace(fullInput))
            {
                commandInputField.text = "";
                return;
            }

            commandHistory.Insert(1, fullInput);
            historyIndex = 0;

            string[] fullSplitInput = fullInput.Split(' ');
            string commandInput = fullSplitInput[0];

            // User input command data.
            int timesToRun = 1;
            bool containsColonCommand = false;
            GetTimesToRunFromCommand(fullSplitInput, ref timesToRun, ref containsColonCommand);
            int userInputParamCount = GetParameterCount(fullSplitInput, containsColonCommand);

            Command[] toExecute = commands.Where(c => c.commandTrigger == commandInput).ToArray();
            for (int i = 0; i < toExecute.Length; i++)
            {
                // Get the type of the class that holds the 'command method'.
                Type declaringType = toExecute[i].commandMethod.DeclaringType;
                // Find all objects of that type.
                UnityEngine.Object[] objects = FindObjectsOfType(declaringType);

                // Get parameters and check if the input command contains the same amount of parameters.
                // If not then break out of the loop and don't execute anything.
                ParameterInfo[] parameters = toExecute[i].commandMethod.GetParameters();

                if (parameters.Length != userInputParamCount && toExecute[i].defaultParameters == null)
                {
                    ShowCommandInvalidParamCountMessage(fullInput, commandInput, parameters.Length);
                    return;
                }

                // Loop through all of those objects and invoke their 'command methods'.
                for (int ii = 0; ii < objects.Length; ii++)
                {
                    for (int iii = 0; iii < timesToRun; iii++)
                    {
                        // No parameters and we didn't input any parameters, invoke method.
                        if (parameters.Length == 0 && userInputParamCount == 0)
                        {
                            toExecute[i].commandMethod.Invoke(objects[ii], null);
                        }
                        // Method has parameters but we didnt input any but we did in fact specify default parameters so try to use those.
                        else if (parameters.Length > 0 && userInputParamCount == 0 && toExecute[i].defaultParameters.Length == parameters.Length)
                        {
                            toExecute[i].commandMethod.Invoke(objects[ii], toExecute[i].defaultParameters);
                        }
                        else
                        {
                            // Method has parameters and we input the correct amount.
                            // Or.
                            // Method has parameters but we didn't input the correct amount but we did in fact specify default parameters. Try to fill in the missing params from the default params.
                            if ((parameters.Length > 0 && parameters.Length == userInputParamCount) ||
                                (parameters.Length > 0 && parameters.Length != userInputParamCount && toExecute[i].defaultParameters.Length == parameters.Length))
                            {
                                object[] convertedParams = GetConvertedParameters(parameters, fullSplitInput, fullInput, commandInput, toExecute[i].defaultParameters);
                                if (convertedParams.Length != parameters.Length)
                                {
                                    return;
                                }
                                toExecute[i].commandMethod.Invoke(objects[ii], convertedParams);
                            }
                            else
                            {
                                ShowFailedToDetectValidParamsMessage(fullInput, commandInput);
                                return;
                            }
                        }
                    }
                }

                // Command executed.
                ShowCommandExecutedMessage(fullInput, toExecute[i].commandTrigger, objects.Length, timesToRun);
            }

            // Command not found.
            if (toExecute.Length == 0)
            {
                ShowCommandNotFoundMessage(fullInput, commandInput);
            }
        }

        // Try to convert all parameters in our input to the parameters required to invoke the method.
        // If we fail at converting, abort.
        private object[] GetConvertedParameters(ParameterInfo[] commandParameterTypes, string[] fullSplitInput, string fullInput, string commandInput, object[] defaultParameters = null)
        {
            List<object> convertedParams = new List<object>();

            for (int i = 0; i < commandParameterTypes.Length; i++)
            {
                try
                {
                    object parameter = null;

                    if (commandParameterTypes[i].ParameterType == typeof(Vector3))
                    {
                        parameter = GetVector3FromString(fullSplitInput[i + 1]);
                    }
                    else
                    {
                        parameter = Convert.ChangeType(fullSplitInput[i + 1], commandParameterTypes[i].ParameterType);
                    }

                    convertedParams.Add(parameter);
                }
                catch
                {
                    if (defaultParameters[i] != null)
                    {
                        try
                        {
                            convertedParams.Add(Convert.ChangeType(defaultParameters[i], commandParameterTypes[i].ParameterType));
                        }
                        catch
                        {
                            ShowFailedToDetectValidParamsMessage(fullInput, commandInput);
                            return null;
                        }
                    }
                    else
                    {
                        ShowFailedToDetectValidParamsMessage(fullInput, commandInput);
                        return null;
                    }
                }
            }

            return convertedParams.ToArray();
        }

        private Vector3 GetVector3FromString(string s)
        {
            // Remove ( and ) if they are found.
            s = s.StartsWith("(") && s.EndsWith(")") ? s.Substring(1, s.Length - 2) : s;
            // Split the string into pieces.
            string[] splitVector3String = s.Split('-');

            // Parse to a Vector3.
            return new Vector3(float.Parse(splitVector3String[0]), float.Parse(splitVector3String[1]), float.Parse(splitVector3String[2]));
        }

        private void GetTimesToRunFromCommand(string[] fullCommandArray, ref int timesToRun, ref bool containsColon)
        {
            // Check if last string in our full command contains a colon, if it does then try to parse the string to the right side of the colon to an int.
            // Then clamp that parsed int and use that number as the amount of times the command should be executed.

            if (fullCommandArray.Length > 1 && fullCommandArray[fullCommandArray.Length - 1].Contains(':'))
            {
                string trimmed = fullCommandArray[fullCommandArray.Length - 1].TrimStart(':');
                int.TryParse(trimmed, out timesToRun);
                timesToRun = Mathf.Clamp(timesToRun, 1, ConsoleConfig.MAX_COMMAND_REPEATS);

                containsColon = true;
            }
        }

        private int GetParameterCount(string[] fullCommand, bool containsColonCommand)
        {
            // Length minus one if didn't specify how many times the command has to be executed. We don't want to count the baseCommand as a parameter.
            // Length minus two if we did in fact specify how many times the command has to be exucuted, that does not count as a method parameter.

            return fullCommand.Length - (containsColonCommand ? 2 : 1);
        }

        private void HandleCommandInputFieldNavigation()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                commandInputField.Select();

                if (commandHistory.Count == 0)
                {
                    return;
                }

                historyIndex++;
                historyIndex = Mathf.Clamp(historyIndex, 0, commandHistory.Count - 1);

                commandInputField.text = commandHistory[historyIndex];
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                commandInputField.Select();

                if (commandHistory.Count == 0)
                {
                    return;
                }

                historyIndex--;
                historyIndex = Mathf.Clamp(historyIndex, 0, commandHistory.Count - 1);

                commandInputField.text = commandHistory[historyIndex];
            }
        }

#region CommandLogs
        private void ShowCommandExecutedMessage(string fullCommand, string commandTrigger, int objectCount, int timesToRun)
        {
            ShowCommandMessage($"<color={ColorToString(config.commandTextColor)}>Executed command: " +
                               $"<color={ColorToString(config.commandTriggerColor)}><b>{commandTrigger}</b></color> on " +
                               $"<color={ColorToString(config.commandObjectCountColor)}><b>{objectCount}</b></color> object(s) " +
                               $"<color={ColorToString(config.commandRunCountColor)}><b>{timesToRun}</b></color> time(s).</color>",
                               GetTimeString(GetCurrentTime()),
                               fullCommand);
        }

        private void ShowCommandNotFoundMessage(string fullCommand, string baseCommand)
        {
            ShowCommandMessage($"<color={ColorToString(config.commandTextColor)}>Command " +
                               $"<color={ColorToString(config.commandTriggerColor)}><b>{baseCommand}</b></color> not found.</color>",
                               GetTimeString(GetCurrentTime()),
                               fullCommand);
        }

        private void ShowCommandInvalidParamCountMessage(string fullCommand, string baseCommand, int expectedParamCount)
        {
            ShowCommandMessage($"<color={ColorToString(config.commandTextColor)}>Command " +
                               $"<color={ColorToString(config.commandTriggerColor)}><b>{baseCommand}</b></color> requires " +
                               $"<color={ColorToString(config.commandObjectCountColor)}><b>{expectedParamCount}</b></color> parameter(s) to be executed.</color>",
                               GetTimeString(GetCurrentTime()),
                               fullCommand);
        }

        private void ShowFailedToDetectValidParamsMessage(string fullCommand, string baseCommand)
        {
            ShowCommandMessage($"<color={ColorToString(config.commandTextColor)}>Command " +
                               $"<color={ColorToString(config.commandTriggerColor)}><b>{baseCommand}</b></color> failed to detect valid parameter(s)!</color>",
                               GetTimeString(GetCurrentTime()),
                               fullCommand);
        }

        private void ShowCommandMessage(string text, string time, string fullCommand)
        {
            ConsoleCommandMessage newMessage = (ConsoleCommandMessage)pool.GetNewMessage(ConsolePool.PoolType.Command);
            newMessage.Init(text, GetCurrentTime(), fullCommand);
            messages.Add(newMessage);

            newMessage.gameObject.SetActive(FilterMessage(newMessage));

            commandInputField.text = "";
            StartCoroutine(UpdateWindowScrollPositions());
        }
#endregion

        public void ShowLogInfo(string condition, string stackTrace)
        {
            logConditionText.text = condition;
            logStackTraceText.text = stackTrace + stackTrace + stackTrace;

            logInfoCanvas.enabled = true;
        }

        public void HideLogInfo()
        {
            logInfoCanvas.enabled = false;
        }

        public void ScrollLogInfo(BaseEventData data)
        {
            PointerEventData pointerData = (PointerEventData)data;
            logInfoScrollView.OnScroll(pointerData);
        }

        //[KeyCommand(KeyCode.L, PressType.KeyPressType.Hold)]
        //[ConsoleCommand("log")]
        //private void TestLog()
        //{
        //    int i = UnityEngine.Random.Range(0, 5);
        //    switch (i)
        //    {
        //        case 0:
        //            Debug.Log("TEST: Just a normal log message");
        //            break;

        //        case 1:
        //            Debug.LogAssertion("TEST: This is an assert");
        //            break;

        //        case 2:
        //            Debug.LogError("TEST: ERROR ERROR ERROR ERROR ERROR ERROR");
        //            break;

        //        case 3:
        //            Debug.LogException(new Exception("TEST: Wow an exception, this is unusual!"));
        //            break;

        //        case 4:
        //            Debug.LogWarning("TEST: I use this the most. A log warning.");
        //            break;
        //    }
        //}

        //[KeyCommand(KeyCode.P, PressType.KeyPressType.Down, 3.3f, "yeet", 47)]
        //[ConsoleCommand("param", 3.3f, "yeet", 47)]
        //private void TestLogParameter(float f, string s, int i)
        //{
        //    Debug.Log($"Parameter(s): {f} {s} {i}");
        //}

        //[ConsoleCommand("float")]
        //private void FloatParam(float f)
        //{
        //    Debug.Log($"Float: {f}");
        //}

        //[ConsoleCommand("int")]
        //private void IntParam(int i)
        //{
        //    Debug.Log($"Int: {i}");
        //}

        //[ConsoleCommand("v3")]
        //private void Vector3Param(Vector3 v)
        //{
        //    Debug.Log($"Vector3: {v}");
        //}

        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }
    }
}
