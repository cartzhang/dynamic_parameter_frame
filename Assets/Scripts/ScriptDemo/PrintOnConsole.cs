using UnityEngine;
using System.Collections;

public class PrintOnConsole : MonoBehaviour {

    private string output = "this is print test,not use Debug.Log()";
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.P))
        {
            this.ConsolePrint(output);
            //Debug.Log(output);
        }

    }
}
