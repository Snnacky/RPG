using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput { get; private set; }
    private PlayerInputSet input;

    #region UI compontions
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip {  get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScene deathScreenUI { get; private set; }
    #endregion
    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        skillToolTip=GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip=GetComponentInChildren<UI_ItemToolTip>();
        statToolTip=GetComponentInChildren<UI_StatToolTip>();
        skillTreeUI=GetComponentInChildren<UI_SkillTree>(true); 
        inventoryUI=GetComponentInChildren<UI_Inventory>(true);
        storageUI=GetComponentInChildren<UI_Storage>(true);
        craftUI=GetComponentInChildren<UI_Craft>(true);
        merchantUI=GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI= GetComponentInChildren<UI_Options>(true);
        deathScreenUI=GetComponentInChildren<UI_DeathScene>(true);

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    private void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }

    //设置ui输入
    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTreeUI();
        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.OptionsUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }

    ////角色控制
    //private void StopPlayerControls(bool stopControls)
    //{
    //    if (stopControls)
    //        input.Player.Disable();
    //    else input.Player.Enable();
    //}
    //角色控制
    private void StopPlayerControlsIfNeeded()
    {
        foreach(var element in uiElements)
        {
            if(element.activeSelf)
            {
                Debug.Log(element);
                input.Player.Disable();
                return;
            }
        }
        input.Player.Enable();
        Debug.Log("yes");
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        input.Disable();
    }


    //设置ui
    public void OpenOptionsUI()
    {
        SwitchTo(optionsUI.gameObject);

        HideAllToolTips();
        StopPlayerControlsIfNeeded();


    }

    //游玩ui
    public void SwitchToInGameUI()
    {
        HideAllToolTips();
        SwitchTo(inGameUI.gameObject);//switchto要写在stop...ifneeded前面

        StopPlayerControlsIfNeeded();


        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach(var element in uiElements)
            element.gameObject.SetActive(false);

        objectToSwitchOn.SetActive(true);
    }


    //切换技能树ui开关
    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();//设置优先级

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllToolTips();  
        StopPlayerControlsIfNeeded();
    }
    //切换玩家库存ui开关
    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);

        HideAllToolTips();
        StopPlayerControlsIfNeeded();
    }

    //存储ui
    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControlsIfNeeded() ;
        if (openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);
            HideAllToolTips();  
        }
    }

    //商店ui
    public void OpenMerchantUI(bool openMerchantUI)
    {
        if (merchantUI.gameObject != null)
            merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControlsIfNeeded();
        if (openMerchantUI == false)
            HideAllToolTips();
    }

    //隐藏所有tooltip
    public void HideAllToolTips()
    {
        skillToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }

    //设置优先级
    private void SetTooltipsAsLastSibling()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }
} 
