using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class load_map : MonoBehaviour {
    RawImage img = null;
    public int img_row = 308;
    public int img_col = 780;
    public int res;
    
    void Start()
    {

    }

    public void show_image(int image_r, int image_c,string filename) {
        img = gameObject.GetComponent<RawImage>();
        Texture2D resource = (Texture2D)Resources.Load(filename);
        GetComponent<RectTransform>().sizeDelta = new Vector2(image_r, image_c);
        img.texture = resource;
    }

    void Update()
    {

    }
}
