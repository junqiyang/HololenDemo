using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class mesh_placement : Singleton<mesh_placement>
{
    public bool GotTransform { get; private set; }
    // Use this for initialization
    void Start () {
        GotTransform = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GotTransform)
        {
            transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
        }
    }

    public void OnSelect() {
        GameObject cloud = GameObject.Find("cloudMeshMap");
        if (GotTransform == false)
        {
            cloud.GetComponent<cloud_mesh>().map_placed();
        }
        GestureManager.Instance.OverrideFocusedObject = null;
        GotTransform = true;
        GameObject infom = GameObject.Find("info");
        infom.GetComponent<tutorial>().System_info();

    }

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;
        return retval;
    }


}
