#define UNITY_EDITOR_WIN
#define UNITY_STANDALONE_WIN
using UnityEngine;
using System.Collections;
using SLQJ;

public class ServerConsole : MonoBehaviour
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    private ConsoleTestWindows.ConsoleWindow console = null;
    private ConsoleTestWindows.ConsoleInput input = null;

    private static bool ishowWindow = false;
    private bool oldWindowState = false;
	//
	// Create console window, register callbacks
	//
	void Awake() 
	{
		DontDestroyOnLoad( gameObject );
        CollectAttributeUtil.InitialCollectModifyAttribute();
        Debug.Log( "Console Started" );
	}
 
	//
	// Text has been entered into the console
	// Run it as a console command
	//
	void OnInputText( string obj )
	{
        this.ConsolePrint(obj);        
        if (!NotifyToChangeVarialbe(obj))
        {
            this.ConsolePrint("not correct input to change variable");
        }
    }
 
	//
	// Debug.Log* callback
	//
	void HandleLog( string message, string stackTrace, LogType type )
	{
        if (type == LogType.Warning)
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
        else if (type == LogType.Error)
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        else
            System.Console.ForegroundColor = System.ConsoleColor.White;
 
		// We're half way through typing something, so clear this line ..
        if (System.Console.CursorLeft != 0)
			input.ClearLine();
 
		System.Console.WriteLine( message );
 
		// If we were typing something re-add it.
		input.RedrawInputLine();
	}

    //
    // Update the input every frame
    // This gets new key input and calls the OnInputText callback
    //
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            ishowWindow = !ishowWindow;
            if (ishowWindow)
            {
                console = new ConsoleTestWindows.ConsoleWindow();
                input = new ConsoleTestWindows.ConsoleInput();
                console.Initialize();
                console.SetTitle("Test command");
                input.OnInputText += OnInputText;
                Application.logMessageReceived += HandleLog;
            }
            else
            {
                CloseConsoleWindow();
            }
            oldWindowState = ishowWindow;
        }
        // input update
        if (ishowWindow && null != input)
        {
            input.Update();
        }

        if (ishowWindow != oldWindowState && !ishowWindow)
        {
            CloseConsoleWindow();
        }
        oldWindowState = ishowWindow;
    }
 
	//
	// It's important to call console.ShutDown in OnDestroy
	// because compiling will error out in the editor if you don't
	// because we redirected output. This sets it back to normal.
	//
	void OnDestroy()
	{
        CloseConsoleWindow();
    }

    void CloseConsoleWindow()
    {
        if (console != null)
        {
            console.Shutdown();
            console = null;
            input = null;
        }
    }
    // control by other .
    public static void SetIshowWindow(bool flag)
    {   
       ishowWindow = flag;
    }

    private bool NotifyToChangeVarialbe(string input)
    {
        bool bresult = false;
        string[] splits = input.Split(' ');
        if (splits.Length !=  3)
            return bresult;

        string tempValue = null;
        string messageInfor = (splits[0] + "_" + splits[1]).ToUpper();
        bresult = CollectAttributeUtil.IsVarialbeInList(splits[0], (splits[0] + "#" + splits[1]), out tempValue);
        if (bresult)
        {
            string getvaluve = splits[2];
            object variable = new object();
            if("int" == tempValue)
            {   
                int tempvia = -1;
                int.TryParse(getvaluve, out tempvia);
                variable = tempvia;
            }
            if ("float" == tempValue)
            {
                float tempvia = -1;
                float.TryParse(getvaluve, out tempvia);
                variable = tempvia;
            }
            if ("string" == tempValue)
            {   
                variable = getvaluve;
            }
            if ("bool" == tempValue)
            {
                bool tempvia = false;
                bool.TryParse(getvaluve, out tempvia);
                variable = tempvia;
            }
            NotificationManager.Instance.Notify(messageInfor, variable);
        }

        return bresult;
    }

#endif
}

public static class ExtendDebugClass
{
    public static void ConsolePrint(this MonoBehaviour mono, string message)
    {
        if (message.Length < 0) return;
        System.Console.WriteLine(message);
    }
}
