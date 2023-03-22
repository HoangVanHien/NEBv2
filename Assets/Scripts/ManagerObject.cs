using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerObject : MonoBehaviour
{
    private bool destroyThisManager = true;

    public virtual void Init()
    {
        destroyThisManager = false;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (destroyThisManager) Destroy(gameObject);
    }

}
