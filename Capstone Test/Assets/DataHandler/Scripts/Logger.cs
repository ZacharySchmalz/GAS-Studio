using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class Logger : MonoBehaviour {

    public string scriptToLog;
    public string[] valuesToLog;
    public Component[] scriptComponents;

    private List<FieldInfo> fieldsToLog;
    private Type t;
    private FieldInfo[] scriptFields;
    private Component targetScript;
	// Use this for initialization
	void Start () {

        scriptComponents = GetComponents<Component>();
        fieldsToLog = new List<FieldInfo>();

        foreach (Component c in scriptComponents)
        {
            Debug.Log("components in game: " + c.GetType());
        }

        t = Type.GetType(scriptToLog);
        
        if (t != null)
        {
            targetScript = GetComponent(t);
            Debug.Log("Target Script: " + targetScript);
            scriptFields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
                | BindingFlags.Public);

            foreach (FieldInfo info in scriptFields)
            {
                for (int i = 0; i < valuesToLog.Length; i++)
                {
                    if (valuesToLog[i] == info.Name)
                    {
                        fieldsToLog.Add(info);
                    }
                }
            }
        }

        Debug.Log("Type gotten: " + t);
        Debug.Log("--About to log--");
        foreach (FieldInfo f in fieldsToLog)
        {
            Debug.Log(f.Name + " " + f.GetValue(targetScript));
        }

	}
	
	// Update is called once per frame
	void Update () 
    {
        foreach (FieldInfo f in fieldsToLog)
        {
            Debug.Log(f.Name + " " + f.GetValue(targetScript));
            LogManager.instance.Log(string.Format("{0}: {1}", f.Name, f.GetValue(targetScript)));
        }
	}
}
