using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;

public class FileReader : MonoBehaviour
{
    // File Reader
    List<string> list = new List<string>();
    string[] lines;
    string[] splittedLines;

    const float TO_SECONDS = 60;


    public Queue<CustomerData> GenerateCustomers()
    {
        Queue<CustomerData> customers = new Queue<CustomerData>();
        List<CustomerData> customerList = new List<CustomerData>();

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

                //customers.Enqueue(new CustomerData(id, arrivalTime, serviceTime));
                customerList.Add(new CustomerData(id, arrivalTime * TO_SECONDS, serviceTime * TO_SECONDS));
            }

            // Order by arrival Time
            customerList = customerList.OrderBy(c => c.arrivalTime).ToList();

            // Load into queue with arrival time as the time from the previous customer
            for (int i = 0; i < customerList.Count; i++)
            {
                //Debug.Log("ArrivalTime: " + customerList[i].arrivalTime);
                if (i != 0)
                {
                    CustomerData customerI = customerList[i];
                    customers.Enqueue(new CustomerData(customerI.id, customerI.arrivalTime - customerList[i - 1].arrivalTime, customerI.serviceTime));
                }
            }

            // For printing customer Queue
            CustomerData[] customersArray = customers.ToArray();
            for (int i = 0; i < customersArray.Length; i++)
            {
                //Debug.Log("ArrivalTime: " + customersArray[i].arrivalTime);
            }


            reader.Close();
            return customers;
        }
    }
}
