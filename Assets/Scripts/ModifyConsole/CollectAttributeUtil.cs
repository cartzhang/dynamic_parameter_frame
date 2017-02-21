using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AttributeForClass
{
    public string class_variable;
    public System.Type variable_type;
}

public class CollectAttributeUtil
{
    // class name add varible name as key,and same name as methodinfor.
    public static SortedList myFuctionList = new SortedList();

    public static void InitialCollectModifyAttribute()
    {
        myFuctionList.Clear();
        MonoBehaviour[] AllMonoScripts = GetMonoScriptOfType<MonoBehaviour>();
        GetAllDestByProperties<ModifyAttribute>(AllMonoScripts);
        Debug.Log(myFuctionList.Count + " class needs to be modified in list." );
    }
    /// <summary>
    /// clear list content.
    /// </summary>
    public static void ClearCollection()
    {
        myFuctionList.Clear();
    }

    public static bool IsVarialbeInList(string classname,string classname_variableName, out string valuetype)
    {
        bool bresult = false;
        string tmpclassName = classname;
        Debug.Assert(!string.IsNullOrEmpty(tmpclassName));
        Debug.Assert(!string.IsNullOrEmpty(classname_variableName));
        valuetype = null;
        if (CollectAttributeUtil.myFuctionList.ContainsKey(tmpclassName))
        {
            List<AttributeForClass> mlist = (List<AttributeForClass>)myFuctionList[tmpclassName];
            for (int i = 0; i < mlist.Count; i++)
            {
                if (mlist[i] != null &&
                    string.Compare( mlist[i].class_variable ,classname_variableName,true) == 0)
                {   
                    valuetype = CheckVariableType(mlist[i].variable_type);
                    bresult = true;
                    break;
                }
            }
        }
        return bresult;
    }

    public static string CheckVariableType(System.Type variableType)
    {
        string temp = variableType.ToString();
        if (string.Equals(temp, "System.Int32"))
        {
            return "int";
        }
        else if (string.Equals(temp, "System.Single"))
        {
            return "float";
        }
        else if (string.Equals(temp, "System.String"))
        {
            return "string";
        }
        else if (string.Equals(temp, "System.Boolean"))
        {
            return "bool";
        }
        return temp;
    }

    private static MonoBehaviour[] GetMonoScriptOfType<T>()
    {
        // current scripts in current scene;
        MonoBehaviour[] Monoscripts = (MonoBehaviour[])GameObject.FindObjectsOfType<MonoBehaviour>();
        return Monoscripts;
    }

    public static void GetAllDestByProperties<T>(object[] mono) where T : ModifyAttribute
    {
        int length = mono.Length;
        for (int i = 0; i < length; i++)
        {
            CollectAttributeProperty<T>((MonoBehaviour)mono[i]);
        }
    }
    /// <summary>
    /// get attribute type variable in each monobehaviour class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="p"></param>
    private static void CollectAttributeProperty<T>(MonoBehaviour p) where T : ModifyAttribute
    {
        var flags = BindingFlags.Instance  | BindingFlags.Public | BindingFlags.Static;
        var type = p.GetType();
        FieldInfo[] fieldInfor = type.GetFields(flags);
        foreach (var field in fieldInfor)
        {
            var objs = field.GetCustomAttributes(typeof(T), false);
            if (objs.Length > 0)
            {
                string keyTmp = type.FullName;
                AttributeForClass attri4Class = new AttributeForClass();
                attri4Class.class_variable = (type.FullName + "#" + field.Name);
                attri4Class.variable_type = field.FieldType;
                if (!myFuctionList.Contains(keyTmp))
                {
                    List<AttributeForClass> AttriClassList = new List<AttributeForClass>();
                    AttriClassList.Add(attri4Class);
                    myFuctionList.Add(keyTmp, AttriClassList);
                }
                else
                {
                    List<AttributeForClass> mlist = (List<AttributeForClass>)myFuctionList[keyTmp];
                    mlist.Add(attri4Class);
                }
                //Debug.Log(type.ToString() + " current varible is  " + field.Name + " type is " + field.FieldType);
            }
        }
    }
}
