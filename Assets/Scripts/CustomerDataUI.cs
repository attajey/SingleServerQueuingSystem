using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerDataUI : MonoBehaviour
{
    [SerializeField] private Text _customersServedText;
    [SerializeField] private Text _meanInterArrivalTime;

    private void OnEnable()
    {
        if (CustomerDataManager.Instance != null)
        {
            CustomerDataManager.Instance.OnCustomerServedUpdated += UpdateCustomersServedText;
            CustomerDataManager.Instance.OnMeanInterArrivalTimeUpdated += UpdateMeanIntervalArrivalTimeText;
        }
    }

    private void Start()
    {
        CustomerDataManager.Instance.OnCustomerServedUpdated += UpdateCustomersServedText;
        CustomerDataManager.Instance.OnMeanInterArrivalTimeUpdated += UpdateMeanIntervalArrivalTimeText;
    }

    private void OnDisable()
    {

        CustomerDataManager.Instance.OnCustomerServedUpdated -= UpdateCustomersServedText;
        CustomerDataManager.Instance.OnMeanInterArrivalTimeUpdated -= UpdateMeanIntervalArrivalTimeText;
    }

    private void UpdateCustomersServedText(int newNumber)
    {
        Debug.Log("UpdateingValue");
        _customersServedText.text = newNumber.ToString();
    }

    private void UpdateMeanIntervalArrivalTimeText(float newNumber)
    {
        Debug.Log("UpdateingValue");
        _meanInterArrivalTime.text = (newNumber/60f).ToString()+" min";
    }

}
