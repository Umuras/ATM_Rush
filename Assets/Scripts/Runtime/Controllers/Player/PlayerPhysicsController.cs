using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody managerRigidbody;
    [SerializeField]
    private new Collider collider;

    private readonly string _obstacle = "Obstacle";
    private readonly string _atm = "ATM";
    private readonly string _collectable = "Collectable";
    private readonly string _miniGameArea = "MiniGameArea";

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

        if (other.CompareTag(_miniGameArea))
        {
            CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
            return;
        }
    }
}
