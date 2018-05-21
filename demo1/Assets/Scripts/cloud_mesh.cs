using UnityEngine;
using System;
using UnityEngine.VR.WSA;
using System.Linq;
using HoloToolkit.Unity;
using System.Collections;
using HoloToolkit.Sharing;
using System.Threading;
using HoloLensXboxController;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class cloud_mesh : MonoBehaviour
{

    public bool GotTransform { get; private set; }

    private Mesh mesh;

    float[] demArray;

    float[] demCurrentArray;

    string[] result_name;
    string[] DEM1 = { "tmp" };
    string[] DEM2 = { "DEM3/2_0", "DEM3/2_1", "DEM3/2_2", "DEM3/2_3", "DEM3/2_4",
        "DEM3/2_5", "DEM3/2_6", "DEM3/2_7", "DEM3/2_8", "DEM3/2_9",
        "DEM3/2_10", "DEM3/2_11", "DEM3/2_12", "DEM3/2_13", "DEM3/2_14"};
    string[] DEM3 = { "DEM2/3_0", "DEM2/3_1", "DEM2/3_2", "DEM2/3_3", "DEM2/3_4", "DEM2/3_5", "DEM2/3_6", "DEM2/3_7", "DEM2/3_8",
        "DEM2/3_9", "DEM2/3_10", "DEM2/3_11", "DEM2/3_12", "DEM2/3_13", "DEM2/3_14", "DEM2/3_15", "DEM2/3_16",
        "DEM2/3_17", "DEM2/3_18", "DEM2/3_19", "DEM2/3_20", "DEM2/3_21", "DEM2/3_22", "DEM2/3_23", "DEM2/3_24",
        "DEM2/3_25", "DEM2/3_26", "DEM2/3_27", "DEM2/3_28", "DEM2/3_29", "DEM2/3_30", "DEM2/3_31", "DEM2/3_32",
        "DEM2/3_33", "DEM2/3_34", "DEM2/3_35", "DEM2/3_36", "DEM2/3_37", "DEM2/3_38", "DEM2/3_39", "DEM2/3_40",
        "DEM2/3_41", "DEM2/3_42", "DEM2/3_43", "DEM2/3_44", "DEM2/3_45", "DEM2/3_46", "DEM2/3_47", "DEM2/3_48", };



    int rowNum = 380;
    int colNum = 708;
    public int showRowNum = 200;
    public int showColNum = 200;
    public bool placed =false ;

    GameObject mapview;


    public float max_level,average,min_level;
    bool is_animate = false;
    bool is_animate_done = false;

    public Vector3[] points;
    public string filename_rr = " ";


    int[] indices;
    Color[] colors;
    Texture2D img;
    Color[] colArray;

    int counter = 0;

    private ControllerInput controllerInput;

    Vector3 Current;
    TextAsset demTxt;
    void Start()
    {
        demCurrentArray = new float[showColNum * showRowNum];
        for (int i = 0; i < showColNum * showRowNum; i++) {
            demCurrentArray[i] = 0; 
        }
        controllerInput = new ControllerInput(0, 0.19f);
        is_animate = false;
    }


    void next_result() {
        GameObject mapview = GameObject.Find("map_view");
        if (Input.GetKeyDown(KeyCode.JoystickButton4)   || controllerInput.GetButtonDown(ControllerButton.RightShoulder)) {
            counter = counter + 1;
            if (counter >= result_name.Length) {
                counter = counter-1;
            }
            img = (Texture2D)Resources.Load(result_name[counter]);
            filename_rr = result_name[counter];
            Debug.Log(result_name[counter]);
            colArray = new Color[colNum * rowNum];
            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    colArray[j * colNum + i] = (img.GetPixel(i, rowNum - j));
                }
            }
            Update_position(Current);
            mapview.GetComponent<load_map>().show_image(colNum,rowNum, result_name[counter]);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5) || controllerInput.GetButtonDown(ControllerButton.LeftShoulder)) {
            counter = counter - 1;
            if (counter <= 0)
            {
                counter = 0;
            }
            img = (Texture2D)Resources.Load(result_name[counter]);
            filename_rr = result_name[counter];
            Debug.Log(result_name[counter]);
            colArray = new Color[colNum * rowNum];
            for (int i = 0; i < colNum; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    colArray[j * colNum + i] = (img.GetPixel(i, rowNum - j));
                }
            }
            Update_position(Current);
            mapview.GetComponent<load_map>().show_image(colNum, rowNum, result_name[counter]);
        }
    }






    public void display_request_received(int Width,int Height,int vid) {
        rowNum = Height;
        colNum = Width;
        demTxt = Resources.Load("DEM1/DEM") as TextAsset; 
        switch (vid) {
            case 1:
                demTxt = Resources.Load("DEM1/DEM") as TextAsset;
                result_name = DEM1;
                img = (Texture2D)Resources.Load("DEM1/tmp");
                filename_rr = "DEM1/tmp";
                break;
            case 2:
                Debug.Log("DEM3/DEM");
                demTxt = Resources.Load("DEM3/DEM") as TextAsset;
                result_name = DEM2;
                img = (Texture2D)Resources.Load("DEM3/2_0");
                filename_rr = "DEM3/2_0";
                break;
            case 3:
                demTxt = Resources.Load("DEM2/DEM") as TextAsset;
                result_name = DEM3;
                img = (Texture2D)Resources.Load("DEM2/3_0");
                filename_rr = "DEM2/3_0";
                break;
        }

        colArray = new Color[colNum * rowNum];
        for (int i = 0; i < colNum; i++)
        {
            for (int j = 0; j < rowNum; j++)
            {
                colArray[j * colNum + i] = (img.GetPixel(i, rowNum - j));
            }
        }

        demArray = ConvertByteToFloat(demTxt.bytes);


        max_level = demArray.Max();
        min_level = demArray.Min();
        float range_level = max_level - min_level;
        for (int i = 0; i < demArray.Length; i++) {
            demArray[i] = (demArray[i] - min_level) / range_level * 200;
        }

        average = demArray.Average();

        for (int i = 0; i < demArray.Length; i++)
        {
            demArray[i] = demArray[i] - average;
        }
        if (placed) {
            Update_position(new Vector3(0, 0, 0));
        }

    }




    void Update()
    {
        controllerInput.Update();

        if (placed && is_animate)
        {
            loadThirdDEM();
            GameObject center = GameObject.Find("navigators");
            center.GetComponent<navigator>().enable_movement();
            is_animate = false;
        }
        else {
            next_result();
        }








/*        if (is_animate == true)
        {

            map_animation();
        }
        if (animate_level > max_level && is_animate == true)
        {
            is_animate = false;
            loadThirdDEM();
            GameObject center = GameObject.Find("navigators");
            center.GetComponent<navigator>().enable_movement();
        }
*/
    }

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;
        return retval;
    }

  /*  public void OnSelect()
    {

        if (placed == true)
        {
            return;
        }
        else
        {

            placed = true;
            is_animate = true;
        }
        GestureManager.Instance.OverrideFocusedObject = null;
    }
    */

    public void Enable_map()
    {
        reset_map();
    }

    public void map_placed() {
        placed = true;
        is_animate = true;
        GameObject expo = GameObject.Find("expo");
        expo.GetComponent<explorer>().enable_display();
    }

    

    public void resize_view(int new_res, Vector3 position)
    {
        showRowNum = new_res;
        showColNum = new_res;

        Vector3 scales = this.transform.localScale;
        scales.x = ((float)0.15 / new_res) * 200;
        scales.y = ((float)0.15 / new_res) * 200;
        scales.z = ((float)0.15 / new_res) * 200;
        this.transform.localScale = scales;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        points = new Vector3[showRowNum * showColNum];
        indices = new int[showRowNum * showColNum];
        colors = new Color[showRowNum * showColNum];
        Update_position(position);
    }



    public void Update_position(Vector3 position)
    {
        int x = (int)position.x;
        int y = -(int)position.y;

        x = x + colNum / 2 - showRowNum / 2;
        y = y + rowNum / 2 - showColNum / 2;


        for (int i = 0; i < showRowNum; i++)
        {
            for (int j = 0; j < showColNum; j++)
            {

                int index = i * showColNum + j;
                int demIndex = (i + (int)y) * colNum + j + x;
                indices[index] = index;
                points[index] = new Vector3(showRowNum / 2 - i, demArray[demIndex], -showColNum / 2 + j);
                colors[index] = colArray[demIndex];

            }
        }
        mesh.vertices = points;
        mesh.SetIndices(indices, MeshTopology.Points, 0); // for point visualization only. 
        mesh.colors = colors;
        Current = position;
    }


    public void reset_map()
    {
        is_animate = false;

        Shader s = Shader.Find("Unlit/pointColorShader");
        GetComponent<Renderer>().material.shader = s;


        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        points = new Vector3[showRowNum * showColNum];
        indices = new int[showRowNum * showColNum];
        colors = new Color[showRowNum * showColNum];
        for (int i = 0; i < showRowNum; i++)
        {
            for (int j = 0; j < showColNum; j++)
            {
                int index = i * showColNum + j;
                indices[index] = index;
                points[index] = new Vector3(-showRowNum / 2 + i, 0, -showColNum / 2 + j);
                colors[index] = new Color(0, 0, 0);
            }
        }
        mesh.vertices = points;
        mesh.SetIndices(indices, MeshTopology.Points, 0); // for point visualization only. 
        mesh.colors = colors;
    }

    public void map_animation()
    {
        int x = 0;
        int y = 0;

        x = x + colNum / 2 - showRowNum / 2;
        y = y + rowNum / 2 - showColNum / 2;
        
        indices = new int[showRowNum * showColNum];
        for (int i = 0; i < showRowNum; i++)
        {
            for (int j = 0; j < showColNum; j++)
            {
                int index = i * showColNum + j;
                int demIndex = (i + (int)y) * colNum + j + x;
                if (demCurrentArray[index] == demArray[demIndex]) {
                    indices[index] = index;
                    points[index] = new Vector3(showRowNum / 2 - i, demCurrentArray[index], showRowNum / 2 - j);
                    continue;
                }
                else if (demCurrentArray[index] > demArray[demIndex])
                {
                    demCurrentArray[index]--;
                    if (demCurrentArray[index] <= demArray[demIndex])
                    {
                        demCurrentArray[index] = demArray[demIndex];
                    }
                }
                else if (demCurrentArray[index] < demArray[demIndex])
                {
                    demCurrentArray[index]++;
                    if (demCurrentArray[index] >= demArray[demIndex])
                    {
                        demCurrentArray[index] = demArray[demIndex];
                    }
                }
                indices[index] = index;
                points[index] = new Vector3(showRowNum / 2 - i, demCurrentArray[index], showRowNum / 2 - j);
            }
        }
        mesh.vertices = points;
        mesh.SetIndices(indices, MeshTopology.Points, 0); // for point visualization only. 
    }


    void loadThirdDEM()
    {
        Update_position(new Vector3(0, 0, 0));
    }

    public float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToSingle(array, i * 4);
        }
        return floatArr;
    }

}


