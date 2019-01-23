using System;
using TMPro;
using UnityEngine;

namespace DB.CommandConsole
{
    public class ConsoleCommandMessage : ConsoleMessage
    {

        private string command;

        [SerializeField] private TextMeshProUGUI commandText;

        public override void Init(string commandMessage, TimeSpan time, string fullCommand)
        {
            base.Init(commandMessage, time, fullCommand);

            commandText.text = commandMessage;
            messageType = MessageType.Command;

            command = fullCommand;
        }

        public void RerunCommand()
        {
            console.ExecuteCommand(command);
        }

        public override bool IsTheSameAs(ConsoleMessage message)
        {
            if (message is ConsoleCommandMessage commandMessage)
            {
                return commandText.text == commandMessage.commandText.text && messageType == commandMessage.messageType;
            }
            else
            {
                return false;
            }
        }

        public override bool IsFromTheSameSourceAs(ConsoleMessage message)
        {
            return false;
        }
    }
}
