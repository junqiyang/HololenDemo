using UnityEngine;
using System.Collections;
using HoloLensXboxController;
public class tutorial : MonoBehaviour {
    private ControllerInput controllerInput;
    bool tt;
    bool on = true;

    // Use this for initialization
    void Start () {
        this.GetComponent<TextMesh>().text = "Loading.............";
        controllerInput = new ControllerInput(0, 0.19f);
        tt = false;
    }
	
	// Update is called once per frame
	void Update () {
        controllerInput.Update();
        //transform.position = new Vector3(Camera.main.transform.position.x - (float)0.3 , Camera.main.transform.position.y + (float)0.2 , Camera.main.transform.position.z + 2);
        if (controllerInput.GetButton(ControllerButton.Menu) && tt == true){
            on = !on;
            if (on)
            {
                System_info();
            }
            else if (!on) {
                this.GetComponent<TextMesh>().text = "";
            }
        }
    }


    public void download_complete() {
        this.GetComponent<TextMesh>().text = "First place your map by click in the air";
    }

    public void map_placed()
    {
        this.GetComponent<TextMesh>().text = "Second place your 3D terrian by click in the air";
        tt = true;
    }

    public void System_info()
    {
        this.GetComponent<TextMesh>().text = " Using left Joystick to move\n Using right Joystick to rotate\n Using LB/RB to switch Result\n Using D-pad to move navigator\n Close/Open this Tutorial by meun button";
    }


}
