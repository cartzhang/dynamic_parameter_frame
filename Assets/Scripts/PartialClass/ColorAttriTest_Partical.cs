// H:/Unity/Project/TestConsoleWindow/UnityConsoleWindow/Assets/Scripts/PartialClass
// ColorAttriTest
// @cartzhang 

using UnityEngine;
using SLQJ;

public partial class ColorAttriTest
{ 

// ColorAttriTest_clolor
private void ColorAttriTest_clolor(SLQJ.MessageObject obj)
{
clolor = (int)obj.MsgValue;
}

// ColorAttriTest_attack
private void ColorAttriTest_attack(SLQJ.MessageObject obj)
{
attack = (string)obj.MsgValue;
}

// ColorAttriTest_destroymode
private void ColorAttriTest_destroymode(SLQJ.MessageObject obj)
{
destroymode = (bool)obj.MsgValue;
}


// add auto regist 
private void Start4AutoSubscribe()
{
NotificationManager.Instance.Subscribe("COLORATTRITEST_CLOLOR", ColorAttriTest_clolor);
NotificationManager.Instance.Subscribe("COLORATTRITEST_ATTACK", ColorAttriTest_attack);
NotificationManager.Instance.Subscribe("COLORATTRITEST_DESTROYMODE", ColorAttriTest_destroymode);

Debug.Log("partical class start to regist");
}

//auto create partical code end.
} 


