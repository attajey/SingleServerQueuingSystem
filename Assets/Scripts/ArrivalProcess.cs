using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalProcess : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform customerSpawnPlace;

    [Header("Exponential Interval Time Properties")]
    [SerializeField] private float arrivalRateInCustomerPerHour = 20;


    [Header("Constant & Observed Interval Time Properties")]
    [SerializeField] private float interArrivalTimeInHours;

    [Header("Uniform Interval Time Properties")]
    [SerializeField] private float minInterArrivalTimeInSeconds = 3;
    [SerializeField] private float maxInterArrivalTimeInSeconds = 60;
    
    [SerializeField] private ArrivalIntervalTimeStrategy arrivalIntervalTimeStrategy = ArrivalIntervalTimeStrategy.UniformIntervalTime;

    [SerializeField] private bool isSimulationRunning = true;

    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;

    //QueueManager queueManager;
    public enum ArrivalIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    void Start()
    {
        isSimulationRunning = true;

        //queueManager = GameObject.FindGameObjectWithTag("ATMWindow").GetComponent<QueueManager>();

        interArrivalTimeInHours = 1.0f / arrivalRateInCustomerPerHour;
        interArrivalTimeInMinutes = interArrivalTimeInHours * 60;
        interArrivalTimeInSeconds = interArrivalTimeInMinutes * 60;

        StartCoroutine(GenerateArrivals());
    }

    IEnumerator GenerateArrivals()
    {
        while (isSimulationRunning)
        {
            GameObject customerGameObject = Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);

            float timeToNextArrivalInSec = interArrivalTimeInSeconds;

            switch (arrivalIntervalTimeStrategy)
            {
                case ArrivalIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;
                case ArrivalIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextArrivalInSec = Random.Range(minInterArrivalTimeInSeconds, maxInterArrivalTimeInSeconds);
                    break;
                case ArrivalIntervalTimeStrategy.ExponentialIntervalTime:
                    float Lambda = 1 / arrivalRateInCustomerPerHour;
                    timeToNextArrivalInSec = Utilities.GenerateExponentiallyDistributedValue(Lambda);
                    break;
                case ArrivalIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextArrivalInSec = interArrivalTimeInSeconds;
                    break;
                default:
                    print("No acceptable arrivalIntervalTimeStrategy:" + arrivalIntervalTimeStrategy);
                    break;
            }
            yield return new WaitForSeconds(timeToNextArrivalInSec);
        }
    }
}
