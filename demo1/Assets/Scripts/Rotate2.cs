using UnityEngine;
using System.Collections;
using HoloLensXboxController;


public class Rotate2 : MonoBehaviour {
    private ControllerInput controllerInput;
    // Use this for initialization
    void Start()
    {
        controllerInput = new ControllerInput(0, 0.19f);
    }

    // Update is called once per frame
    void Update()
    {
        controllerInput.Update();
        float rotation_z = this.transform.eulerAngles.z;
        float rotation_y = this.transform.eulerAngles.y;
        float rotation_x = this.transform.eulerAngles.x;

        if (Input.GetAxis("rotate_v") < 0 || controllerInput.GetAxisRightThumbstickY() < 0)
        {

            rotation_x = rotation_x + 1;
            if (rotation_x >= 358)
            {
                rotation_x = 358;
            }
            this.transform.rotation = Quaternion.Euler(rotation_x, rotation_y, rotation_z);
        }
        if (Input.GetAxis("rotate_v") > 0 || controllerInput.GetAxisRightThumbstickY() > 0)
        {
            rotation_x = rotation_x - 1;
            if (rotation_x < 270)
            {
                rotation_x = 270;
            }
            this.transform.rotation = Quaternion.Euler(rotation_x, rotation_y, rotation_z);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton9) || (controllerInput.GetButton(ControllerButton.RightThumbstick)))
        {
           // this.transform.rotation = Quaternion.Euler(0, 0, 0);
            GameObject mesh = GameObject.Find("cloudMeshMap");
            mesh.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }



    }
}
