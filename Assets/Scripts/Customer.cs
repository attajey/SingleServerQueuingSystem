using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private CustomerData customerData = new CustomerData(0,0,0);

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform exit;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(new Vector3(-8,2,20));
    }

    public void InitalizeCustomer(CustomerData customerData)
    {
        this.customerData = customerData;
    }


}
