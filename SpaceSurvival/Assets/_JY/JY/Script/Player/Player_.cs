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


public class Player_ : MonoBehaviour, IBattle
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
                    anim.runtimeAnimatorController = shotGun_AC;
                    break;
                default:
                    break;
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

    Transform weapon_Parent_Transform;
    Transform jewel_Parent_Transform;
    Transform hat_Parent_Transform;
    Transform suit_Parent_Transform;

    public Transform Weapon_Parent_Transform => weapon_Parent_Transform;
    public Transform Jewel_Parent_Transform => jewel_Parent_Transform;
    public Transform Hat_Parent_Transform => hat_Parent_Transform;
    public Transform Suit_Parent_Transform => suit_Parent_Transform;

    Vector3 pistol_Pos;
    Vector3 rifle_Pos;
    Vector3 shotGun_Pos;
    Quaternion pistol_Rotation;
    Quaternion rifle_Rotation;
    Quaternion shotGun_Rotation;
    Transform shootPointTransform;

    AudioSource audioSource;
    public AudioClip pistol_Sound;
    public AudioClip shotGun_Sound;
    public AudioClip rifle_Sound;
    public AudioClip equip_Sound;
    public AudioClip punch_Sound;
    public AudioClip potion_Sound;


    Animator anim;
    SkillBox_Description skill_Description;
    ItemDescription itemDescription;
    EquipBox_Description EquipBox_Description;
    EquipBox equipBox;
    Skill_Blessing skill_Blessing;
    SkillData currentSkillData = null;
    Player_Status player_Status;


    public Action onOpenInven;

    public float moveSpeed = 0.0f;
    public float rotateSpeed = 0.0f;
    public float pickupRange = 3.0f;

    public Action<ItemData> onEquipItem;
    public Action<ItemData> onUnEquipItem;
    public Action onClearSlot;
    public Action on_Attack;
    public Action<float> on_Player_Stamina_Change;
    public Action<float> on_Player_HP_Change;
    public Action on_DarkForce_Change;
    public Action<SkillData> on_ActiveSkill;
    public Action<Skill_Blessing> on_Buff_Start;
    public Action<bool> on_CursorChange;

    int attack_Trigger_Hash = Animator.StringToHash("Attack");
    int get_Hit_Hash = Animator.StringToHash("Get_Hit");

  
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
    float maxHP = 200;
    public float MaxHp => maxHP;
    public float HP
    {
        get => hp;
        private set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
                on_Player_HP_Change?.Invoke(hp);
            }
        }
    }
    float stamina = 10;
    const float max_Stamina = 20;
    public float Max_Stamina => max_Stamina;
    public float Stamina
    {
        get => stamina;
        set
        {
            if (stamina != value)
            {
                stamina = Mathf.Clamp(value, 0, max_Stamina);
                on_Player_Stamina_Change?.Invoke(stamina);
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
    bool duringBuffSkill = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        pistol_Pos = new Vector3(0.012f, 0.085f, 0.141f);
        pistol_Rotation = Quaternion.Euler(7.309f, 74.719f, 267.319f);
        rifle_Pos = new Vector3(0, 0.11f, 0.226f);
        rifle_Rotation = Quaternion.Euler(3.819f, -293.381f, 247.4f);
        shotGun_Pos = new Vector3(0.006f, 0.153f, 0.199f);
        shotGun_Rotation = Quaternion.Euler(8.924f, -301.04f, 246.652f);

        weapon_Parent_Transform = GetComponentInChildren<Weapon_Parent_Transform>().transform;
        jewel_Parent_Transform = GetComponentInChildren<Jewel_Parent_Transform>().transform;
        hat_Parent_Transform = GetComponentInChildren<Hat_Parent_Transform>().transform;
        suit_Parent_Transform = GetComponentInChildren<BodySuit_Parent_Transform>().transform;
    }
    void Set_ShootPoint_Transform(Transform itemObj)
    {
        shootPointTransform = itemObj.GetChild(1);
    }
 
    private void Attack()
    {
        Stamina--;
        on_Attack();
    }
    void Basic_Attack()
    {
        anim.SetTrigger(attack_Trigger_Hash);
        audioSource.PlayOneShot(punch_Sound);
        // audioSource.Play();
    }
    void Pistol_Attack()
    {
        audioSource.PlayOneShot(pistol_Sound);
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
    }
    void Rifle_Attack()
    {
        audioSource.PlayOneShot(rifle_Sound);
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
    }
    void ShotGun_Attack()
    {
        audioSource.PlayOneShot(shotGun_Sound);
        anim.SetTrigger(attack_Trigger_Hash);
        Instantiate(bulletProjectilePrefab, shootPointTransform.position, shootPointTransform.rotation);
    }
    public void Skill_Action(SkillData skillData)
    {
        skill_Blessing = skillData as Skill_Blessing;
        if (skill_Blessing == null && Stamina >= skillData.Require_Stamina_For_UsingSkill && this.Weapon_Type != WeaponType.None)
        {
            if (player_Status.IsCritical(skillData))
            {
                on_ActiveSkill?.Invoke(skillData);

            }
            else
            {
                on_ActiveSkill?.Invoke(skillData);
            }
            on_CursorChange?.Invoke(true);
            this.currentSkillData = skillData; 
        }
        else if (skill_Blessing != null)//만약 사용한 스킬이 버프스킬이면@@@// PlayerStatus에 신호보내 적용하기
        {
            player_Status.Reset_Status();//장비아이템의 능력치가 합산된 플레이어의 공격력, 방어력 적용하기
            float finalAttackPoint = player_Status.ATT * skill_Blessing.SkillPower;
            float finalDefencePoint = player_Status.DP * skill_Blessing.SkillPower;
            player_Status.ATT = (uint)finalAttackPoint; //리셋된 공격력에 스킬의 skillPower만큼 곱해주기
            player_Status.DP = (uint)finalDefencePoint;
            on_Buff_Start?.Invoke(skill_Blessing);// TurnBuffCount 프로퍼티로 몇턴 동안 버프가 유지될 것인지 설정되어 있습니다.
            duringBuffSkill = true;//버프스킬 발동중 표시
            on_CursorChange?.Invoke(false);
            GameManager.EffectPool.GetObject(SkillType.Blessing,transform.position);
            GameManager.SoundManager.PlayOneShot_Buff();
            return;
        }
    }

    public void Rotate(Vector3 position)
    {
        //position.y = f;
        transform.rotation = Quaternion.LookRotation(position - transform.position);
    }
    public void SkillPostProcess()//skillAction 실행 후 grid 에서 호출할 함수 
    {
       // StopCoroutine(RotateCoroutine);
        Stamina--;
        anim.SetTrigger(attack_Trigger_Hash);
        on_CursorChange?.Invoke(false);
        if (this.currentSkillData is Skill_Sniping)
        {
            GameManager.SoundManager.PlayOneShot_Sniping();
        }
        else if (this.currentSkillData is Skill_Penetrate)
        {
            GameManager.SoundManager.PlayOneShot_Penetrate();
        }
        else if (this.currentSkillData is Skill_Rampage)
        {
            GameManager.SoundManager.PlayOneShot_Rampage();
        }
        else if (this.currentSkillData is Skill_Normal_Attack)
        {
            GameManager.SoundManager.PlayOneShot_NormalAttack();
        }

    }
    void PopupLevelUp_Effect()
    {
        GameManager.SoundManager.PlayOneShot_LevelUp();
        GameManager.EffectPool.GetLevelUp_Effect(transform);
    }
    public void DeBuff()//버프스킬 적용 해제
    {
        player_Status.Reset_Status();
    }

  
    private void On_MouseClickRight()
    {
        Attack();
    }

    private void Start()
    {
        InputSystemController.Instance.OnUI_Inven_ItemPickUp += ItemPickUp;
        InputSystemController.Instance.OnUI_Inven_DoubleClick += On_DoubleClick;
        InputSystemController.Instance.OnUI_Inven_Inven_Open += OpenInven;
        InputSystemController.Instance.OnUI_Inven_MouseClickRight += On_MouseClickRight;

        skill_Description = FindObjectOfType<SkillBox_Description>();
        player_Status = GameManager.PlayerStatus;
        itemDescription = GameManager.SlotManager.ItemDescription;
        equipBox = GameManager.EquipBox;
        EquipBox_Description = equipBox.Description;

        onEquipItem += equipBox.Set_ItemData_For_DoubleClick;
        onUnEquipItem += GameManager.SlotManager.UnEquip_Item;
        onOpenInven += GameManager.Inventory.Open_Inventory;
        GameManager.QuickSlot_Manager.on_Activate_Skill += Skill_Action;

        player_Status.on_LevelUp += PopupLevelUp_Effect;

        equipBox.on_Update_Status += Update_Status;
        equipBox.on_Pass_Item_Transform += Set_ShootPoint_Transform;

        armors = new Transform[4];
        armors[0] = transform.GetChild(6).transform;// 기본 Crue 케릭터
        armors[1] = transform.GetChild(17).transform;// Space Armor
        armors[2] = transform.GetChild(20).transform;// Big Armor
        armors[3] = transform.GetChild(19).transform;// 머리

        //초기스펙 설정
        Weapon_Type = WeaponType.None;
    }

     void Update_Status()
    {
  
        if (duringBuffSkill)//버프중이면
        {
            player_Status.Reset_Status();//장비아이템의 능력치가 합산된 플레이어의 공격력, 방어력 적용하기
            float finalAttackPoint = player_Status.ATT * skill_Blessing.SkillPower;
            float finalDefencePoint = player_Status.DP * skill_Blessing.SkillPower;
            this.ATT = (uint)finalAttackPoint; //리셋된 공격력에 스킬의 skillPower만큼 곱해주기
            this.DP = (uint)finalDefencePoint;
        }
        else
        {
            player_Status.Reset_Status();
        }
    }


    //private void On_Equip_Item(InputAction.CallbackContext _)
    private void On_DoubleClick()
    {
        if (itemDescription.ItemData != null)
        {
            audioSource.PlayOneShot(equip_Sound);
            Stamina--;//다른 아이템 장착시  stamina 차감
            onEquipItem?.Invoke(itemDescription.ItemData);
        }
        else if (EquipBox_Description.ItemData != null)
        {
            onUnEquipItem?.Invoke(EquipBox_Description.ItemData);
        }
        else if (skill_Description.SkillData != null)
        {
            Skill_Action(skill_Description.SkillData);
        }
    }


    //private void OpenInven(InputAction.CallbackContext _)
    private void OpenInven()
    {
        Debug.Log("1");
        onOpenInven?.Invoke();
    }

    //private void ItemPickUp(InputAction.CallbackContext _)
    private void ItemPickUp()
    {
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, pickupRange, LayerMask.GetMask("Item"));
        foreach (var collider in itemColliders)
        {
            ItemObject itemObj = collider.GetComponent<ItemObject>();
            if (itemObj != null)
            {
                if (GameManager.SlotManager.AddItem(itemObj.ItemData.code))
                {
                    Destroy(itemObj.gameObject);
                }

            }
        }
    }
    public void Play_PotionSound()
    {
        audioSource.PlayOneShot(potion_Sound);
    }
    public void Recovery_HP(int recoveryValue, float duration)
    {
        Stamina--;// stamina 차감
        StartCoroutine(Recovery_HP_(recoveryValue, duration));
    }
    public void Recovery_Stamina(int recoveryValue, float duration)
    {
        StartCoroutine(Recovery_Stamina_(recoveryValue, duration));
    }
    IEnumerator Recovery_HP_(int recoveryValue, float duration)
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
    IEnumerator Recovery_Stamina_(int recoveryValue, float duration)
    {
        float regenPerSecond = recoveryValue / duration;
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Stamina += regenPerSecond * Time.deltaTime;
            yield return null;
        }
    }
    bool IsEquipped()
    {
        return false;
    }

    public void Attack_Enemy(IBattle target)
    {

        Attack();
        float damage = att;
        if (target != null)
        {
            target.Defence(damage);
        }
    }

    public void Defence(float damage, bool isCritical)
    {
        anim.SetTrigger(get_Hit_Hash);
        float final_Damage = damage - DP;
        GameManager.PlayerStatus.HP -= final_Damage;
        GameManager.EffectPool.GetObject(damage, transform, isCritical);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, pickupRange, 2.0f);
    }


#endif
}
