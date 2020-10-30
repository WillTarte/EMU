using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private string type = "weapon_melee";
    public int durability = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reload()
    {
        durability = 100;
    }

    public void use()
    {
        --durability;
    }
}
