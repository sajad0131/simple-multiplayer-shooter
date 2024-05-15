using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Player;



namespace Project.Networking
{
    public class NetworkHealth : MonoBehaviour
    {
        private int oldHealthChecker;
        private NetworkIdentity networkIdentity;
        private DataManager dataManager;
        private CustomEventsHandler ev;
        private GameObject networkManager;


        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            dataManager = new DataManager();
            
            oldHealthChecker = gameObject.GetComponent<PlayerManager>().healthChecker;
        }

        void Update()
        {
            if (networkIdentity.IsControlling() == true && oldHealthChecker != gameObject.GetComponent<PlayerManager>().healthChecker)
            {
                sendData();

                oldHealthChecker = gameObject.GetComponent<PlayerManager>().healthChecker;

            }


        }

        void sendData()
        {
            var hitedID = GetComponent<PlayerManager>().hitID;
            var gunDamageAmount = GetComponent<PlayerManager>().gunDamageAmount;
            var killer = networkIdentity.GetID();
            int[] n = { 999 };
            var healthData = dataManager.data(n, "hitedID", hitedID, "gunDamageAmount",gunDamageAmount.ToString(),"killer", killer);
            ev.send(networkIdentity.GetSocket(), "updateHealth", healthData);
            

        }

    }

}
