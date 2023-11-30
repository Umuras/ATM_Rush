using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
//Daha efektif bir optimizasyon yapmak istiyorsak structta �al��mam�z daha do�ru.
public struct InputData
{
    public float HorizontalInputSpeed;
    public float2 HorizontalInputClampSides;
    public float HorizontalInputClampStopValue;
}
