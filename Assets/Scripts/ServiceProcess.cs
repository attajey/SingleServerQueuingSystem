using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProcess : MonoBehaviour
{
    [SerializeField] private GameObject customerInService;
    [SerializeField] private Transform customerExitPlace;

    [Header("Exponential Interval Time Properties")]
    [SerializeField] private float serviceRateAsCustomersPerHour = 25;

    [Header("Constant & Observed Interval Time Properties")]
    [SerializeField] private float interServiceTimeInHours; 

    [Header("Uniform Interval Time Properties")]
    [SerializeField] private float minInterServiceTimeInSeconds = 3;
    [SerializeField] private float maxInterServiceTimeInSeconds = 60;

    [SerializeField] private ServiceIntervalTimeStrategy serviceIntervalTimeStrategy = ServiceIntervalTimeStrategy.UniformIntervalTime;

    [SerializeField] private bool generateServices = false;

    private float interServiceTimeInMinutes;
    private float interServiceTimeInSeconds;

    //QueueManager queueManager;

    public enum ServiceIntervalTimeStrategy
    {
        ConstantIntervalTime,
        UniformIntervalTime,
        ExponentialIntervalTime,
        ObservedIntervalTime
    }

    void Start()
    {
        interServiceTimeInHours = 1.0f / serviceRateAsCustomersPerHour;
        interServiceTimeInMinutes = interServiceTimeInHours * 60;
        interServiceTimeInSeconds = interServiceTimeInMinutes * 60;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Customer")
    //    {
    //        customerInService = collision.gameObject;

    //        customerInService.GetComponent<CustomerController>().SetInService(true);

    //        generateServices = true;

    //        StartCoroutine(GenerateServices());
    //    }
    //}

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

            //Debug.Log("Service Time for current customer: " + timeToNextServiceInSec);

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

            //Debug.Log("Service Time for next customer: " + timeToNextServiceInSec);

            yield return new WaitForSeconds(timeToNextServiceInSec);
        }
        customerInService.GetComponent<CustomerController>().ExitService(customerExitPlace);
    }
}
