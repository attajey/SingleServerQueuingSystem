using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public Transform atm;
    public Transform exit;


    public enum CustomerState
    {
        None = -1,
        Arrived,
        Waiting,
        Servicing,
        Serviced
    }

    public CustomerState customerState = CustomerState.None;
    public Transform target;

    public NavMeshAgent agent;
    void Start()
    {
        customerState = CustomerState.Arrived;
        atm = GameObject.FindGameObjectWithTag("ATM").transform;
        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
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

    private void DoServiced()
    {
        //throw new NotImplementedException();
    }

    private void DoServing()
    {
        //throw new NotImplementedException();
    }

    private void DoWaiting()
    {
        //throw new NotImplementedException();
    }

    private void DoArrived()
    {
        //throw new NotImplementedException();
        target = GetTarget();
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private Transform GetTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 5f)) // Look 5m in front
        {
            return hit.transform;
        }
        else
        {
            return atm;
        }
    }

}
