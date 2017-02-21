using UnityEngine;
using System.Collections;

public class ColorAttriTest : MonoBehaviour
{
    [ModifyAttribute]
    public int clolor;

    [ModifyAttribute]
    public string attack;

    [ModifyAttribute]
    public bool destroymode;

    private Material material = null;
	// Use this for initialization
	void Start ()
    {   
        if (null != material)
        {
            material.color = Color.cyan;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	
	}
}
