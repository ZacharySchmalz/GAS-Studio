using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLogger : Logger {

    public bool x = true;
    public bool y = true;
    public bool z = true;

    private Transform logTransform;
    private float xval;
    private float yval;
    private float zval;

	public override void Start ()
    {

        logTransform = GetComponent<Transform>();
        type = LoggerType.Transform;

        if (logGameObjectName)
        {
            gameObjectName = gameObject.name;
        }
        base.Start();	
	}
	
	public override void Update ()
    {
        base.Update();	
	}

    public override void LogValues()
    {
        if (x)
        {
            xval = logTransform.position.x;
            LogManager.instance.Log(string.Format("{0} x: {1}", gameObjectName, xval));
        }
        if (y)
        {
            yval = logTransform.position.y;
            LogManager.instance.Log(string.Format("{0} y: {1}", gameObjectName, yval));
        }
        if (z)
        {
            zval = logTransform.position.z;
            LogManager.instance.Log(string.Format("{0} z: {1}", gameObjectName, zval));
        }

        base.LogValues();
    }
}
