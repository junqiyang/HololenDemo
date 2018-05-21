using HoloToolkit;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using SimpleJSON;
using UnityEngine.Networking;
using System;

public class AppStateManager : Singleton<AppStateManager>
{
    string DEM_file;
    float Longitude = (float)-123.374686146645, Latitude = (float)38.6206038029538,  Pixel_length = (float)0.0174532925199433;
    int Width = 522, Height = 581, VID;
    string uid;
    int counter = 0;
    GameObject mapview;
    GameObject cloudmesh;
    GameObject navigates;
    GameObject infom;
    bool dis_info = false;

    string URL;
    string IP = "http://128.194.140.193:8888/";
    public enum AppState
    {
        Waitingfordisplay = 0,
        poll_file_name,
        begin_display,
        Ready
    }


    public AppState CurrentAppState { get; set; }
    // Use this for initialization
    void Start () {
        CurrentAppState = AppState.Waitingfordisplay;
        mapview = GameObject.Find("map_view");
        cloudmesh = GameObject.Find("cloudMeshMap");
        navigates = GameObject.Find("navigators");
        infom = GameObject.Find("info");
        GestureManager.Instance.OverrideFocusedObject = GameObject.Find("worldmap");
        uid = getUTCTime();

    }

    public void ResetStage()
    {
        CurrentAppState = AppState.Waitingfordisplay;
    }

    // Update is called once per frame
    void Update () {

        URL = IP+"check_visual_request" + "?t=" + getUTCTime();
        switch (CurrentAppState){
            case AppState.Waitingfordisplay:
                CurrentAppState = AppState.poll_file_name;  
                navigates.GetComponent<navigator>().set_IP(IP, uid);
                break;
            case AppState.poll_file_name:
                WWW request = new WWW(URL);
                while (!request.isDone) {

                }

                if(dis_info == false)
                {
                    infom.GetComponent<tutorial>().download_complete();
                    dis_info = true;
                }


                string readHtml = request.text;
                var N = JSON.Parse(readHtml);
                VID = N["vrid"];
                DEM_file = N["DEM"].ToString();
                Longitude = N["Longitude"];
                Latitude = N["Latitude"];
                Height = N["Height"];
                Width = N["Width"];
                Pixel_length = N["Pixel_length"];

  
                switch (VID) {
                    case 1:
                        mapview.GetComponent<load_map>().show_image(Width, Height, "DEM1/tmp");
                        cloudmesh.GetComponent<cloud_mesh>().display_request_received(Width, Height, 1);
                        navigates.GetComponent<navigator>().set_para_2(Height, Width, Longitude, Latitude, Pixel_length);
                        break;
                    case 2:
                        mapview.GetComponent<load_map>().show_image(Width, Height, "DEM3/2_0");
                        cloudmesh.GetComponent<cloud_mesh>().display_request_received(Width, Height, 2);
                        navigates.GetComponent<navigator>().set_para_2(Height, Width, Longitude, Latitude, Pixel_length);
                        break;
                    case 3:
                        mapview.GetComponent<load_map>().show_image(Width, Height, "DEM2/3_0");
                        cloudmesh.GetComponent<cloud_mesh>().display_request_received(Width, Height, 3);
                        navigates.GetComponent<navigator>().set_para_2(Height, Width, Longitude, Latitude, Pixel_length);
                        break;
                }
                CurrentAppState = AppState.Ready;
                break;

            case AppState.Ready:

                counter++;

                if(counter == 500)
                {

                   
                    WWW check = new WWW(URL);
                    while (!check.isDone)
                    {
                    }
                    string readcheck = check.text;
                    var M = JSON.Parse(readcheck);
                    int tmp_id = M["vrid"];
                    if (tmp_id != VID) {
                         Debug.Log("Here");      
                        cloudmesh.GetComponent<cloud_mesh>().reset_map();
                        navigates.GetComponent<navigator>().reset();
                        CurrentAppState = AppState.poll_file_name;
                    }
                    counter = 0;
                }
                break;
        }
    }


    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }


}
