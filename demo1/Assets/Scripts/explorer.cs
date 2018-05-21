using UnityEngine;
using System.Collections;
using HoloLensXboxController;

public class explorer : MonoBehaviour {
    public Vector3[] height;
    int res_c;
    int res_r;
    int res;
    float lat, log, pix;
    string file_name;
    float p_x = 0, p_z = 0, average;
    GameObject cloud;
    GameObject navi;
    GameObject Info_dis;

    bool en_dis = false;

    private ControllerInput controllerInput;
    // Use this for initialization
    void Start () {
        controllerInput = new ControllerInput(0, 0.19f); 
        cloud = GameObject.Find("cloudMeshMap");
        navi = GameObject.Find("navigators");
        Info_dis = GameObject.Find("Info_display");
        en_dis = false;
    }

    public void enable_display() {
        en_dis = true;
    }

	
	// Update is called once per frame
	void Update () {
        controllerInput.Update();
        
        cloud_mesh mesh = cloud.GetComponent<cloud_mesh>();
        navigator naviss = navi.GetComponent<navigator>();
        height = mesh.points;        
        res = mesh.showColNum;
        average = mesh.average;
        lat = naviss.get_lat;
        log = naviss.get_log;
        pix = naviss.get_Pix;
        file_name = mesh.filename_rr;




        if (Input.GetAxis("explore_h") == -1 || controllerInput.GetButton(ControllerButton.DPadRight))
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.z++;


            if (current_position.z < 100)
            {
                p_x = current_position.x;
                p_z = current_position.z;
                int x = (int)p_x + res / 2;
                int z = (int)p_z + res / 2;
                int index = (z) + (res - x) * res;
            //    Debug.Log("Test:");
            //    Debug.Log(height[index]);
                this.transform.localPosition = new Vector3(p_x, height[index].y, p_z);
                Debug.Log(this.transform.localPosition);
            }
        }
        else if (Input.GetAxis("explore_h") == 1 || controllerInput.GetButton(ControllerButton.DPadLeft))
        {
            Vector3 current_position = this.transform.localPosition;
            Debug.Log(current_position);
            current_position.z--;
            if (current_position.z > -100)
            {
                p_x = current_position.x;
                p_z = current_position.z;
                int x = (int)p_x + res / 2;
                int z = (int)p_z + res / 2;
                int index = (z) + (res - x) * res;
           //     Debug.Log("Test:");
           //     Debug.Log(height[index]);
                this.transform.localPosition = new Vector3(p_x, height[index].y, p_z);
               
           //     Debug.Log(this.transform.localPosition);
            }
        }

        else if (Input.GetAxis("explore_v") == 1 || controllerInput.GetButton(ControllerButton.DPadDown))
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.x++;
            if (current_position.x < 100)
            {
                p_x = current_position.x;
                p_z = current_position.z;
                int x = (int)p_x + res / 2;
                int z = (int)p_z + res / 2;
                int index = ( z) + (res - x) * res;
          //      Debug.Log("Test:");
          //      Debug.Log(height[index]);
                this.transform.localPosition = new Vector3(p_x, height[index].y, p_z);
           //     Debug.Log(this.transform.localPosition);
            }
        }
        else if (Input.GetAxis("explore_v") == -1 || controllerInput.GetButton(ControllerButton.DPadUp))
        {
            Vector3 current_position = this.transform.localPosition;
            current_position.x--;
            if (current_position.x > -100)
            {
                p_x = current_position.x;
                p_z = current_position.z;
                int x = (int)p_x + res / 2;
                int z = (int)p_z + res / 2;
                int index = ( z) + (res - x) * res;
                Debug.Log("Test:");
                Debug.Log(height[index]);
                this.transform.localPosition = new Vector3(p_x, height[index].y, p_z);
                Debug.Log(this.transform.localPosition);
            }
        }
        else if(height.Length != 0) {
            Vector3 current_position = this.transform.localPosition;
            p_x = current_position.x;
            p_z = current_position.z;
            int x = (int)p_x + res / 2;
            int z = (int)p_z + res / 2;
            int index = (z) + (res - x) * res;

        //    Debug.Log(height[index]);
            this.transform.localPosition = new Vector3(p_x, height[index].y, p_z);
         //   Debug.Log(this.transform.localPosition);
        }
        if (en_dis) {

            float dis_hei = this.transform.localPosition.y + average;
            float n_lat = lat + this.transform.localPosition.x * pix;
            float n_log = log + this.transform.localPosition.z * pix;
            Info_dis.GetComponent<TextMesh>().text = "Latitude: " + n_lat + "\n" + "Logtitude: " + n_log + "\n" + "Normalized Height(0 - 200 pixel unit): " + dis_hei + "\n" + "Current Result File: " + file_name;


        }

    }
}
