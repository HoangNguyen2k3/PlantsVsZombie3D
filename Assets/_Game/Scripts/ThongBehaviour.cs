using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThongBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        
    }
    protected virtual void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        //For overide
    }
}
