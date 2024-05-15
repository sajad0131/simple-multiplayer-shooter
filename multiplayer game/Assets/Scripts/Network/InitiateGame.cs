using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Project.Networking
{


    public class InitiateGame : MonoBehaviour
    {
        NetworkIdentity networkIdentity;
        GameObject networkManager;
        CustomEventsHandler ev;
        DataManager dataManager;
        
        private void Start()
        {
            
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
            ev = networkManager.GetComponent<CustomEventsHandler>();
            dataManager = new DataManager();
        }
        public void Initiate(string map , string mode, string id)
        {

            ev.send(networkManager.GetComponent<NetworkClient>().igws, "initiateGame", dataManager.data(new int[]{999} , "map", map,"mode" ,  mode,"id" , id));
        }
        public void mapLoaded(string id)
        {
            Debug.Log("data sent");
            ev.send(networkManager.GetComponent<NetworkClient>().igws, "mapLoaded", dataManager.data(new int[] { 999 }, "id", NetworkClient.ClientID));
        }



    }



}

