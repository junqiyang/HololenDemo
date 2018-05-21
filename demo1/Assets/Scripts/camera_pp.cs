using UnityEngine;
using System.Collections;

public class camera_pp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
	}

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;
        return retval;
    }
}
