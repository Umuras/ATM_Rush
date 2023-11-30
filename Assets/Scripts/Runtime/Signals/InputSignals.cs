using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputSignals : MonoSingleton<InputSignals>
{
    public UnityAction onFirstTimeTouchTaken = delegate { };
    public UnityAction onInputTaken = delegate { };
    public UnityAction<HorizontalInputParams> onInputDragged = delegate { };
    public UnityAction onInputReleased = delegate { };
    public UnityAction<bool> onChangeInputState = delegate { };
}
