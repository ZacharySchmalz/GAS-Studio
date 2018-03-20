using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision_UnityEvent : MonoBehaviour {

    public string collisionTag;
    public UnityEvent OnCollisionEnterEvent;
    public UnityEvent OnCollisionExitEvent;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == collisionTag && OnCollisionEnterEvent != null)
            OnCollisionEnterEvent.Invoke();
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == collisionTag && OnCollisionExitEvent != null)
            OnCollisionExitEvent.Invoke();
    }

}
