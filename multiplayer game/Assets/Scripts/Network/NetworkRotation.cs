using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


namespace Project.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour
    {


        private NetworkIdentity networkIdentity;
        private Vector3 rotation = new Vector3(0,0,0);
        private float gunRotation;
        private string playerID;
        public Transform gunObj;

        private Timer timer1;
        private float stillCounter = 0;
        private DataManager dataManager;
        private CustomEventsHandler ev;
        private GameObject networkManager;
        public void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            dataManager = new DataManager();
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
                    sendData();
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
                //sendData();
                //Debug.LogFormat("({0},{1},{2})", player.rotation.x, player.rotation.y, player.rotation.z);
            }
        }

        private void sendData()
        {

            
                rotation.x = transform.eulerAngles.x;
                rotation.y = transform.eulerAngles.y;
                rotation.z = transform.eulerAngles.z;
                //Debug.LogFormat("({0},{1},{2})", player.rotation.x, player.rotation.y, player.rotation.z);
                gunRotation = gunObj.localRotation.x * 100;



                int[] b = { 1 };
                int[] n = { 999 };

                var playerRotationData = dataManager.data(b, "id", playerID, "rotation", dataManager.data(n, "x", rotation.x.ToString(), "y", rotation.y.ToString(), "z", rotation.z.ToString()));
                ev.send(networkIdentity.GetSocket(), "updateRotation", playerRotationData);
                var gunRotationData = dataManager.data(n, "gunRotation", gunRotation.ToString());
                ev.send(networkIdentity.GetSocket(), "updateGunRotation", gunRotationData);

            
            //update player transform
            
        }
    }

}
