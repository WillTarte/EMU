using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject[] inventoryContainer = new GameObject[3];

    private int selectedIndex = 0;
    private bool test = true;

    // Start is called before the first frame update
    void Start()
    {
        inventoryContainer[0] = transform.GetChild(0).gameObject;
        inventoryContainer[1] =  transform.GetChild(1).gameObject;
        inventoryContainer[2] =  transform.GetChild(2).gameObject;
        transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        bool ADown = Input.GetKeyDown(KeyCode.A);
        bool SDown = Input.GetKeyDown(KeyCode.S);
        bool DDown = Input.GetKeyDown(KeyCode.D);
        bool FDown = Input.GetKeyDown(KeyCode.F);
        bool GDown = Input.GetKeyDown(KeyCode.G);
        bool HDown = Input.GetKeyDown(KeyCode.H);
        bool JDown = Input.GetKeyDown(KeyCode.J);
        
        if (ADown) changeSelected();
        
        if (DDown) addToInventory("weapon_ranged", Instantiate(Resources.Load<GameObject>("Temporary/Gun"), new Vector3(0.0f, 0.0f), Quaternion.identity) as GameObject);
        if (SDown) addToInventory("throwable", Instantiate(Resources.Load<GameObject>("Temporary/Grenade"), new Vector3(0.0f, 0.0f), Quaternion.identity) as GameObject);
        if (FDown) addToInventory("weapon_melee", Instantiate(Resources.Load<GameObject>("Temporary/Katana"), new Vector3(0.0f, 0.0f), Quaternion.identity) as GameObject);
        
        if (HDown) useSelected();
        if (JDown) updateSelected();
        
        if (GDown) throwSelected();
        
        if (test)
        {
            addToInventory("weapon_ranged", Instantiate(Resources.Load<GameObject>("Temporary/Gun"), new Vector3(0.0f, 0.0f), Quaternion.identity) as GameObject);
            test = false;
        }
    }

    public void changeSelected()
    {
        inventoryContainer[selectedIndex].transform.GetChild(1).gameObject.SetActive(false);
        
        do
        {
            ++selectedIndex;
            if (selectedIndex > 2) selectedIndex = 0;
        } while (!checkIfWeapon(selectedIndex));
        
        inventoryContainer[selectedIndex].transform.GetChild(1).gameObject.SetActive(true);
        
    }

    public void throwSelected()
    {
        if (selectedIndex == 2)
        {
            inventoryContainer[selectedIndex].SendMessage("removeThrowable", SendMessageOptions.DontRequireReceiver);
            changeSelected();
        }
        else if (selectedIndex == 0 && checkIfWeapon(1))
        {
            GameObject temp = inventoryContainer[1].gameObject.GetComponent<InventoryWeapon>().weapon;
            inventoryContainer[0].SendMessage("setWeapon", temp, SendMessageOptions.DontRequireReceiver); 
            inventoryContainer[1].SendMessage("removeWeapon", SendMessageOptions.DontRequireReceiver);
        }
        else if (selectedIndex == 1)
        {
            inventoryContainer[selectedIndex].SendMessage("removeWeapon", SendMessageOptions.DontRequireReceiver);
            changeSelected();
        }
    }

    public void updateSelected()
    {
        inventoryContainer[selectedIndex].SendMessage("reload", SendMessageOptions.RequireReceiver);
    }

    public void useSelected()
    {
        inventoryContainer[selectedIndex].SendMessage("use", SendMessageOptions.RequireReceiver);
    }

    public void addToInventory(string type, GameObject item)
    {
        switch (type)
        {
            case "weapon_melee":
                for (int index = 0; index < 2; ++index)
                {
                    if (!checkIfWeapon(index))
                    {
                        inventoryContainer[index].SendMessage("setWeapon", item, SendMessageOptions.DontRequireReceiver); 
                        break;
                    }
                }
                break;
            case "weapon_ranged":
                for (int index = 0; index < 2; ++index)
                {
                    if (!checkIfWeapon(index))
                    {
                        inventoryContainer[index].SendMessage("setWeapon", item, SendMessageOptions.DontRequireReceiver);
                        break;
                    }
                }
                break;
            case "throwable":
                if (!checkIfWeapon(2))
                {
                    inventoryContainer[2].SendMessage("setThrowable", item, SendMessageOptions.DontRequireReceiver);
                }
                break;
            default:
                print("Inventory: \"unrecognized item type\"");
                break;
        }
    }

    private bool checkIfWeapon(int index)
    {
        if (index == 2)
        {
            return inventoryContainer[2].GetComponent<InventoryThrowable>().throwable != null;
        }
        else return inventoryContainer[index].GetComponent<InventoryWeapon>().weapon != null;
    }
}
