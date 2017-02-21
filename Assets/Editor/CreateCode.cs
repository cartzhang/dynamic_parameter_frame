using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
public class CreateCode : MonoBehaviour
{
    private static Dictionary<string, string> scriptsList = new Dictionary<string,string>();
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("ModifyCode/AutoModifyClass")]
    private static void AutoModify()
    {
        Debug.Log("auto modify file and class...");
        CollectAttributeUtil.InitialCollectModifyAttribute();
        CreateParticalClass();
        ChangOrginalClass();
    }

    [MenuItem("ModifyCode/ClearCode")]
    private static void ClearCode()
    {
        Debug.Log("Clear all modification attribute properties ...");
        CollectAttributeUtil.ClearCollection();
    }
        
    /// <summary>
    /// create partical for regist infor.
    /// </summary>
    private static void CreateParticalClass()
    {
        // create file folder.
        string filePath = GetFilePath();

        for (int i = 0; i < CollectAttributeUtil.myFuctionList.Count; i++)
        {
            string classname = CollectAttributeUtil.myFuctionList.GetKey(i) as string;
            if (string.IsNullOrEmpty(classname))
            {
                continue;
            }
            //1. create file in folder.
            string filefullpath = CreateParticalFile(filePath, classname);
            //2. creat a class in file.
            CreatClassFromName(filefullpath, classname);

            List<AttributeForClass> mlist = (List<AttributeForClass>)CollectAttributeUtil.myFuctionList.GetByIndex(i);
            AttributeForClass[] testClass_viriable = mlist.ToArray();
            //3. add function in file.
            AddFunctionToClass(filefullpath, testClass_viriable);
        }
    }
        
    /// <summary>
    /// change orginal code  class to paritcal class.
    /// </summary>
    private static void ChangOrginalClass()
    {
        // 1.find all the scripts.        
        CollectAllScripts(ref scriptsList);
        if (scriptsList.Count <= 0)
            return;
         
        //
        for (int i = 0; i < CollectAttributeUtil.myFuctionList.Count; i++)
        {
            string classname = CollectAttributeUtil.myFuctionList.GetKey(i) as string;
            if (string.IsNullOrEmpty(classname))
            {
                continue;
            }
            //2. find orginal file in folder.
            string filefullpath = FindOrginalClassFile(classname);
            Debug.Assert(!string.IsNullOrEmpty(filefullpath));
            //3. change the class in file.
            ChangeTheClassAsPartial(filefullpath,classname);
        }
    }

    #region  Create Partial code 
    /// <summary>
    /// get partical define  file path.
    /// </summary>
    /// <returns></returns>
    private static string GetFilePath()
    {
        string filePath = Application.dataPath + "/Scripts/PartialClass";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        return filePath;
    }
    /// <summary>
    /// create .cs file and add head using.
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="classname"></param>
    /// <returns></returns>
    private static string CreateParticalFile(string filepath,string classname)
    {
        if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(classname))
        {
            return "";
        }

        string fileFullPath = filepath + "/" + classname + "_Partial.cs";
        FileStream fileStream = null;
        if (!File.Exists(fileFullPath))
        {
            fileStream = File.Create(fileFullPath);
            fileStream.Close();
        }
        else
        {
            File.Delete(fileFullPath);
        }

        using (StreamWriter writer = new StreamWriter(fileFullPath, true))
        {
            // write file head information.
            StringBuilder output = new StringBuilder();
            output.Append("// " + filepath + "\n");
            output.Append("// " + classname + "\n");
            output.Append("// @cartzhang \n\n");
            output.Append("using UnityEngine;\n");
            output.Append("using SLQJ;\n");
            writer.WriteLine(output.ToString());
            writer.Close();
        }
        return fileFullPath;
    }

    private static void CreatClassFromName(string filefullpath,string classsname)
    {
        using (StreamWriter writer = new StreamWriter(filefullpath, true))
        {
            // write file head information.
            StringBuilder output = new StringBuilder();
            string classdefine = "public partial class " + classsname;
            output.Append(classdefine + "\n");
            output.Append("{ \n");
            writer.WriteLine(output.ToString());
            writer.Close();
        }
    }

    private static void AddFunctionToClass(string filefullPath, AttributeForClass[] ClassName_VariableList)
    {
        if (string.IsNullOrEmpty(filefullPath) || ClassName_VariableList.Length <= 0)
            return;

        const char flagbit = '#';
        string ClassName_Variable = "";
        int hashIndex = -1;
        string variable;
        string collectSubscribeReference = string.Empty;
        int ListLength = ClassName_VariableList.Length;
        string tempVarialbeType = string.Empty;
        using (StreamWriter writer = new StreamWriter(filefullPath, true))
        {
            //1. need regist function.
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < ListLength; i++)
            {
                ClassName_Variable = ClassName_VariableList[i].class_variable;
                if (string.IsNullOrEmpty(ClassName_Variable) || !ClassName_Variable.Contains("#"))
                    continue;
                //
                hashIndex = ClassName_Variable.IndexOf(flagbit);
                variable = ClassName_Variable.Substring(hashIndex + 1);
                //Debug.Log(" varible name is " + ClassName_Variable);
                ClassName_Variable = ClassName_Variable.Replace(flagbit, '_');
                collectSubscribeReference += "#" + ClassName_Variable;

                // write file head information.
                output.Append("// " + ClassName_Variable + "\n");
                // message funtion.
                output.Append("private void " + ClassName_Variable + "(SLQJ.MessageObject obj)\n");
                output.Append("{\n");
                // accroding to the type to change.
                output.Append(variable + " = ");
                tempVarialbeType = CollectAttributeUtil.CheckVariableType(ClassName_VariableList[i].variable_type);
                output.Append("(" + tempVarialbeType + ")" + "obj.MsgValue;\n");
                output.Append("}\n\n");
            }
            writer.WriteLine(output.ToString());
            output.Remove(0, output.Length);
                        
            //2. write file regist information.
            string[] registStringList = collectSubscribeReference.Split(flagbit);
            int registLength = registStringList.Length;
            output.Append("// add auto regist \n");
            // regist function
            output.Append("private void " + "Start4AutoSubscribe()\n");
            output.Append("{\n");
            for (int j = 1; j < registLength; j++)
            {
                output.Append("NotificationManager.Instance.Subscribe(" + "\"" + registStringList[j].ToUpper() + "\", "
                    + registStringList[j] + ");\n");
            }
            output.Append("\nDebug.Log(\"partical class start to regist\");\n");
            output.Append("}\n");

            //3. complete the class
            output.Append("\n//auto create partical code end.\n} \n\n");
            writer.WriteLine(output.ToString());
            writer.Close();
        }
    }
    #endregion

    #region Modify orginal class
    private static void CollectAllScripts(ref Dictionary<string,string> ScriptsList)
    {
        ScriptsList.Clear();
        string tmpClassName = "";
        string assetPath = string.Empty;
        int flagIndex = -1;
        string[] scriptsGuids = AssetDatabase.FindAssets("t:Script");
        for (int i = 0; i < scriptsGuids.Length; i++)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(scriptsGuids[i]);
            if (assetPath.Contains("/Editor/") || assetPath.Contains("/Plugins/"))
                continue;
            
            flagIndex = assetPath.LastIndexOf('/');
            tmpClassName = assetPath.Substring(flagIndex + 1, assetPath.Length - flagIndex - 4);
            if (!scriptsList.ContainsKey(tmpClassName))
            {
                scriptsList.Add(tmpClassName, assetPath);
            }
            else
            {
                Debug.LogError(tmpClassName + " scritps have same name in current project");
            }
        }

        return;
    }


    private static string FindOrginalClassFile(string classname)
    {
        string classfile = string.Empty;        
        if (scriptsList.ContainsKey(classname))
        {
            scriptsList.TryGetValue(classname,out classfile);
        }
        return classfile;
    }
    
    private static void ChangeTheClassAsPartial(string filefullpath,string classname)
    {
        bool isReWrite = false;

        // 1.open file read file and find the line write to file.
        string sourceFile = filefullpath;        
        string tempFile = "automake_tmp.cs";
        string noteInfor = "// auto regist code. @cz";

        //2. Read the appropriate line from the file.
        string lineToWrite = string.Empty;
        string blankSpace = string.Empty;
        string tmpstring = string.Empty;
        string TmpLine = string.Empty;
        using (StreamReader reader = new StreamReader(sourceFile))
        using (StreamWriter writer = new StreamWriter(tempFile))
        {   
            while (!reader.EndOfStream)
            {
                lineToWrite = string.Empty;
                lineToWrite = reader.ReadLine();

                TmpLine = lineToWrite.ToLower();
                TmpLine = TmpLine.Replace(" ", "");
                // 
                string lowerCompaer = "class" + classname.ToLower() + ":monobehaviour";
                if (TmpLine.Contains(lowerCompaer) && !lineToWrite.Contains(" partial "))
                {
                    int maohaoIndex = lineToWrite.LastIndexOf(':');
                    string inheritClassName = lineToWrite.Substring(maohaoIndex+1);
                    lineToWrite = "public partial class " + classname + " :" + inheritClassName;
                }

                // start()  or start(){
                if (TmpLine.Contains("start()"))
                {
                    // write start()
                    writer.WriteLine(lineToWrite);
                    // write {
                    TmpLine = reader.ReadLine();
                    if (TmpLine.Trim() == "{")
                    {
                        writer.WriteLine(TmpLine);
                    }
                    // read //auto 判断是否已经加入注册代码。
                    lineToWrite = reader.ReadLine();
                    if (lineToWrite.Trim() == noteInfor.Trim())
                    {
                        
                    }
                    else
                    {   
                        isReWrite = true;
                        // write note and add start4auoto
                        for (int i = 0; i < TmpLine.Length + 3; i++)
                        {
                            blankSpace += " ";
                        }
                        tmpstring = blankSpace + noteInfor + "\n"+ blankSpace;
                        tmpstring += "Start4AutoSubscribe(); ";
                        writer.WriteLine(tmpstring);
                    }
                }
                writer.WriteLine(lineToWrite);
            }
            //3. close file
            reader.Close();
            writer.Close();
        }

        // save old code as hidden file.and copy the new code file.
        if (isReWrite)
        {
            sourceFile = sourceFile.Substring(0, sourceFile.LastIndexOf('/') + 1);
            sourceFile += "." + classname + ".cs";
            FileStream filestream = File.Create(sourceFile);
            filestream.Close();
            File.Copy(filefullpath, sourceFile, true);
        }
        File.Copy(tempFile, filefullpath, true);
        
        return;
    }
    #endregion
}
#endif
