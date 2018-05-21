using UnityEngine;
using System.Collections;

public class face_camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        var fwd = Camera.main.transform.forward;
        fwd.y = 0;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
