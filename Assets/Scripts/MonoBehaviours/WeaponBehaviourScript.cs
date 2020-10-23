using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// Controls the behavior of a weapon. 
/// </summary>
public class WeaponBehaviourScript : MonoBehaviour
{
    // TODO: 
    // 1. Make the Active weapon follow the direction of the player.
    // 2. Change state of the weapon based on game state
    // 3. Make the projectiles spawn at the end of the weapon
    // 4. Make sure the weapon gameobject does not have more ammo than allowed by the WeaponData instance
    // 5. Make sure proper keybindings are set and used
    // 6. When player walks close to weapon OnGround, then display interactable prompt

    [SerializeField] private GameObject weaponOnGroundBehaviourPrefab;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponState weaponState;
    [SerializeField] private int currentMagazineAmmunition;
    [SerializeField] private int currentTotalAmmunition;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    
    public WeaponData WeaponData => weaponData;
    public Vector2 Direction { get; private set; }

    public WeaponState WeaponStateProp => weaponState;

    public int CurrentMagazineAmmunition
    {
        get => currentMagazineAmmunition;
        set => currentMagazineAmmunition = value;
    }

    public int CurrentTotalAmmunition
    {
        get => currentTotalAmmunition;
        set => currentTotalAmmunition = value;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        //TODO remove this
        Direction = Vector2.right;
    }

    private void Update()
    {
        switch (weaponState)
        {
            case WeaponState.OnGround:
                _spriteRenderer.sprite = weaponData.OnGroundSprite;
                _rigidbody.isKinematic = false;
                GameObject groundBehaviour = Instantiate(weaponOnGroundBehaviourPrefab, transform);
                groundBehaviour.GetComponent<WeaponOnGroundInteractBehaviour>().Init(this);
                groundBehaviour.SetActive(true);
                break;
            case WeaponState.InInventory:
                _spriteRenderer.sprite = weaponData.InInventorySprite;
                break;
            case WeaponState.Inactive:
                _spriteRenderer.sprite = weaponData.InactiveSprite;
                break;
            case WeaponState.Active:
                _spriteRenderer.sprite = weaponData.ActiveSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // TODO: Move this Input stuff to player game object
        if (weaponState == WeaponState.Active)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            } else if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Init(Vector2 direction)
    {
        this.Direction = direction;
    }
    
    public void Shoot()
    {
        weaponData.ShootStrategy.Shoot(this);
    }

    public void Reload()
    {
        weaponData.ShootStrategy.Reload(this);
    }

    public enum WeaponState
    {
        OnGround,
        InInventory,
        Inactive,
        Active
    }
}
