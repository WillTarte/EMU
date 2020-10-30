using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryWeapon : MonoBehaviour
{
    public Sprite defaultImage;
    public string defaultText;
    public GameObject weapon;
    
    private Image image;
    private GameObject selected;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        selected = transform.GetChild(1).gameObject;
        image = transform.GetChild(2).gameObject.GetComponent<Image>();
        image.sprite = defaultImage;
        text.text = defaultText;
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon != null)
        {
            if (weapon.name.Contains("Katana"))
            {
                text.text = weapon.GetComponent<Katana>().durability + "%";
            }
            else if (weapon.name.Contains("Gun"))
            {
                text.text = weapon.GetComponent<Gun>().magazineSize + "/" + weapon.GetComponent<Gun>().ammunition;
            }
        }
    }

    public void setWeapon(GameObject newWeapon)
    {
        weapon = newWeapon;
        image.sprite = newWeapon.GetComponent<Image>().sprite;
        if (newWeapon.name.Contains("Katana"))
        {
            text.text = newWeapon.GetComponent<Katana>().durability + "%";
        }
        else if (newWeapon.name.Contains("Gun"))
        {
            text.text = newWeapon.GetComponent<Gun>().magazineSize + "/" + newWeapon.GetComponent<Gun>().ammunition;
        }
    }

    public bool haveWeapon()
    {
        if (weapon) return true;
        else return false;
    }

    public void removeWeapon()
    {
        weapon = null;
        image.sprite = defaultImage;
        text.text = defaultText;
    }
}
