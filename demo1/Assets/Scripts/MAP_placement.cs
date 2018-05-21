using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class MAP_placement : Singleton<MAP_placement> {
    public bool GotTransform { get; private set; }
    // Use this for initialization
    void Start () {
        GotTransform = false;
        GestureManager.Instance.OverrideFocusedObject = this.gameObject;
    }

    void Update()
    {
        if (!GotTransform)
        {
            transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
        }
    }

    public void ResetStage()
    {
        GotTransform = false;
        AppStateManager.Instance.ResetStage();
        GestureManager.Instance.OverrideFocusedObject = this.gameObject;
    }


    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 5;
        return retval;
    }

    public void OnSelect() {
        GameObject cloud = GameObject.Find("cloudMeshMap");
        GameObject mesh = GameObject.Find("Mesh_holder");
        if (GotTransform == false)
        {
            cloud.GetComponent<cloud_mesh>().Enable_map();
        }
        GestureManager.Instance.OverrideFocusedObject = mesh;
        GotTransform = true;
        GameObject infom = GameObject.Find("info");
        infom.GetComponent<tutorial>().map_placed();

    }


}
