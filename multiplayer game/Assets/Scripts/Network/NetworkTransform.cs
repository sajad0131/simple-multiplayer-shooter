using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace Project.Networking
{


    public class NetworkTransform : MonoBehaviour
    {
        [SerializeField]
        private GameObject networkManager;
        private Vector3 oldPosition;
        private NetworkIdentity networkIdentity;
        private string playerID;
        private float stillCounter = 0;
        private CustomEventsHandler ev;
        private DataManager dataManager;
        private Timer timer1;
        // Start is called before the first frame update
        public void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            oldPosition = transform.position;
            
            dataManager = new DataManager();
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            
            playerID = networkIdentity.GetID();
            if (!networkIdentity.IsControlling())
            {
                enabled = false;
            }
            else
            {
                timer1 = new Timer();
                timer1.Elapsed += (sender, e) =>
                {
                    SendData();
                };
         timer1.Interval = 50;//miliseconds
                
                timer1.Start();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (networkIdentity.IsControlling())
            {
                if(oldPosition != transform.position)
                {
                    oldPosition = transform.position;
                    stillCounter = 0;
                    //SendData();
                }
                else
                {
                    stillCounter += Time.deltaTime;

                    if(stillCounter >= 1)
                    {
                        //SendData();
                    }
                }
            }
        }

        private void SendData()
        {
           

            
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                int[] b = { 1 };
                int[] n = { 999 };
                var response = dataManager.data(b, "id", playerID, "position", dataManager.data(n, "x", x.ToString(), "y", y.ToString(), "z", z.ToString()));

                ev.send(networkIdentity.GetSocket(), "updatePosition", response);
           

            
        }
    }
}