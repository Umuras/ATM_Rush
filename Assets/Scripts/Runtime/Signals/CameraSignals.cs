using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSignals : MonoSingleton<CameraSignals>
{
    public UnityAction<CameraStates> onChangeCameraState = delegate { };
    public UnityAction<CameraTargetState> onSetCinemachineTarget = delegate { };
}
