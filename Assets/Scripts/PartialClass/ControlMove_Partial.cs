// H:/Unity/Project/DynamicParaFrame/Assets/Scripts/PartialClass
// ControlMove
// @cartzhang 

using UnityEngine;
using SLQJ;

public partial class ControlMove
{ 

// ControlMove_movespeed
private void ControlMove_movespeed(SLQJ.MessageObject obj)
{
movespeed = (float)obj.MsgValue;
}


// add auto regist 
private void Start4AutoSubscribe()
{
NotificationManager.Instance.Subscribe("CONTROLMOVE_MOVESPEED", ControlMove_movespeed);

Debug.Log("partical class start to regist");
}

//auto create partical code end.
} 


