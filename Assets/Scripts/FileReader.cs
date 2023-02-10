using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public class FileReader : MonoBehaviour
{
    // Parameters
    float arrivalTime;
    float serviceTime;
    int id;


    // File Reader
    StreamReader reader = new StreamReader("Assets/customersData.txt");
    List<string> list = new List<string>();
    string[] lines;
    string[] splittedLines;
    public List<Customer> customers = new List<Customer>();


    void Start()
    {
        // Adding each line to list
        for (int i = 0; !reader.EndOfStream; i++)
        {
            string line = reader.ReadLine();
            if (line != null)
            {
                list.Add(line);
            }
        }

        // Converting the list to an array of lines named 'lines'
        lines = list.ToArray();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            // Splitting each line by whitespace, storing them in splittedLines array
            splittedLines = lines[i].Split(null);
            id = Int32.Parse(splittedLines[0]);
            arrivalTime = float.Parse(splittedLines[1]);
            serviceTime = float.Parse(splittedLines[2]);
            //Debug.Log( i + " " + id + " " + arrivalTime + " " + serviceTime);


            customers.Add(gameObject.GetComponent<Customer>().CreateCustomer(id, arrivalTime, serviceTime));
            Debug.Log("ID: " + customers[i].GetId() + " Arrival Time: " + customers[i].GetArrivalTime() + " Service Time: " + customers[i].GetServiceTime());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
