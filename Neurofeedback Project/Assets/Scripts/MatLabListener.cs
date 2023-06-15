using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System;
using System.IO;
using System.Text;
public class MatLabListener : MonoBehaviour
{
    TcpListener listener;
    String msg;

    // Start is called before the first frame update
    void Start()
    {
        // establish listener
        listener = new TcpListener(IPAddress.Parse("100.82.194.74"), 55001);
        listener.Start();
        print("is listening");
    }

    // Update is called once per frame
    void Update()
    {
        if (!listener.Pending())
        {
        }
        else
        {
            // receives message from MatLab
            print("socket established");
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            StreamReader reader = new StreamReader(ns);
            msg = reader.ReadToEnd();
            print(msg);
        }
    }
}
