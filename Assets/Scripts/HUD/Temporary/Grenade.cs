using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private string type = "throwable";
    public int ammunition = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getAmmo()
    {
        return ammunition;
    }
    
    public void use()
    {
        --ammunition;
    }

    public void reload()
    {
        ammunition = 5;
    }
    
    
}
