using System;
using TMPro;
using UnityEngine;

namespace DB.CommandConsole
{

    public abstract class ConsoleMessage : MonoBehaviour
    {

        protected ConsoleWindow console;

        public enum MessageType { Log, Error, Exception, Warning, Assert, Command }
        protected MessageType messageType;

        private TimeSpan logTime;
        [SerializeField] private TextMeshProUGUI timeText;

        private int collapseCount = 1;
        [SerializeField] private TextMeshProUGUI collapseCountText;

        public virtual void Init(ConsoleWindow.LogData data)
        {
            timeText.text = ConsoleWindow.GetTimeString(data.logTime);
            logTime = data.logTime;

            ResetPosition();
            console = transform.root.GetComponent<ConsoleWindow>();

            DontDestroyOnLoad(gameObject);
        }

        public virtual void Init(string message, TimeSpan time, string fullCommand)
        {
            timeText.text = ConsoleWindow.GetTimeString(time);
            logTime = time;

            ResetPosition();
            console = transform.root.GetComponent<ConsoleWindow>();

            DontDestroyOnLoad(gameObject);
        }

        public MessageType GetMessageType()
        {
            return messageType;
        }

        private void ResetPosition()
        {
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public void ResetCollapseCount()
        {
            collapseCount = 1;
            if (collapseCountText)
            {
                collapseCountText.text = "";
            }
        }

        public void IncrementCollapseCount()
        {
            collapseCount++;
            if (collapseCountText)
            {
                collapseCountText.text = $"<color=white><b>x</b></color>{collapseCount}";
            }
        }

        public bool IsWithinTimeRange(ConsoleMessage message, float seconds)
        {
            return Mathf.Abs(logTime.Seconds - message.logTime.Seconds) < seconds;
        }

        public abstract bool IsTheSameAs(ConsoleMessage message);

        public abstract bool IsFromTheSameSourceAs(ConsoleMessage message);

        private void OnDisable()
        {
            ResetCollapseCount();
        }
    }
}