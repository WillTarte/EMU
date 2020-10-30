using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private string type = "weapon_ranged";
    private int maxMagazineSize = 12;
    public int magazineSize = 12;
    public int ammunition = 80;
    
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
        if (magazineSize < maxMagazineSize)
        {
            int toBeAdded = maxMagazineSize - magazineSize;

            if (ammunition < toBeAdded)
            {
                magazineSize += ammunition;
                ammunition = 0;
                ++magazineSize;
            }
        }
    }

    public void use()
    {
        --magazineSize;
    }

}
