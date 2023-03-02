using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    //Queue<GameObject> queue = new Queue<GameObject>();
    List<GameObject> queue = new List<GameObject>();
    LinkedList<GameObject> q = new LinkedList<GameObject>();
    

    public GameObject Last()
    {
       
        GameObject go = null;

        if (queue.Count > 0)
        {
            go= queue[queue.Count - 1];
        }
        return go;
    }
    public GameObject First()
    {
        GameObject go = null;

        if (queue.Count > 0)
        {
            go = queue[0];
        }
        return go;
    }
    public void Add(GameObject gameObject)
    {
        queue.Add(gameObject);
#if DEBUG_QM
        print("**** QueueManager.Add:ID=" + gameObject.GetInstanceID() + ", Count="+queue.Count+" ****");
#endif
    }
    public GameObject PopFirst()
    {
        GameObject go = null;
        if (queue.Count > 0)
        {
            go = queue[0];
            queue.RemoveAt(0);
        }
        return go;
    }
    public int Count()
    {   
        return queue.Count;
    }
    public void Update()
    {
#if DEBUG_QM
        print("*** QueueManager.Update: Count="+queue.Count+" ***");
#endif

    }
    public void Start()
    {
#if DEBUG_QM
        print("*** QueueManager.Start ***");    
#endif
    }
}
