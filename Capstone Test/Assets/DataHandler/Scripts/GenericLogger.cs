using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class GenericLogger : Logger {

    public string scriptToLog;
    public string[] valuesToLog;

    private List<FieldInfo> fieldsToLog;
    private Type t;
    private FieldInfo[] scriptFields;
    private Component targetScript;

    public override void Start ()
    {
        fieldsToLog = new List<FieldInfo>();

        t = Type.GetType(scriptToLog);

        if (t != null)
        {
            targetScript = GetComponent(t);
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

        //foreach (FieldInfo f in fieldsToLog)
        //{
        //    Debug.Log(f.Name + " " + f.GetValue(targetScript));
        //}

        type = LoggerType.Generic;
        base.Start();
    }
	
	public override void Update ()
    {
        base.Update();
	}

    public override void LogValues()
    {
        foreach (FieldInfo f in fieldsToLog)
        {
            //Debug.Log(f.Name + " " + f.GetValue(targetScript));
            LogManager.instance.Log(string.Format("{0} {1}: {2}", gameObjectName, f.Name, f.GetValue(targetScript)));
        }

        base.LogValues();
    }
}
