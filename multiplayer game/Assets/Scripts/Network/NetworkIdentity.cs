using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace Project.Networking
{
    public class NetworkIdentity : MonoBehaviour
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private bool isControlling;

        public WebSocket ws;

        // Start is called before the first frame update
        void Awake()
        {
            isControlling = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setControllerID (string ID)
        {
            id = ID;
            if(NetworkClient.ClientID == ID)
            {
                isControlling = true;
            }
            else
            {
                isControlling = false;

            }
        }

        public void setWebSocket(WebSocket websocket)
        {
            ws = websocket;
        }

        public string GetID()
        {
            return id;
        }

        public bool IsControlling()
        {
            return isControlling;
        }

        public WebSocket GetSocket()
        {
            return ws;
        }
    }
}