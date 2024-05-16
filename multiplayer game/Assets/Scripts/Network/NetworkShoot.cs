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


        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            dataManager = new DataManager();

            
        }

        public void SendShoot(Vector3 hitpoint)
        {
            var id = networkIdentity.GetID();
            int[] b = { 1 };
            int[] n = { 999 };
            var response = dataManager.data(b, "id", id, "hitPoint", dataManager.data(n, "x", hitpoint.x.ToString(), "y", hitpoint.y.ToString(), "z", hitpoint.z.ToString()));
            
            ev.send(networkIdentity.GetSocket(), "shoot", response);
        }

        

    }

}
