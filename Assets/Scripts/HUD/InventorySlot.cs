using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MonoBehaviours.WeaponsSystem;

namespace HUD
{
    public class InventorySlot : MonoBehaviour
    {
        //Image and text when the inventory slot is empty
        public Sprite defaultImage;
        public string defaultText;
        
        //Weapon currently in the inventory slot
        private WeaponBehaviourScript _weaponScript;
        
        //Sub components of the inventory slot
        private Image _image;
        private GameObject _selected;
        private TextMeshProUGUI _text;
        
        void Start()
        {
            _text = transform.Find("Info").gameObject.GetComponent<TextMeshProUGUI>();
            _selected = transform.Find("Selected").gameObject;
            _image = transform.Find("Image").gameObject.GetComponent<Image>();
            
            _image.sprite = defaultImage;
            _text.text = defaultText;
        }
        
        private void Update()
        {
            //TODO:
            // 1. Modify the text only when the player shoots. Avoid using Update() for UI.
            // 2. Modify the text according to the type of the weapon.
            if (_weaponScript != null)
            {
                _text.text = _weaponScript.CurrentMagazineAmmunition + "/" + _weaponScript.CurrentTotalAmmunition;
            }
        }

        public void AddWeaponToInventorySlot(WeaponBehaviourScript weaponScript)
        {
            _weaponScript = weaponScript;
            _image.sprite = _weaponScript.gameObject.GetComponent<SpriteRenderer>().sprite;
            _text.text = _weaponScript.CurrentMagazineAmmunition + "/" + _weaponScript.CurrentTotalAmmunition;
        }

        public void RemoveWeaponFromInventorySlot()
        {
            _weaponScript = null;
            _image.sprite = defaultImage;
            _text.text = defaultText;
        }
    }
}
