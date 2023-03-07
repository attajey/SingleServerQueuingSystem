using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDataManager : MonoBehaviour
{
    public Action<int> OnCustomerServedUpdated;
    public Action<float> OnMeanInterArrivalTimeUpdated;

    [SerializeField] private float _totalIntervalTime = 0f;
    [SerializeField] private int _customersSpawned = 0;

    [SerializeField] private int _customersServed = 0;


    public static CustomerDataManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static CustomerDataManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    public void Start()
    {
        OnCustomerServedUpdated?.Invoke(_customersServed);
        OnMeanInterArrivalTimeUpdated?.Invoke(0f);
    }

    public void CustomerServiced()
    {
        _customersServed++;
        OnCustomerServedUpdated(_customersServed);
    }

    public void AddSpawnIntervalTime(float time)
    {
        _totalIntervalTime += time;
        OnMeanInterArrivalTimeUpdated(GetMeanIntervalTime());
    }

    public void CustomerSpawned()
    {
        _customersSpawned++;
    }

    public float GetMeanIntervalTime()
    {
        return _totalIntervalTime / _customersSpawned;
    }

    public int GetCustomersServed()
    {
        return _customersServed;
    }
}
