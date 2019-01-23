using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DB.CommandConsole
{
    public class ConsoleLogMessage : ConsoleMessage
    {

        [SerializeField] private TextMeshProUGUI conditionText;
        [SerializeField] private TextMeshProUGUI stackTraceText;

        public override void Init(ConsoleWindow.LogData data)
        {
            base.Init(data);

            conditionText.text = data.condition;
            conditionText.color = data.textColor;

            stackTraceText.text = data.stackTrace;
            stackTraceText.color = data.textColor;

            switch (data.logType)
            {
                case LogType.Error:
                    messageType = MessageType.Error;
                    break;

                case LogType.Assert:
                    messageType = MessageType.Assert;
                    break;

                case LogType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case LogType.Log:
                    messageType = MessageType.Log;
                    break;

                case LogType.Exception:
                    messageType = MessageType.Exception;
                    break;
            }
        }

        public void ShowLogInfo()
        {
            console.ShowLogInfo(conditionText.text, stackTraceText.text);
        }

        public void HideLogInfo()
        {
            console.HideLogInfo();
        }

        public void ScrollLogInfo(BaseEventData data)
        {
            console.ScrollLogInfo(data);
        }

        public override bool IsTheSameAs(ConsoleMessage message)
        {
            if (message is ConsoleLogMessage logMessage)
            {
                return conditionText.text == logMessage.conditionText.text && stackTraceText.text == logMessage.stackTraceText.text && messageType == logMessage.messageType;
            }
            else
            {
                return false;
            }
        }

        public override bool IsFromTheSameSourceAs(ConsoleMessage message)
        {
            if (message is ConsoleLogMessage logMessage)
            {
                return stackTraceText.text == logMessage.stackTraceText.text;
            }
            else
            {
                return false;
            }
        }
    }
}
