using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float GenerateExponentiallyDistributedValue(float lambda)
    {
        return -Mathf.Log(1 - Random.value) / lambda;
    }
}
