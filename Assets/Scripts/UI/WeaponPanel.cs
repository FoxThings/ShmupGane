using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    // Этот скрипт - технологический кринж из мира UI, не судите строго :))))
    public class WeaponPanel : MonoBehaviour
    {
        public List<WeaponIcon> Icons;
        public Sprite Upgrade;
        public List<ModuleInfo> Slots;
        public GameObject UpgradeSlot;
        public GameObject Panel;
    
        private Dictionary<WeaponTypes, Sprite> icons;
        private Dictionary<ModulePlacement, GameObject> slots;

        private Vector3 lastPos;
        private Vector3 lastScale;

        public static WeaponPanel S;

        private PlayerController player;

        void Start()
        {
            if (S != null)
            {
                Debug.LogError("WeaponPanel already exist");
                return;
            }
            
            S = this;
            
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
            slots = new Dictionary<ModulePlacement, GameObject>();
            icons = new Dictionary<WeaponTypes, Sprite>();
        
            foreach (var ico in Icons)
            {
                icons.Add(ico.Weapon, ico.Ico);
            }
        
            foreach (var slot in Slots)
            {
                slots.Add(slot.Placement, slot.Module);
            }
            
            lastPos = UpgradeSlot.transform.position;
            lastScale = UpgradeSlot.transform.localScale;
        }
        
        private void ClearInterface()
        {
            ClearButtons();
            UpgradeSlot.transform.position = lastPos;
            UpgradeSlot.transform.localScale = lastScale;
            foreach (var slot in slots)
            {
                var img = slot.Value.GetComponent<Image>();
                img.color = new Color(255, 255, 255, 0);
            }
        }

        private void ClearButtons()
        {
            foreach (var slot in slots)
            {
                slot.Value.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        public void DrawPanelUpgrade()
        {
            ClearInterface();
            foreach (var slot in slots)
            {
                var info = player.GetModule(slot.Key);
                if (info.Weapon != WeaponTypes.Nothing)
                {
                    var img = slot.Value.GetComponent<Image>();
                
                    img.color = Color.white;
                    img.sprite = icons[player.GetModule(slot.Key).Weapon];
                
                    UpgradeSlot.GetComponent<Image>().sprite = Upgrade;
                    slot.Value.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        var t = WeaponManager.Weapons[info.Weapon];
                        (info.Module.GetComponent(t) as IWeapon)?.Upgrade();
                        ClearButtons();
                        DoAnim(slot.Value.transform);
                    });
                }
            }
            Time.timeScale = 0;
            Panel.SetActive(true);
        }
        
        public void DrawPanelWeapon(WeaponTypes weapon)
        {
            ClearInterface();
            foreach (var slot in slots)
            {
                var info = player.GetModule(slot.Key);
                if (info.Weapon != WeaponTypes.Nothing)
                {
                    var img = slot.Value.GetComponent<Image>();

                    img.color = Color.white;
                    img.sprite = icons[player.GetModule(slot.Key).Weapon];
                }

                UpgradeSlot.GetComponent<Image>().sprite = icons[weapon];
                slot.Value.GetComponent<Button>().onClick.AddListener(() =>
                {
                    player.AddWeapon(slot.Key, weapon);
                    ClearButtons();
                    DoAnim(slot.Value.transform);
                });
            }

            Time.timeScale = 0;
            Panel.SetActive(true);
        }

        private void DoAnim(Transform to)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(UpgradeSlot.transform.DOMove(to.position, 1f).SetEase(Ease.InOutExpo));
            sequence.Append(UpgradeSlot.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine));
            sequence.SetUpdate(UpdateType.Normal, true);
            sequence.OnComplete(ClosePanel);
        }
        
        public void ClosePanel()
        {
            Time.timeScale = 1;
            Panel.SetActive(false);
        }
    
    }

    [Serializable]
    public class WeaponIcon
    {
        public WeaponTypes Weapon;
        public Sprite Ico;
    }
}