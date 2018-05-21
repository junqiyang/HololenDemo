using UnityEngine;
using System.Collections;
using HoloLensXboxController;

public class Rotate : MonoBehaviour {
    private ControllerInput controllerInput;
    // Use this for initialization
    void Start () {
        controllerInput = new ControllerInput(0, 0.19f);
    }
	
	// Update is called once per frame
	void Update () {
        controllerInput.Update();
        float rotation_z = this.transform.localEulerAngles.z;
        float rotation_y = this.transform.localEulerAngles.y;
        float rotation_x = this.transform.localEulerAngles.x;
       // Debug.Log(rotation_y);
        if (Input.GetAxis("rotate_h") < 0 || controllerInput.GetAxisRightThumbstickX() < 0)
         {

             rotation_y = rotation_y + 1;
             if (rotation_y > 360)
             {
                 rotation_y =0;
             }
             this.transform.localRotation = Quaternion.Euler(rotation_x, rotation_y, rotation_z);
         }
         if (Input.GetAxis("rotate_h") > 0  || controllerInput.GetAxisRightThumbstickX() > 0)
         {
             if (rotation_y < 2)
             {
                 rotation_y = 360;
             }
             rotation_y = rotation_y - 1;

             this.transform.localRotation = Quaternion.Euler(rotation_x, rotation_y, rotation_z);
         }
       

    }   
}
