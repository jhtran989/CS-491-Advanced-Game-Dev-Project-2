using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomOutline : Outline
{
    protected override void Awake()
    {
        this.OutlineMode = Outline.Mode.OutlineAll;
        this.OutlineColor = Color.white;
        this.OutlineWidth = 10f;
        
        base.Awake();
        
        enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // this.OutlineMode = Outline.Mode.OutlineAll;
        // this.OutlineColor = Color.white;
        // this.OutlineWidth = 10f;
        //
        // enabled = false;
    }

    // FIXME FINAL: if not commented out, it will override from base class
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public void EnableOutline()
    {
        enabled = true;
    }

    public void DisableOutline()
    {
        enabled = false;
    }
}
