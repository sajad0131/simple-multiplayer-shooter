using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

namespace Project.Networking
{
    public class CustomEventsHandler : MonoBehaviour
    {

        public Data senddata;
        public DataManager dataManager;
        public void Start()
        {
            senddata = new Data();
        }

        public void send(WebSocket ws, string EventName, string Data)
        {


            dataManager = new DataManager();
            string d = "{\"eventName\": \"" + EventName + "\", \"data\": " + Data + " }";
            
            //JObject js = JObject.Parse(d);
            //Debug.Log("data is : " + d);
            ws.Send(d);
            



            //Debug.Log( "the data is : " + js.ToString());
        }

        public void on(string name, string data, Func<JObject, JObject> functionToPass)
        {

            JObject js = JObject.Parse(data);

            if (js["eventName"].ToString() == name)
            {
                functionToPass(JObject.Parse(js["data"].ToString()));

            }



        }



    }

    [Serializable]
    public class Data
    {
        public string eventName;
        public JSONObject data;
    }
    [Serializable]
    public class DataManager
    {

        /// <summary>
        /// data(string name,data,name,data, ....)
        /// </summary>
        public string data(int[] branch, params string[] d)
        {
            List<string> datas = new List<string>();
            List<string> names= new List<string>();
            
            
            string da = "";
            int b =0;
            
            for (int i = 1; i < d.Length +1; i++)
            {
                if (i % 2 == 0)
                {
                    datas.Add(d[i-1]);
                   
                    
                }
                else
                {
                    names.Add(d[i-1]);
                    
                    
                }
                
            }
            string[] data = datas.ToArray();
            string[] name = names.ToArray();

            if(branch[0] == 999 && data.Length == 1)
            {
                if (data.Length == 1)
                {
                    da = "{\"" + name[0] + "\": \"" + data[0] + "\"}  ";
                }
            }
            else
            {

            
            
            if(branch[0] != 999)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == 0)
                    {
                        if(branch[0] == 0)
                        {
                            da += "{\"" + name[0] + "\": " + data[0] + ",  ";
                            b++;
                        }
                        else
                        {
                            da += "{\"" + name[0] + "\": \"" + data[0] + "\",  ";
                        }
                        
                        
                    }
                    else if (i == data.Length -1)
                    {
                        if(branch[b] == i)
                        {
                            da += "\"" + name[i] + "\": " + data[i] + "  }";
                            b++;
                        }
                        else
                        {
                            da += "\"" + name[i] + "\": \"" + data[i] + "\"  }";

                        }
                       
                    }
                    else
                    {
                        if(branch[b] == i)
                        {
                            da += "\"" + name[i] + "\": " + data[i] + ",";
                            b++;
                        }
                        else
                        {
                            da += "\"" + name[i] + "\": \"" + data[i] + "\",";
                        }
                        
                    }


                }
                return da;
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == 0)
                    {
                        da += "{\"" + name[0] + "\": \"" + data[0] + "\",  ";
                    }
                    else if (i == data.Length - 1)
                    {
                        da += "\"" + name[i] + "\": \"" + data[i] + "\"  }";
                    }
                    else
                    {
                        da += "\"" + name[i] + "\": \"" + data[i] + "\",";
                    }


                }
                
               
            }
            }

            return da;



        }
    }
}