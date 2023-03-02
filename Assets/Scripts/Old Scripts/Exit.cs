using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private LayerMask customerMask;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);

    }
}
