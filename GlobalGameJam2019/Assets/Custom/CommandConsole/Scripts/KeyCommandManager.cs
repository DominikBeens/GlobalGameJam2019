using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DB.CommandConsole
{
    public class KeyCommandManager : MonoBehaviour
    {

        private ConsoleWindow console;
        private List<KeyCommand> commands = new List<KeyCommand>();

        public struct KeyCommand
        {
            public KeyCode[] commandKeyTriggers;
            public PressType.KeyPressType pressType;
            public MethodInfo commandMethod;
            public object[] parameters;
        }

        private void Awake()
        {
            console = GetComponent<ConsoleWindow>();
            FindAllCommands();
        }

        private void Update()
        {
            if (!console.IsOpen || (console.IsOpen && console.Config.detectInputWhenConsoleIsOpen))
            {
                DetectCommandInput();
            }
        }

        private void DetectCommandInput()
        {
            for (int i = 0; i < commands.Count; i++)
            {
                switch (commands[i].pressType)
                {
                    case PressType.KeyPressType.Down:
                        TryExecuteCommand(commands[i], PressType.KeyPressType.Down);
                        break;

                    case PressType.KeyPressType.Up:
                        for (int ii = 0; ii < commands[i].commandKeyTriggers.Length; ii++)
                        {
                            if (Input.GetKeyUp(commands[i].commandKeyTriggers[ii]))
                            {
                                if (ii == commands[i].commandKeyTriggers.Length - 1)
                                {
                                    ExecuteCommand(commands[i]);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;

                    case PressType.KeyPressType.Hold:
                        TryExecuteCommand(commands[i], PressType.KeyPressType.Hold);
                        break;
                }
            }
        }

        private void TryExecuteCommand(KeyCommand command, PressType.KeyPressType pressType)
        {
            // If we need to press multiple keys, loop over them.
            if (command.commandKeyTriggers.Length > 1)
            {
                for (int ii = 0; ii < command.commandKeyTriggers.Length; ii++)
                {
                    // If were at the end of the required keys to press array, check if we pressed the last one and execute the command based on that.
                    if (ii == command.commandKeyTriggers.Length - 1)
                    {
                        switch (pressType)
                        {
                            case PressType.KeyPressType.Down:
                                if (Input.GetKeyDown(command.commandKeyTriggers[ii]))
                                {
                                    ExecuteCommand(command);
                                }
                                break;

                            case PressType.KeyPressType.Hold:
                                if (Input.GetKey(command.commandKeyTriggers[ii]))
                                {
                                    ExecuteCommand(command);
                                }
                                break;
                        }
                    }
                    // This is one of the keys we need to press in order to execute the command. Check if it is pressed down.
                    else
                    {
                        if (!Input.GetKey(command.commandKeyTriggers[ii]))
                        {
                            break;
                        }
                    }
                }
            }
            // We only need to press one key.
            else
            {
                switch (pressType)
                {
                    case PressType.KeyPressType.Down:
                        if (Input.GetKeyDown(command.commandKeyTriggers[0]))
                        {
                            ExecuteCommand(command);
                        }
                        break;

                    case PressType.KeyPressType.Hold:
                        if (Input.GetKey(command.commandKeyTriggers[0]))
                        {
                            ExecuteCommand(command);
                        }
                        break;
                }
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
                    if (Attribute.GetCustomAttribute(methodInfos[ii], typeof(KeyCommandAttribute)) is KeyCommandAttribute attribute)
                    {
                        KeyCommand newCommand = new KeyCommand
                        {
                            commandKeyTriggers = attribute.KeyCodes ?? new KeyCode[] { attribute.KeyCode },
                            pressType = attribute.PressType,
                            commandMethod = methodInfos[ii],
                            parameters = attribute.Parameters ?? null
                        };

                        if (!commands.Contains(newCommand))
                        {
                            commands.Add(newCommand);
                        }
                    }
                }
            }
        }

        private void ExecuteCommand(KeyCommand toExecute)
        {
            Type declaringType = toExecute.commandMethod.DeclaringType;
            UnityEngine.Object[] objects = FindObjectsOfType(declaringType);

            ParameterInfo[] parameters = toExecute.commandMethod.GetParameters();

            if (parameters.Length == 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    toExecute.commandMethod.Invoke(objects[i], null);
                }
            }
            else
            {
                if (toExecute.parameters != null && toExecute.parameters.Length == parameters.Length)
                {
                    for (int i = 0; i < objects.Length; i++)
                    {
                        toExecute.commandMethod.Invoke(objects[i], toExecute.parameters);
                    }
                }
                else
                {
                    Debug.LogWarning($"Command with trigger '<color=red><b>{toExecute.commandKeyTriggers}<b></color>' has incorrect amount of parameters.");
                }
            }
        }
    }
}