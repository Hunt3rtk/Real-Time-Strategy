using System.Collections.Generic;
using UnityEngine;

public class Viewport : MonoBehaviour
{

    public Plane plane;

    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;


    private Vector3[] positions;

    private LineRenderer viewport;

    void Awake()
    {
        viewport = GetComponent<LineRenderer>();
        positions = new Vector3[4];
    }

    // Update is called once per frame
    void Update() {

        Ray tl = Camera.main.ScreenPointToRay(new Vector2(0, Screen.height));
        float distancetl = 0;
        Physics.Raycast(tl, distancetl);
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, distancetl));

        Ray tr = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
        float distancetr = 0;
        Physics.Raycast(tr, distancetr);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distancetr));

        Ray br= Camera.main.ScreenPointToRay(new Vector2(Screen.width, 0));
        float distancebr = 0;
        Physics.Raycast(br, distancebr);
        bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, distancebr));

        Ray bl= Camera.main.ScreenPointToRay(new Vector2(0, 0));
        float distancebl = 0;
        Physics.Raycast(bl, distancebl);
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distancebl));

        topLeft.z = topLeft.y;
        topRight.z = topRight.y;
        bottomRight.z = bottomRight.y;
        bottomLeft.z = bottomLeft.y;

        topLeft.y = 1;
        topRight.y = 1;
        bottomRight.y = 1;
        bottomLeft.y = 1;
        

        positions[0] = topLeft;
        positions[1] = topRight;
        positions[2] = bottomRight;
        positions[3] = bottomLeft;


        Debug.Log(topLeft);
        Debug.Log(topRight);
        Debug.Log(bottomRight);
        Debug.Log(bottomLeft);

        viewport.SetPosition(0, positions[0]);
        viewport.SetPosition(1, positions[1]);
        viewport.SetPosition(2, positions[2]);
        viewport.SetPosition(3, positions[3]);
    }
}
