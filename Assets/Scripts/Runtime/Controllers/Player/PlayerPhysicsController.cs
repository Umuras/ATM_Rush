using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody managerRigidbody;

    private readonly string _obstacle = "Obstacle";
    private readonly string _atm = "ATM";
    private readonly string _collectable = "Collectable";
    private readonly string _conveyor = "Conveyor";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_obstacle))
        {
            managerRigidbody.transform.DOMoveZ(managerRigidbody.transform.position.z - 10f, 1f).SetEase(Ease.OutBack);
            return;
        }

        if (other.CompareTag(_atm))
        {
            CoreGameSignals.Instance.onAtmTouched?.Invoke(other.gameObject);
            return;
        }

        if (other.CompareTag(_collectable))
        {
            //Burada tagini Collected yapmamýzýn sebebi o toplanan objenin bir daha toplanabilir hale gelmemesi için yapýyoruz.
            other.tag = "Collected";
            StackSignals.Instance.onInteractionCollectable?.Invoke(other.transform.parent.gameObject);
            return;
        }

        if (other.CompareTag(_conveyor))
        {
            CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
            DOVirtual.DelayedCall(1.5f, () => CameraSignals.Instance.onChangeCameraState(CameraStates.MiniGame));
            DOVirtual.DelayedCall(2.5f, () => CameraSignals.Instance.onSetCinemachineTarget(CameraTargetState.FakePlayer));
            return;
        }
    }
}
