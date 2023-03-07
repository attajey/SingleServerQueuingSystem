using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;

    public Action OnfirstArrival;
    public Action<GameObject> OnTargetUpdate;

    public GameObject GetFirst()
    {
        GameObject customer = queueManager.PopFirst();
        
        // > Queue is empty so return null
        if (customer == null) return null;
        
        // > Queue is empty after being removed, update the QueueTarget to the Queue transform itself
        if (queueManager.First() == null)
        {
            OnTargetUpdate?.Invoke(gameObject);
        }
        // > Queue has customers, update the new first Customer to follow the Queue Start
        else
        {
            queueManager.First().GetComponent<CustomerController>().UpdateTarget(gameObject);
            OnTargetUpdate?.Invoke(queueManager.Last());
        }
        return customer;

    }

    public GameObject GetTarget()
    {
        if (queueManager.Count() > 0)
        {
            return queueManager.Last();
        }
        else
        {
            return gameObject;
        }
    }

    public void Enqueue(GameObject customer)
    {
        OnTargetUpdate?.Invoke(customer);
        queueManager.Add(customer);

        // If queue has one person in it after enqueuing, then they are a first arrival
        if (queueManager.Count() == 1)
        {
            OnfirstArrival.Invoke();
        }
    }




}
