using System;
using UnityEngine;

public class CollectableMeshController : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    private CollectableMeshData _data;


    private void OnEnable()
    {
        ActivateMeshVisuals();
    }

    internal void SetMeshData(CollectableMeshData meshData)
    {
        _data = meshData;
    }
    private void ActivateMeshVisuals()
    {
        meshFilter.mesh = _data.MeshList[0];
    }


    public void OnUpgradeCollectableVisual(int value)
    {
        meshFilter.mesh = _data.MeshList[value];
    }
}
