using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target = null;
    public Transform targetCustomer;
    public Transform exit = null;

    public bool InService { get; set; }
    public GameObject AtmWindow;
    public QueueManager queueManager;

    public enum CustomerState
    {
        None = -1,
        Arrived,
        Waiting,
        Servicing,
        Serviced
    }

    public CustomerState customerState = CustomerState.None;

    void Start()
    {
        AtmWindow = GameObject.FindGameObjectWithTag("ATMWindow");
        target = AtmWindow.transform;
        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        agent = GetComponent<NavMeshAgent>();

        customerState = CustomerState.Arrived;
        FSMCustomer();
    }

    private void FSMCustomer()
    {
        switch (customerState)
        {
            case CustomerState.Arrived:
                DoArrived();
                break;
            case CustomerState.Waiting:
                DoWaiting();
                break;
            case CustomerState.Servicing:
                Debug.Log("Entered Servicing");
                DoServing();
                break;
            case CustomerState.Serviced:
                DoServiced();
                break;
            default:
                Debug.LogError("State Not Found!\n");
                break;
        }
    }

    private void DoArrived()
    {
        targetCustomer = target;

        queueManager = GameObject.FindGameObjectWithTag("ATMWindow").GetComponent<QueueManager>();
        queueManager.Add(this.gameObject);

        agent.SetDestination(targetCustomer.position);
        agent.isStopped = false;
    }

    private void DoWaiting()
    {
    }

    private void DoServing()
    {
        agent.isStopped = true;
    }

    private void DoServiced()
    {
        agent.SetDestination(exit.position);
        agent.isStopped = false;
    }

    public void ChangeState(CustomerState newCarState)
    {
        this.customerState = newCarState;
        FSMCustomer();
    }

    public void ExitService(Transform target)
    {
        queueManager.PopFirst();
        ChangeState(CustomerState.Serviced);
    }

    public void SetInService(bool value)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
        }
        else if (other.gameObject.tag == "ATMWindow")
        {
            Debug.Log("Entered trigger atm");
            ChangeState(CustomerState.Servicing);
        }
        else if (other.gameObject.tag == "Exit")
        {
            Destroy(this.gameObject);
        }
    }

}
