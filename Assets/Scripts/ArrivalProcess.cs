using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalProcess : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform customerSpawnPlace;

    [Header("Customer Init Data")]
    [SerializeField] private ServiceProcess ATM;
    [SerializeField] private Transform exit;
    [SerializeField] private CustomerQueue queue;


    // For Expo Dist. we need the Lambda (the avg arrival rate, cars/hour)

    // Observed Data
    [SerializeField] private float arrivalRateInCustomerPerHour = 20; // avg rate
    [SerializeField] private float interArrivalTimeInHours; // = 1.0 / arrivalRateAsCarsPerHour;
    private float interArrivalTimeInMinutes;
    private float interArrivalTimeInSeconds;

    // Calculated Data
    //private float interArrivalTimeInMin; // avg #mins between car arrivals

    // Control Simulation Data
    private bool isSimulationRunning = true;

    //Simple generation distribution - Uniform(min,max)
    //
    [SerializeField] private float minInterArrivalTimeInSeconds = 3;
    [SerializeField] private float maxInterArrivalTimeInSeconds = 60;

    public enum ArrivalIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ArrivalIntervalTimeStrategy arrivalIntervalTimeStrategy = ArrivalIntervalTimeStrategy.UniformIntervalTime;

    void Start()
    {        
        isSimulationRunning = true;
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
            customerGameObject.GetComponent<CustomerController>().InitCustomer(queue);

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
                    float U = Random.value;
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
