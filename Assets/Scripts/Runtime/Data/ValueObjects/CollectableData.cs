using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CollectableData
{
    public CollectableMeshData MeshData;
}

[Serializable]
public struct CollectableMeshData
{
    public List<Mesh> MeshList;
}
