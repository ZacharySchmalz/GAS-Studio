using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Reflection;
using System;

public class NetworkGenericLogger : Logger {

    public NetworkIdentity identity;

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

        type = LoggerType.Generic;

        base.Start();
    }
	
	public override void Update ()
    {
        base.Update();
	}

    public override void LogValues()
    {
        if (!identity.isLocalPlayer)
            return;
        
        string temp = "";
        foreach (FieldInfo f in fieldsToLog)
        {
            temp += f.GetValue(targetScript) + ", ";
        }
        LogManager.instance.LogLine(temp);

        base.LogValues();
    }
}
