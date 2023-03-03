using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform target;
    public Transform exit;

    public bool InService { get; set; }
    public GameObject atmWindow;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            // agent.isStopped = true;
        }
        else if (other.gameObject.tag == "ATMWindow")
        {
            Debug.Log("Entered trigger atm");
            ChangeState(CustomerState.Servicing);
        }
        else if (other.gameObject.tag == "Exit")
        {
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        atmWindow = GameObject.FindGameObjectWithTag("ATMWindow");
        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        agent = GetComponent<NavMeshAgent>();
        target = atmWindow.transform;

        customerState = CustomerState.Arrived;
    }

    void Update()
    {

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
        GetTarget();

        queueManager = GameObject.FindGameObjectWithTag("ATMWindow").GetComponent<QueueManager>();
        queueManager.Add(this.gameObject);

        agent.SetDestination(target.position);
    }

    private void GetTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 5f)) // Look 5m in front
        {
            if (hit.transform.CompareTag("Customer"))
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
            Debug.Log("HIT : " + hit.transform);
        }
        else
        {
            agent.isStopped = false;
        }
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
        if (value)
        {
            ChangeState(CustomerState.Servicing);
        }
    }



}
