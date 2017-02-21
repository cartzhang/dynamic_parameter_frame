// H:/Unity/Project/TestConsoleWindow/UnityConsoleWindow/Assets/Scripts/PartialClass
// MoveAttriTest
// @cartzhang 

using UnityEngine;
using SLQJ;

public partial class MoveAttriTest
{ 

// MoveAttriTest_movespeed
private void MoveAttriTest_movespeed(SLQJ.MessageObject obj)
{
movespeed = (float)obj.MsgValue;
}

// MoveAttriTest_number
private void MoveAttriTest_number(SLQJ.MessageObject obj)
{
number = (float)obj.MsgValue;
}


// add auto regist 
private void Start4AutoSubscribe()
{
NotificationManager.Instance.Subscribe("MOVEATTRITEST_MOVESPEED", MoveAttriTest_movespeed);
NotificationManager.Instance.Subscribe("MOVEATTRITEST_NUMBER", MoveAttriTest_number);

Debug.Log("partical class start to regist");
}

//auto create partical code end.
} 


