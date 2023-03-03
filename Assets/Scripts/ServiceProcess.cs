using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProcess : MonoBehaviour
{

    public GameObject customerInService;
    public Transform customerExitPlace;

    public float serviceRateAsCustomersPerHour = 25;
    public float interServiceTimeInHours; 
    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    public bool generateServices = false;

    public float minInterServiceTimeInSeconds = 3;
    public float maxInterServiceTimeInSeconds = 60;

    QueueManager queueManager;

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    public ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    void Start()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            customerInService = other.gameObject;
            customerInService.GetComponent<CustomerController>().SetInService(true);
            generateServices = true;
            StartCoroutine(GenerateServices());
        }
    }

    IEnumerator GenerateServices()
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
            generateServices = false;
            yield return new WaitForSeconds(timeToNextServiceInSec);
        }
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);

    }

}
