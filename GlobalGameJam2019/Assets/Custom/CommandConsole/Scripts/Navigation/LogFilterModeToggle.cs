using UnityEngine;
using UnityEngine.UI;

namespace DB.CommandConsole
{

    public class LogFilterModeToggle : MonoBehaviour
    {

        private ConsoleWindow console;
        private Toggle toggle;

        [SerializeField] private ConsoleConfig.MessageFilterMode filterMode;

        private void Awake()
        {
            console = transform.root.GetComponent<ConsoleWindow>();
            toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            toggle.onValueChanged.AddListener((bool b) => Toggle(b));

            toggle.isOn = console.Config.GetFilterState(filterMode);
            console.Config.SetFilterState(filterMode, toggle.isOn);
        }

        public void Toggle(bool toggle)
        {
            console.Config.SetFilterState(filterMode, toggle);
            console.FilterMessages();
        }
    }
}