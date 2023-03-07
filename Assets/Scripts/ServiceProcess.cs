using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ServiceProcess : MonoBehaviour
{
    [SerializeField] private Transform customerExitPlace;
    [SerializeField] private CustomerQueue customerQueue;

    public Action<Transform> OnQueueUpdate;

    public float serviceRateAsCustomersPerHour = 25; // customer/hour
    public float interServiceTimeInHours; // = 1.0 / ServiceRateAscustomersPerHour;
    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    //public float ServiceRateAsCarsPerHour = 20; // car/hour
    public bool generateServices = false;

    //Simple generation distribution - Uniform(min,max)
    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;

    private GameObject customerInService;

    // For Testing
    ActionTimer pullTimer = new ActionTimer();

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    // Start is called before the first frame update
    void Awake()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;

        customerQueue.OnfirstArrival += PullCustomer;
    }

    // > Pulls Customer from Queue and places it on the Atm
    public void PullCustomer()
    {
        if (customerInService != null) return;
        if (customerQueue.Empty) return;
        CustomerController customer = customerQueue.GetFirst().GetComponent<CustomerController>();

        customer.UpdateTarget(gameObject);
        customer.SetStopDistance(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        customerInService = other.gameObject;
        generateServices = true;
        StartCoroutine(GenerateService());
    }  
    IEnumerator GenerateService()
    {
        while (generateServices)
        {
            float timeToNextServiceInSec = interServiceTimeInSeconds;
            switch (serviceIntervalTimeStrategy)
            {
                case ServiceIntervalTimeStrategy.ConstantIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                case ServiceIntervalTimeStrategy.UniformIntervalTime:
                    timeToNextServiceInSec = Random.Range(minInterServiceTimeInSeconds, maxInterServiceTimeInSeconds);
                    break;
                case ServiceIntervalTimeStrategy.ExponentialIntervalTime:
                    float U = Random.value;
                    float Lambda = 1 / serviceRateAsCustomersPerHour;
                    timeToNextServiceInSec = Utilities.GenerateExponentiallyDistributedValue(Lambda);
                    break;
                case ServiceIntervalTimeStrategy.ObservedIntervalTime:
                    timeToNextServiceInSec = interServiceTimeInSeconds;
                    break;
                default:
                    print("No acceptable ServiceIntervalTimeStrategy:" + serviceIntervalTimeStrategy);
                    break;

            }

            yield return new WaitForSeconds(timeToNextServiceInSec);
            generateServices = false;
        }

        CustomerDataManager.Instance.CustomerServiced();
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);
        customerInService = null;
        PullCustomer();
    }
}
