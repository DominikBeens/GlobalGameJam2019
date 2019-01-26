using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DB.CommandConsole
{

    [Serializable]
    public class ConsolePool
    {

        public static Queue<GameObject> logPool = new Queue<GameObject>();
        public static Queue<GameObject> commandPool = new Queue<GameObject>();

        public enum PoolType { Log, Command };

        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject logMessagePrefab;
        [SerializeField] private GameObject commandMessagePrefab;

        public void Init()
        {
            SetupObjectPools();
        }

        private void SetupObjectPools()
        {
            for (int i = 0; i < ConsoleConfig.LOG_POOL_START_COUNT; i++)
            {
                ExpandLogPool();
            }

            for (int i = 0; i < ConsoleConfig.COMMAND_POOL_START_COUNT; i++)
            {
                ExpandCommandPool();
            }
        }

        private void ExpandLogPool()
        {
            GameObject newLog = UnityEngine.Object.Instantiate(logMessagePrefab, Vector3.zero, Quaternion.identity);
            newLog.SetActive(false);
            newLog.hideFlags = HideFlags.HideInHierarchy;

            logPool.Enqueue(newLog);
        }

        private void ExpandCommandPool()
        {
            GameObject newCommand = UnityEngine.Object.Instantiate(commandMessagePrefab, Vector3.zero, Quaternion.identity);
            newCommand.SetActive(false);
            newCommand.hideFlags = HideFlags.HideInHierarchy;

            commandPool.Enqueue(newCommand);
        }

        public ConsoleMessage GetNewMessage(PoolType type)
        {
            GameObject messageObj = null;
            ConsoleMessage message = null;

            CheckPoolAvailability(type);

            switch (type)
            {
                case PoolType.Log:
                    messageObj = logPool.Dequeue();
                    ConsoleLogMessage temp = messageObj.GetComponent<ConsoleLogMessage>();
                    if (temp)
                    {
                        message = temp;
                    }
                    break;

                case PoolType.Command:
                    messageObj = commandPool.Dequeue();
                    ConsoleCommandMessage temp2 = messageObj.GetComponent<ConsoleCommandMessage>();
                    if (temp2)
                    {
                        message = temp2;
                    }
                    break;
            }

            messageObj.transform.SetParent(contentParent);
            messageObj.SetActive(true);
            return message;
        }

        private void CheckPoolAvailability(PoolType type)
        {
            switch (type)
            {
                case PoolType.Log:
                    if (logPool.Count == 0)
                    {
                        ExpandLogPool();
                    }
                    break;

                case PoolType.Command:
                    if (commandPool.Count == 0)
                    {
                        ExpandCommandPool();
                    }
                    break;
            }
        }
    }
}
