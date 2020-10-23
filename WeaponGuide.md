# This is a guide to explain the "Weapons" feature for the game
## Important things
  - WeaponData (Scriptable Object)
  - ProjectileData (Scriptable Object)
  - WeaponShootStrategy (Scriptable Object*)
  - Weapon (GameObject/Prefab)
  - Projectile (GameObject/Prefab)
  - WeaponOnGroundBehaviour (GameObject/Prefab)
  - WeaponBehaviourScript (Monobehaviour)
  - WeaponOnGroundInteractBehaviour (Monobehaviour)
  - ProjectileBehaviourScript (Monobehaviour)
## How to create a new Weapon: A top-down approach
1. Drag and drop the "Weapon" prefab into the Hierarchy.
2. In the inspector, look for "Weapon Behaviour Script" which is what we'll modify.
4. Assign a new "WeaponData" instance to the correct field in the inspector.  
  a. Create a new "WeaponData" scriptable object by right clicking > Create > ScriptableObjects > WeaponData (put this is the Assets/ScriptableObjects/WeaponData folder pls)  
  b. Rename to an appropriate name (e.g. M16).  
  c. Create a new "ProjectileData" instance (right clicking > Create > ScriptableObjects > ProjectileData), modify its fields and assign it to the new "WeaponData" instance.  
  d. Create a new "WeaponShootStrategy" instance (right clicking > Create > ScriptableObjects > WeaponShootStrategy > "strategy"), modify its fields if needed, and assign it to the new "WeaponData" instance.  
  e. Change the fields of the new "WeaponData" instance to suit your needs.  
  f. Once done, you can assign this new WeaponData instance to the "Weapon Behavior Script" mentioned earlier.
5. You should be good to go!
## Wtf are Scriptable Objects ?
- [Scriptable Object Unity Manual](https://docs.unity3d.com/Manual/class-ScriptableObject.html)
- [Scriptable Object Unity API](https://docs.unity3d.com/ScriptReference/ScriptableObject.html)
- Basically, they are a way to serialize data (objects) which can still hook into the Unity systems.
- In this use case, we use them as a serializable data container (WeaponData, ProjectileData), and also as an implementation of the strategy pattern (WeaponShootStrategy)
