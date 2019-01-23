using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DB.MenuPack
{
    public class Setting_MouseSens : Setting
    {

        public static float mouseSensitivity = 0.1f;
        private const string MOUSE_SENS_PREF_KEY = "MouseSensitivity";

        [SerializeField] private float minSensitivity = 0.01f;
        [SerializeField] private float maxSensitivity = 1f;
        [SerializeField] private float defaultSensitivity = 0.5f;
        [SerializeField] private Slider mouseSensSlider;
        [SerializeField] private TextMeshProUGUI mouseSensText;

        public override void Init()
        {
            base.Init();

            mouseSensitivity = PlayerPrefs.HasKey(MOUSE_SENS_PREF_KEY) ? PlayerPrefs.GetFloat(MOUSE_SENS_PREF_KEY) : defaultSensitivity;

            mouseSensSlider.minValue = minSensitivity;
            mouseSensSlider.maxValue = maxSensitivity;
            mouseSensSlider.value = mouseSensitivity;
            mouseSensSlider.onValueChanged.AddListener(SetMouseSensitivity);

            mouseSensText.text = mouseSensitivity.ToString("F2");
        }

        private void SetMouseSensitivity(float value)
        {
            mouseSensitivity = value;
            mouseSensText.text = mouseSensitivity.ToString("F2");

            PlayerPrefs.SetFloat(MOUSE_SENS_PREF_KEY, mouseSensitivity);
        }
    }
}