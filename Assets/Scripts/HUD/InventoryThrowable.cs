using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryThrowable : MonoBehaviour
{
    public Sprite defaultImage;
    public string defaultText;
    public GameObject throwable;
    
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
        if (throwable != null) text.text = "" + throwable.GetComponent<Grenade>().ammunition;
    }

    public void setThrowable(GameObject newWeapon)
    {
        throwable = newWeapon;
        image.sprite = newWeapon.GetComponent<Image>().sprite;
        text.text = "" + newWeapon.GetComponent<Grenade>().ammunition;
    }

    public bool haveThrowable()
    {
        if (throwable) return true;
        else return false;
    }

    public void removeThrowable()
    {
        throwable = null;
        image.sprite = defaultImage;
        text.text = defaultText;
    }
}
