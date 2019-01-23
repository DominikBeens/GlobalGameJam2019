using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class KeyCommandAttribute : Attribute
{

    private KeyCode keyCode;
    public KeyCode KeyCode { get { return keyCode; } }

    private KeyCode[] keyCodes;
    public KeyCode[] KeyCodes { get { return keyCodes; } }

    private PressType.KeyPressType pressType;
    public PressType.KeyPressType PressType { get { return pressType; } }

    private object[] parameters;
    public object[] Parameters { get { return parameters; } }

    /// <param name="keyCode">
    /// The key required to be pressed to execute this function.
    /// </param>
    /// <param name="pressType">
    /// The type of keypress required to execute this function.
    /// </param>
    /// <param name="parameters">
    /// Parameters for this function.
    /// </param>
    public KeyCommandAttribute(KeyCode keyCode, PressType.KeyPressType pressType, params object[] parameters)
    {
        this.keyCode = keyCode;
        this.pressType = pressType;
        this.parameters = parameters;
    }

    /// <param name="keyCodes">
    /// The set of keys required to be pressed to execute this function. (Note: KeyPressType.Down: keys need to be pressed in the correct order, 
    /// KeyPressType.Hold: keys don't need to be pressed in the correct order, KeyPressType.Up: all keys need to be released in the same frame)
    /// </param>
    public KeyCommandAttribute(KeyCode[] keyCodes, PressType.KeyPressType pressType, params object[] parameters)
    {
        if (keyCodes.Length > 1)
        {
            this.keyCodes = keyCodes;
        }
        else
        {
            this.keyCode = keyCodes[0];
        }

        this.pressType = pressType;
        this.parameters = parameters;
    }
}
