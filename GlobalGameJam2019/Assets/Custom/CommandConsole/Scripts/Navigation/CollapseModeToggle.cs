using UnityEngine;
using UnityEngine.UI;

namespace DB.CommandConsole
{

    public class CollapseModeToggle : MonoBehaviour
    {

        private ConsoleWindow console;
        private Toggle toggle;

        private void Awake()
        {
            console = transform.root.GetComponent<ConsoleWindow>();
            toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            toggle.onValueChanged.AddListener((bool b) => Toggle(b));

            toggle.isOn = console.Config.GetCollapseState();
            console.Config.SetCollapseState(toggle.isOn);
        }

        public void Toggle(bool toggle)
        {
            console.Config.SetCollapseState(toggle);
            console.FilterMessages();
        }
    }
}