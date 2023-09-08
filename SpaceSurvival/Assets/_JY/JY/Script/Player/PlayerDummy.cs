using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerDummy : MonoBehaviour, IHealth
{
    public AnimatorOverrideController pistol_AC;
    public AnimatorOverrideController shotGun_AC;
    public AnimatorOverrideController rifle_AC;
    public AnimatorOverrideController no_Weapon_AC;
    public enum WeaponType
    {
        None,
        Pistol,
        Rifle,
        ShotGun
    }
    WeaponType weaponType = WeaponType.None;
    public WeaponType Weapon_Type
    {
        get => weaponType;
        set
        {
            if (weaponType != value)
            {
                weaponType = value;
                switch (weaponType)
                {
                    case WeaponType.None:
                        on_Attack = Basic_Attack;
                        anim.runtimeAnimatorController = no_Weapon_AC;
                        break;
                    case WeaponType.Pistol:
                        on_Attack = Pistol_Attack;
                        weapon_Parent_Transform.localPosition = pistol_Pos;
                        weapon_Parent_Transform.localRotation = pistol_Rotation;
                        anim.runtimeAnimatorController = pistol_AC;
                        break;
                    case WeaponType.Rifle:
                        on_Attack = Rifle_Attack;
                        weapon_Parent_Transform.localPosition = rifle_Pos;
                        weapon_Parent_Transform.localRotation = rifle_Rotation;
                        anim.runtimeAnimatorController = rifle_AC;
                        break;
                    case WeaponType.ShotGun:
                        on_Attack = ShotGun_Attack;
                        anim.runtimeAnimatorController= shotGun_AC;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public enum ArmorType
    {
        None,
        SpaceArmor,
        BigArmor

    }
    ArmorType armorType = ArmorType.None;
     public ArmorType ArmorType_
    {
        get => armorType;
        set
        { 
            if (armorType != value)
            {
                armorType = value;
                Active_Correct_Armor();
            }
        }
    }
    void Active_Correct_Armor()
    {
        switch (armorType)
        {
            case ArmorType.None:
                DeActive_Armor();
                armors[0].gameObject.SetActive(true);
                break;
            case ArmorType.SpaceArmor:
                DeActive_Armor();
                armors[1].gameObject.SetActive(true);
                break;
            case ArmorType.BigArmor:
                DeActive_Armor();
                armors[2].gameObject.SetActive(true);
                break;
            default:
                break;

        }
    }
    void DeActive_Armor()
    {
        for (int i = 0; i < armors.Length; i++)
        {
            armors[i].gameObject.SetActive(false);
        }
        armors[3].gameObject.SetActive(true);
    }
    Transform[] armors;

    [SerializeField]
    private Transform bulletProjectilePrefab;

    public Transform weapon_Parent_Transform;
    Vector3 pistol_Pos;
    Vector3 rifle_Pos;
    Vector3 shotGun_Pos;
    Quaternion pistol_Rotation;
    Quaternion rifle_Rotation;
    Quaternion shotGun_Rotation;

    Transform shootPointTransform;
    AudioSource audioSource;


    InputKeyMouse inputActions;
    Animator anim;
    ItemDescription itemDescription;
    EquipBox_Description EquipBox_Description;
    EquipBox equipBox;

    public Action onOpenInven;

    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;
    public float pickupRange = 3.0f;

    public Action<ItemData> onEquipItem;
    public Action<ItemData> onUnEquipItem;
    public Action onClearSlot;
    public Action on_Attack;

    int attack_Trigger_Hash = Animator.StringToHash("Attack");
    
    uint darkForce = 500;
    public uint DarkForce
    {
        get => darkForce;
        set
        {
            darkForce = value;

        }
    }
    int money = 0;
    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                Debug.Log(money);
            }
        }
    }
    float hp = 200;
    public float HP
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = value;
                Debug.Log($"플레이어 HP : {hp:f0}");
            }
        }
    }
    float mp = 150;
    public float MP
    {
        get => mp;
        private set
        {
            if (mp != value)
            {
                mp = value;
            }
        }
    }
    uint att;
    public uint ATT
    {
        get => att;
        set
        {
            if (att != value)
            {
                att = value;
                Debug.Log($"플레이어 공격력 : {att}");
            }
        }
    }
    uint dp;
    public uint DP
    {
        get => dp;
        set
        {
            if (dp != value)
            {
                dp = value;
                Debug.Log($"플레이어 방어력 : {dp}");
            }
        }
    }

    private void Awake()
    {
        inputActions = new InputKeyMouse();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        
        pistol_Pos = new Vector3(0.012f, 0.085f, 0.141f);
        pistol_Rotation = Quaternion.Euler(7.309f, 74.719f, 267.319f);
        rifle_Pos = new Vector3(0, 0.11f, 0.226f);
        rifle_Rotation = Quaternion.Euler(3.819f, -293.381f, 247.4f);
        shotGun_Pos = new Vector3(0.006f, 0.153f, 0.199f);
        shotGun_Rotation = Quaternion.Euler(8.924f, -301.04f, 246.652f);
    }
    void Set_ShootPoint_Transform(Transform itemObj)
    {
        shootPointTransform = itemObj.GetChild(1);
    }
    private void Attack()
    {
        on_Attack();
    }
    void Basic_Attack()
    {
        anim.SetTrigger(attack_Trigger_Hash);
       // audioSource.Play();
    }
    void Pistol_Attack()
    {
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
        audioSource.Play();
    }
    void Rifle_Attack()
    {
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
        audioSource.Play();
    }
    void ShotGun_Attack()
    {
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
        audioSource.Play();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.ItemPickUp.performed += ItemPickUp;
        inputActions.Player.Equip_Item.performed += On_Equip_Item;
        inputActions.KeyBoard.Enable();
        inputActions.KeyBoard.InvenKey.performed += OpenInven;
        inputActions.Mouse.Enable();
        inputActions.Mouse.MouseClickRight.performed += On_MouseClickRight;
    }

    private void On_MouseClickRight(InputAction.CallbackContext _)
    {
        Attack();
    }

    private void Start()
    {
        itemDescription = GameManager.SlotManager.ItemDescription;
        equipBox = GameManager.EquipBox;
        EquipBox_Description = equipBox.Description;

        equipBox.on_Update_Status_For_EquipOrSwap += Update_Status_For_EquipOrSwap;
        equipBox.on_Update_Status_For_UnEquip += Update_Status_For_UnEquip;
        equipBox.on_Pass_Item_Transform += Set_ShootPoint_Transform;

        armors = new Transform[4];
        armors[0] = transform.GetChild(6).transform;// 기본 Crue 케릭터
        armors[1] = transform.GetChild(17).transform;// Space Armor
        armors[2] = transform.GetChild(20).transform;// Big Armor
        armors[3] = transform.GetChild(19).transform;// 머리
    }
    public void Disable_Input()
    {
        inputActions.KeyBoard.InvenKey.performed -= OpenInven;
    }
    public void Enable_Input()
    {
        inputActions.KeyBoard.InvenKey.performed += OpenInven;
    }
    void Update_Status_For_UnEquip(ItemData legacyData)
    {
        ItemData_Hat hat = legacyData as ItemData_Hat;
        ItemData_Enhancable weapon = legacyData as ItemData_Enhancable;
        ItemData_Armor armor = legacyData as ItemData_Armor;
        ItemData_Craft jewel = legacyData as ItemData_Craft;
        if (hat != null)
        {
            ATT -= hat.attack_Point;
            DP -= hat.defence_Point;
        }
        else if (armor != null)
        {
            ATT -= armor.attack_Point;
            DP -= armor.defence_Point;
        }
        else if (weapon != null)
        {
            ATT -= weapon.attackPoint;
            DP -= weapon.defencePoint;
        }
        else if (jewel != null)
        {
            ATT -= jewel.attack_Point;
            DP -= jewel.defence_Point;
        }
    }
    private void Update_Status_For_EquipOrSwap(ItemData legacyData, ItemData newData)//구조상 인터페이스를 사용했다면 아래와 같이 형변환을 하고 비교하는 과정이 번거롭지는 않았을 것 같다.
    {
        ItemData_Hat hat = newData as ItemData_Hat;
        ItemData_Enhancable weapon = newData as ItemData_Enhancable;
        ItemData_Armor armor = newData as ItemData_Armor;
        ItemData_Craft jewel = newData as ItemData_Craft;
        if( legacyData == null )//장착이 안되어있을 경우 더해주고 끝
        {
            if (hat != null)
            {
                ATT += hat.attack_Point;
                DP += hat.defence_Point;
            }
            else if (armor != null)
            {
                ATT += armor.attack_Point;
                DP += armor.defence_Point;
            }
            else if (weapon != null)
            {
                ATT += weapon.attackPoint;
                DP += weapon.defencePoint;
            }
            else if (jewel != null)
            {
                ATT += jewel.attack_Point;
                DP += jewel.defence_Point;
            }
        }
        else//이미 장착되어있었을 경우 스테이터스 더하고 빼기
        {
            if (hat != null)
            {
                att += hat.attack_Point;
                dp += hat.defence_Point;
                hat = legacyData as ItemData_Hat;
                ATT -= hat.attack_Point;
                DP -= hat.defence_Point;
            }
            else if (armor != null)
            {
                att += armor.attack_Point;
                dp += armor.defence_Point;
                armor = legacyData as ItemData_Armor;
                ATT -= armor.attack_Point;
                DP -= armor.defence_Point;
            }
            else if (weapon != null)
            {
                att += weapon.attackPoint;
                dp += weapon.defencePoint;
                weapon = legacyData as ItemData_Enhancable;
                ATT -= weapon.attackPoint;
                DP -= weapon.defencePoint;
            }
            else if (jewel != null)
            {
                att += jewel.attack_Point;
                dp += jewel.defence_Point;
                jewel = legacyData as ItemData_Craft;
                ATT -= jewel.attack_Point;
                DP -= jewel.defence_Point;
            }
        }
    }
 
    private void On_Equip_Item(InputAction.CallbackContext _)
    {
        if (itemDescription.ItemData != null)
        {
            onEquipItem?.Invoke(itemDescription.ItemData);
        }
        else if (EquipBox_Description.ItemData != null)
        {
            onUnEquipItem?.Invoke(EquipBox_Description.ItemData);
        }
    }
 

    private void OpenInven(InputAction.CallbackContext _)
    {
        onOpenInven?.Invoke();
    }

    private void ItemPickUp(InputAction.CallbackContext _)
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, pickupRange, LayerMask.GetMask("Item"));
        foreach(var collider in itemColliders)
        {
            ItemObject itemObj = collider.GetComponent<ItemObject>();
            //IConsumable consumable = itemObj.ItemData as IConsumable;
            //if (consumable != null)
            //{
            //    consumable.Consume(this.gameObject);
            //    Destroy(itemObj.gameObject);
            //}
            if (GameManager.SlotManager.AddItem(itemObj.ItemData.code))
            {
                Destroy(itemObj.gameObject);
            }  
        }
    }
    public void RecoveryHP_(int recoveryValue, float duration)
    {
        StartCoroutine(RecoveryHP(recoveryValue, duration));
    }
    IEnumerator RecoveryHP(int recoveryValue, float duration)
    {
        float regenPerSecond = recoveryValue / duration;
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            HP += regenPerSecond * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryMP()
    {
        yield return null;
    }
    bool IsEquipped()
    {
        return false;
    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }
#endif
}
