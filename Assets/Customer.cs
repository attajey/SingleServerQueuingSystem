using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Customer : MonoBehaviour
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

    List<Customer> customers = new List<Customer>();

    
    public Customer(int id, float arrivalTime, float serviceTime)
    {
        this.id = id;
        this.arrivalTime = arrivalTime;
        this.serviceTime = serviceTime;
    }

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

        for (int i = 0; i < lines.Length; i++)
        {
            // Splitting each line by whitespace, storing them in splittedLines array
            splittedLines = lines[i].Split(null);

            id = Int32.Parse(splittedLines[0]);
            arrivalTime = float.Parse(splittedLines[1]);
            serviceTime = float.Parse(splittedLines[2]);

            customers[i] = new Customer(id, arrivalTime, serviceTime);
            Debug.Log("ID: " + customers[i].id + " Arrival Time: " + customers[i].arrivalTime + " Service Time: " + customers[i].serviceTime);
        }

    }

}
