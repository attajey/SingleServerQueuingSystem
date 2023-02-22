using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public class FileReader : MonoBehaviour
{
    // File Reader
    List<string> list = new List<string>();
    string[] lines;
    string[] splittedLines;


    public Queue<CustomerData> GenerateCustomers()
    {
        Queue<CustomerData> customers = new Queue<CustomerData>();

        using (StreamReader reader = new StreamReader("Assets/customersData.txt"))
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
                int id = Int32.Parse(splittedLines[0]);
                float arrivalTime = float.Parse(splittedLines[1]);
                float serviceTime = float.Parse(splittedLines[2]);

                customers.Enqueue(new CustomerData(id, arrivalTime, serviceTime));
                Debug.Log("ID: " + customers.Peek().id + " Arrival Time: " + customers.Peek().arrivalTime + " Service Time: " + customers.Peek().serviceTime);
            }

            reader.Close();
            return customers;
        }
    }
}
