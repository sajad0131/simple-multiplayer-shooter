using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Player;
using System.Drawing;

namespace Project.Networking
{
    public class NetworkShoot : MonoBehaviour
    {
        
        private NetworkIdentity networkIdentity;
        private DataManager dataManager;
        private CustomEventsHandler ev;
        private GameObject networkManager;


        private void Awake()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            dataManager = new DataManager();

            
        }

        public void SendShoot()
        {
            var id = networkIdentity.GetID();
            
            int[] b = { 999 };
            var response = dataManager.data(b, "id", id);
            
            ev.send(networkIdentity.GetSocket(), "shoot", response);
        }

        

    }

}
