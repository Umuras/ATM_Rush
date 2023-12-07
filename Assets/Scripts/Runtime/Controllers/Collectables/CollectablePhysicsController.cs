using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePhysicsController : MonoBehaviour
{
    [SerializeField]
    private CollectableManager collectableManager;

    private readonly string _collectable = "Collectable";
    private readonly string _collected = "Collected";
    private readonly string _gate = "Gate";
    private readonly string _atm = "ATM";
    private readonly string _obstacle = "Obstacle";
    private readonly string _conveyor = "Conveyor";

    private void OnTriggerEnter(Collider other)
    {
        //Buray� anlamad�m.
        if (other.CompareTag(_collectable) && CompareTag(_collected))
        {
            other.tag = _collected;
            collectableManager.InteractionWithCollectable(other.transform.parent.gameObject);
        }

        //Burada Collected tagine bakmas�n�n sebebi stack objesi f�rlat�ld���nda Collectable tagine sahip oluyor ve f�rlat�ld��� anda
        //atm, obstacle ya da atm gibi yerlere de�di�inde i�leme girmesin diye sa�lama ama�l�.
        if (other.CompareTag(_gate) && CompareTag(_collected))
        {
            CollectableSignals.Instance.onCollectableUpgrade?.Invoke(collectableManager.GetCurrentValue());
        }

        if (other.CompareTag(_atm) && CompareTag(_collected))
        {
            collectableManager.InteractionWithAtm(transform.parent.gameObject);
        }

        if (other.CompareTag(_obstacle) && CompareTag(_collected))
        {
            collectableManager.InteractionWithObstacle(transform.parent.gameObject);
        }

        if (other.CompareTag(_conveyor) && CompareTag(_collected))
        {
            collectableManager.InteractionWithConveyor();
        }
    }
}
