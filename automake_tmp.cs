using UnityEngine;
using System.Collections;

public partial class MoveAttriTest : MonoBehaviour
{
    [ModifyAttribute]
    public float movespeed = 10f;

    public float shakeRite = 6.6f;
    [ModifyAttribute]
    public static float number = 2;

    private static bool isok = false;

    private Transform transf;
    private float horizontalDirection;
    private float verticalDirection;

    // Use this for initialization
    void Start()
    {
        // auto regist code. @cz
        Start4AutoSubscribe(); 
        transf = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalDirection = 0;
        verticalDirection = 0;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalDirection = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalDirection = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            verticalDirection = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            verticalDirection = 1;
        }

        transf.position += transf.right * Time.deltaTime * movespeed * horizontalDirection;
        transf.position += transf.up * Time.deltaTime * movespeed * verticalDirection;
    }
}


