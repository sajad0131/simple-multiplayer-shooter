using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Project.Player;
using UnityEngine.SceneManagement;
using System.Timers;
using TMPro;

namespace Project.Networking
{


    public class NetworkClient : MonoBehaviour
    {
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        private Dictionary<string, NetworkIdentity> serverObjects;
        [SerializeField]
        private string url;
        [SerializeField]
        private GameObject networkContainer;
        [SerializeField]
        private GameObject player;
        private DataManager dataManager;
        private GameObject[] spawnPoint;
        private int spawnindex = 0;
        [SerializeField]
        private GameObject waitingScreen;
        private MenuManager menuManager;
        [SerializeField]
        private GameObject GameUIManager;
        private TextMeshPro debugText;
        
        public static string ClientID { get; private set; }
        public WebSocket igws;
        WebSocket ws;
        CustomEventsHandler ev;

        Timer timer1;
        public int ping;
        // Start is called before the first frame update
        void Start()
        {
            
            serverObjects = new Dictionary<string, NetworkIdentity>();
            dataManager = new DataManager();
            ws = new WebSocket(url);
            ws.Connect();
            ev = gameObject.GetComponent<CustomEventsHandler>();
            spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
            
            menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
            //ws.Send("hello");
            igws = ws;

            ev.send(ws, "latency", new DataManager().data(new int[] { 999 }, "latency", "hi"));
                    timer1 = new Timer();
                    timer1.Elapsed += (sender, e) =>
                    {
                        ping++;
                    };
                    timer1.Interval = 1;//miliseconds
                    timer1.AutoReset = true;
                    timer1.Start();
            

        

                
            ws.OnMessage += (sender, e) =>
            {

                ev.on("latency", e.Data, (data) =>
                {
                    
                    timer1.Stop();
                    return data;
                });
                ev.on("register", e.Data, (data) =>
                {
                    ClientID = data["id"].ToString();
                    
                    menuManager.id=ClientID;
                    return data;

                });

                ev.on("joinedRoom", e.Data, (data) =>
                {
                    _actions.Enqueue(()=>
                    {
                        string id = data["playerID"].ToString();
                        if(ClientID == id)
                        {
                            waitingScreen.SetActive(true);

                        }
                        

                    });
                    return data;
                });

                ev.on("roomIsReady", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["id"].ToString();
                        if (id == ClientID)
                        {
                            Debug.Log("room is ready and id is: " + data["id"].ToString());
                            menuManager.loadLevel();
                        }





                    });
                    return data;
                });

                ev.on("teamApoint", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        GameUIManager = GameObject.FindGameObjectWithTag("GameUIManager");
                        string id = data["killer"]["id"].ToString();
                        int Apoint = Convert.ToInt32( data["Apoint"].ToString());
                        GameUIManager.GetComponent<GameUIManager>().teamAPoint.text = Apoint.ToString();
                        if (id == ClientID)
                        {
                            Debug.Log("you killed  " + data["died"]["id"].ToString());
                            
                        }

                    });
                    return data;
                });


                ev.on("teamBpoint", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        GameUIManager = GameObject.FindGameObjectWithTag("GameUIManager");
                        string id = data["killer"]["id"].ToString();
                        int Bpoint = Convert.ToInt32(data["Bpoint"].ToString());
                        GameUIManager.GetComponent<GameUIManager>().teamBPoint.text = Bpoint.ToString();
                        if (id == ClientID)
                        {
                            Debug.Log("you killed  " + data["died"]["id"].ToString());

                        }

                    });
                    return data;
                });

                ev.on("spawn", e.Data, (data) =>
                {

                    

                    string id = data["id"].ToString();
                    _actions.Enqueue(() =>
                    {


                        //waitingScreen.SetActive(false);
                        Debug.Log("spawning");

                        networkContainer = GameObject.FindGameObjectWithTag("SpawnContainer");
                        GameObject go = GameObject.Instantiate(player);
                        
                        go.name = data["id"].ToString();
                        go.transform.SetParent(networkContainer.transform);
                        NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                        ni.setControllerID(id);
                        ni.setWebSocket(ws);
                        serverObjects.Add(id, ni);
                        
                    });
                    return data;
                });


                



                ev.on("disconnected", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {


                        string id = data["id"].ToString();

                        GameObject go = serverObjects[id].gameObject;
                        Destroy(go);
                        serverObjects.Remove(id);
                        Debug.Log("player {0} has been destroyed" + id);


                    });
                    return data;
                });

                

                ev.on("updatePosition",e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        //Debug.Log(data);
                        string id = data["id"].ToString();
                        NetworkIdentity ni = serverObjects[id];
                        float x = Mathf.Lerp(ni.transform.position.x, (float)data["position"]["x"], 100);
                        float y = Mathf.Lerp(ni.transform.position.y, (float)data["position"]["y"], 100);
                        float z = Mathf.Lerp(ni.transform.position.z, (float)data["position"]["z"], 100);
                        //Debug.Log("id is : " + id + "position is : " + new Vector3(x, y, z));

                        /*float x  = (float)data["position"]["x"];
                        float y  = (float)data["position"]["y"];
                        float z  = (float)data["position"]["z"];*/

                        ni.transform.position = new Vector3(x, y, z);
                    });
                    
                    

                    return data;
                });
                ev.on("updateRotation",e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["id"].ToString();
                        NetworkIdentity ni = serverObjects[id];
                        //float x = Mathf.Lerp(ni.transform.rotation.x, (float)data["rotatoin"]["x"], 100);
                        //float y = Mathf.Lerp(ni.transform.rotation.y, (float)data["rotatoin"]["y"], 100);
                        //float z = Mathf.Lerp(ni.transform.rotation.z, (float)data["rotatoin"]["z"], 100);

                        float x = (float)data["rotation"]["x"];
                        float y = (float)data["rotation"]["y"];
                        float z = (float)data["rotation"]["z"];



                        ni.transform.rotation = Quaternion.Euler(x, y, z);
                    });
                    return data;
                });
                ev.on("updateGunRotation", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["id"].ToString();
                        float gunRotation = (float) data["gunRotation"];
                        NetworkIdentity ni = serverObjects[id];
                        Transform gunobj = ni.gameObject.GetComponent<NetworkRotation>().gunObj.transform;
                        gunobj.localRotation = Quaternion.Euler(gunRotation, gunobj.rotation.y, gunobj.rotation.z );
                        
                    });
                    return data;
                });
                ev.on("shoot", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["id"].ToString();
                        float x = (float)data["hitPoint"]["x"];
                        float y = (float)data["hitPoint"]["y"];
                        float z = (float)data["hitPoint"]["z"];
                        Vector3 point = new Vector3(x,y,z);
                        NetworkIdentity ni = serverObjects[id];
                        ni.gameObject.GetComponent<PlayerManager>().muzzle.Play();
                        var audioSource = GetComponent<PlayerManager>().gunAudioSource;
                        AudioSource shootSource = Instantiate(audioSource, ni.GetComponent<PlayerManager>().GunOrigin, ni.GetComponent<PlayerManager>().GunOrigin);
                        
                        shootSource.Play();



                    });
                    return data;
                });

                ev.on("updateHealth", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {


                        string id = data["id"].ToString();

                        GameObject go = serverObjects[id].gameObject;
                        Destroy(go);
                        serverObjects.Remove(id);
                        Debug.Log("player {0} has been destroyed" + id);


                    });
                    return data;
                });

                ev.on("playerDied", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["id"].ToString();
                        GameObject player = serverObjects[id].gameObject;
                        player.SetActive(false);
                    });
                    return data;
                });


                ev.on("updateDeadPlayer", e.Data, (data) =>
                {
                    _actions.Enqueue(() =>
                    {
                        string id = data["player"]["id"].ToString();
                        float x1 = (float)data["point1"]["x"];
                        float y1 = (float)data["point1"]["y"];
                        float z1 = (float)data["point1"]["z"];

                        float x2 = (float)data["point2"]["x"];
                        float y2 = (float)data["point2"]["y"];
                        float z2 = (float)data["point2"]["z"];
                        Vector3 point1 = new Vector3(x1,y1,z1);
                        Vector3 point2 = new Vector3(x1,y2,z2);
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                        GameObject player = serverObjects[id].gameObject;
                        for (int i = 0; i < players.Length; i++)
                        {
                            if(Vector3.Distance( players[i].transform.position,point1) > 2 || Vector3.Distance(players[i].transform.position, point2) > 2)
                            {
                                if(Vector3.Distance(players[i].transform.position, point1) > Vector3.Distance(players[i].transform.position, point2))
                                {
                                    player.transform.position = point1;
                                }
                                else
                                {
                                    player.transform.position = point2;
                                }
                            }
                            else
                            {
                                player.transform.position = new Vector3(0, 9.3f,21.2f);
                            }

                        }
                        
                        player.SetActive(true);
                    });
                    return data;
                });



            };




            

            
        }
        public string GetClientID()
        {
            return ClientID;
        }

        void OnApplicationQuit()
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            ev.send(ws, "disconnected",dataManager.data(new int[999],"id", this.GetComponent<NetworkIdentity>().GetID()));
            Debug.Log(dataManager.data(new int[999], "id", this.GetComponent<NetworkIdentity>().GetID()));
            ws.Close();

        }

        // Update is called once per frame
        void Update()
        {
            // Work the dispatched actions on the Unity main thread
            while (_actions.Count > 0)
            {
                if (_actions.TryDequeue(out var action))
                {
                    action?.Invoke();
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //ev.send(ws, "updatePosition", "(0,0,0)");
                Debug.Log("position sent!");
            }

        }

       

        string b(string e)
        {
            return "hi";
        }
    }

    [Serializable]
    public class Player
    {
        public string id;
        public Position position;
    }
    [Serializable]
    public class Position
    {
        public float x;
        public float y; 
        public float z;

    }
}