using UnityEngine;
using System.Collections;
using SimpleJSON;
using HoloLensXboxController;
using System;

public class navigator : MonoBehaviour {
    int row_num = 380;
    int col_num = 708;
    int resolution = 200;
    float Longti = 0;
    float Lat = 0;
    float Pixel_l = 1;
    public float get_lat = 0;
    public float get_log = 0;
    public float get_Pix = 0;
    public bool enable = false;
    private ControllerInput controllerInput;
    string IP;
    string uid;
    // Use this for initialization
    void Start()
    {
        controllerInput = new ControllerInput(0, 0.19f);

        //
    }



    public void enable_movement()
    {
        enable = true;
    }

    public void set_para(int r, int c, int res)
    {
        row_num = r;
        col_num = c;
        resolution = res;
    }

    public void set_IP(string NIP, string NID) {
        IP = NIP;
        uid = NID;

    }

    public void set_para_2(int r, int c, float lo, float la, float pix)
    {
        row_num = r;
        col_num = c;
        Longti = lo;
        Lat = la;
        Pixel_l = pix;
    }

    void send_coor(Vector3 curr) {
        int x = (int)curr.x;
       
  //      Debug.Log(x);
        int y = (int)curr.y;
  //      Debug.Log(y);
        //string test = "pixel x: " + x.ToString() + ", pixel y: " + y.ToString();
        float n_lo = Longti + ((float)x + col_num / 2 - resolution / 2) * Pixel_l;
        float n_la = Lat + ((float)y - row_num / 2 + resolution / 2) * Pixel_l;
        get_lat = n_la;
        get_log = n_lo;
        get_Pix = Pixel_l;
        
        string sdd = IP+"change_focus_region?uid="+uid[uid.Length-1]+"&lng=" + n_lo.ToString("0.0000000000000") + "&lat=" +
            n_la.ToString("0.0000000000000") + "&height=" + resolution.ToString() + "&width=" + resolution.ToString();
        WWW request = new WWW(sdd);
       Debug.Log(sdd);
       /* string Js = "{\"Longitude\": " + n_la.ToString("0.0000") + ", \"Latitude\": " + n_lo.ToString("0.0000")
             + ", \"Height\": " + col_num.ToString() + ", \"Width\": " + row_num.ToString() + ",  \"vid\": 12}";
         Debug.Log(Js);*/
        //        Debug.Log(n_lo.ToString("0.00000000"));
        //        Debug.Log(n_la.ToString("0.00000000"));
        //{"Longitude": -123.374686146645, "Latitude": 38.6206038029538, "Height": 380, "Width": 708, "vid: 12"}
    }

    public void reset() {
        this.transform.localPosition =new Vector3(0, 0, 0);

    }


    // Update is called once per frame
    void Update()
    {
        controllerInput.Update();
        GameObject cloud = GameObject.Find("cloudMeshMap");
        if (enable == false)
        {
            return;
        }

        if (Input.GetAxis("Vertical") > 0 || controllerInput.GetAxisLeftThumbstickY() > 0 )
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.y++;
            if (current_position.y + resolution / 2 <= row_num / 2)
            {
                this.transform.localPosition = current_position;
                cloud.GetComponent<cloud_mesh>().Update_position(current_position);
                send_coor(current_position);
            }
        }
        if (Input.GetAxis("Vertical") < 0 || controllerInput.GetAxisLeftThumbstickY() < 0)
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.y--;
            if (current_position.y - resolution / 2 >= -row_num / 2)
            {
                this.transform.localPosition = current_position;
                cloud.GetComponent<cloud_mesh>().Update_position(current_position);
                send_coor(current_position);
            }
        }
        if (Input.GetAxis("Horizontal") < 0  || controllerInput.GetAxisLeftThumbstickX() < 0)
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.x--;
            if (current_position.x - resolution / 2 >= -col_num / 2)
            {
                this.transform.localPosition = current_position;
                cloud.GetComponent<cloud_mesh>().Update_position(current_position);
                send_coor(current_position);
            }
        }
        if (Input.GetAxis("Horizontal") > 0 || controllerInput.GetAxisLeftThumbstickX() > 0)
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.x++;
            if (current_position.x + resolution / 2 <= col_num / 2)
            {
                this.transform.localPosition = current_position;
                cloud.GetComponent<cloud_mesh>().Update_position(current_position);
                send_coor(current_position);
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton8) || (controllerInput.GetButton(ControllerButton.LeftThumbstick)))
        {
            Vector3 current_position = new Vector3(0, 0, 0);
            this.transform.localPosition = current_position;
            cloud.GetComponent<cloud_mesh>().Update_position(current_position);
            send_coor(current_position);
        }
/*
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (resolution > 49)
            {
                resolution = resolution / 2;
                Vector3 current_position = this.transform.localPosition;
                cloud.GetComponent<cloud_mesh>().resize_view(resolution, current_position);
                send_coor(current_position);
            }

        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (resolution < 101)
            {
                resolution = resolution * 2;
                Vector3 current_position = this.transform.localPosition;
                if (current_position.y + resolution / 2 > row_num / 2)
                {
                    current_position.y = row_num / 2 - resolution / 2;
                }
                if (current_position.y - resolution / 2 < -row_num / 2)
                {
                    current_position.y = -row_num / 2 + resolution / 2;
                }
                if (current_position.x + resolution / 2 > col_num / 2)
                {
                    current_position.x = col_num / 2 - resolution / 2;
                }
                if (current_position.x - resolution / 2 < -col_num / 2)
                {
                    current_position.x = -col_num / 2 + resolution / 2;
                }
                this.transform.localPosition = current_position;
                cloud.GetComponent<cloud_mesh>().resize_view(resolution, current_position);
                send_coor(current_position);
            }
        }
        */


    }
}
