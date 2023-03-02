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

    //public Transform atm;

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
        //throw new NotImplementedException();
        //target = GetTarget();
        //if (target != null)
        //{
        //    agent.SetDestination(target.position);
        //}
    }
    private void DoWaiting()
    {
        //throw new NotImplementedException();
    }
    private void DoServing()
    {
        agent.isStopped = true;

        //throw new NotImplementedException();
    }


    private void DoServiced()
    {
        agent.SetDestination(exit.position);
        agent.isStopped = false;
        //throw new NotImplementedException();
    }

    public void ChangeState(CustomerState newCarState)
    {
        this.customerState = newCarState;
        FSMCustomer();
    }

    public void ExitService(Transform target)
    {
        //this.SetInService(false);

        queueManager.PopFirst();
        ChangeState(CustomerState.Serviced);
        //targetExit = target;

        //navMeshAgent.SetDestination(target.position);
        //navMeshAgent.isStopped = false;
    }

    public void SetInService(bool value)
    {
        //Chaneg        InService = value;
        //if (InService)
        //{
        //    navMeshAgent.isStopped=true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter the trigger mother fucker");
        if (other.gameObject.tag == "Customer")
        {
            //this.navMeshAgent.desiredVelocity.
            //if (targetCar == null)
            //{
            //targetCar = other.gameObject.transform;
            //navMeshAgent.SetDestination(targetCar.position);
            //}
        }
        else if (other.gameObject.tag == "ATMWindow")
        {
            Debug.Log("Entered trigger atm");
            ChangeState(CustomerState.Servicing);
            //SetInService(true);
        }
        else if (other.gameObject.tag == "Exit")
        {
            Destroy(this.gameObject);
        }
    }

    //private Transform GetTarget()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 5f)) // Look 5m in front
    //    {
    //        return hit.transform;
    //    }
    //    else
    //    {
    //        return atm;
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(this.transform.position, target.transform.position);
    //    if (targetCustomer)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawLine(this.transform.position, targetCustomer.transform.position);

    //    }
    //    if (exit)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(this.transform.position, exit.transform.position);

    //    }


    //}

}
