using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGM.Examples.Inventory;
using UGM.Examples.ThirdPersonController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityRoyale
{
    public class LaunchSettingsManager : MonoBehaviour
    {
        public static LaunchSettingsManager Instance;

        [System.Serializable]
        public struct CharacterSettings
        {
            public string name;
            public string avatarId;
            public string weaponId;
        }
        [System.Serializable]
        public struct PlayerSettings
        {
            public CharacterSettings warrior;
            public CharacterSettings archer;
            public CharacterSettings mage;

        }
        public AvatarLoader avatarLoader;
        public HumanoidEquipmentLoader equipmentLoader;

        public GameObject LoadingScreen;
        public Image LoadingImage;

        public GameObject CardsGO;
        public GameObject AvatarGO;
        public GameObject AvatarUIGO;
        public GameObject EquipUIGO;

        public GameObject[] pools;
        public CharacterSettingsLoader[] preloadedCharacters;

        public PlayerSettings settings;

        private CharacterSettings selectedSettings;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                avatarLoader.onModelSuccess.AddListener(AvatarChanged);
            }
            else
            {
                Instance.pools = pools;
                Instance.preloadedCharacters = preloadedCharacters;
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                avatarLoader.onModelSuccess.RemoveListener(AvatarChanged);
            }
        }

        public async void SelectCharacter(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                AvatarGO.SetActive(false);
                AvatarUIGO.SetActive(false);
                EquipUIGO.SetActive(false);
                return;
            }
            AvatarGO.SetActive(true);
            //Set the currently selected
            if (name == settings.warrior.name)
            {
                selectedSettings = settings.warrior;
                await avatarLoader.LoadAsync(settings.warrior.avatarId);
                equipmentLoader.Load(settings.warrior.weaponId);
            }
            else if(name == settings.archer.name)
            {
                selectedSettings = settings.archer;
                await avatarLoader.LoadAsync(settings.archer.avatarId);
                equipmentLoader.Load(settings.archer.weaponId);
            }
            else if(name == settings.mage.name)
            {
                selectedSettings = settings.mage;
                await avatarLoader.LoadAsync(settings.mage.avatarId);
                equipmentLoader.Load(settings.mage.weaponId);
            }
            AvatarUIGO.SetActive(true);
            EquipUIGO.SetActive(true);
        }

        private void AvatarChanged(GameObject arg0)
        {
            if (selectedSettings.name == settings.warrior.name)
            {
                equipmentLoader.Load(settings.warrior.weaponId);
            }
            else if (selectedSettings.name == settings.archer.name)
            {
                equipmentLoader.Load(settings.archer.weaponId);
            }
            else if (selectedSettings.name == settings.mage.name)
            {
                equipmentLoader.Load(settings.mage.weaponId);
            }
        }

        public void SetAvatar(string avatarId)
        {
            selectedSettings.avatarId = avatarId;
            if (selectedSettings.name == settings.warrior.name)
            {
                settings.warrior = selectedSettings;
            }
            else if (selectedSettings.name == settings.archer.name)
            {
                settings.archer = selectedSettings;
            }
            else if (selectedSettings.name == settings.mage.name)
            {
                settings.mage = selectedSettings;
            }
        }

        public void SetWeapon(string weaponId)
        {
            selectedSettings.weaponId = weaponId;
            if (selectedSettings.name == settings.warrior.name)
            {
                settings.warrior = selectedSettings;
            }
            else if (selectedSettings.name == settings.archer.name)
            {
                settings.archer = selectedSettings;
            }
            else if (selectedSettings.name == settings.mage.name)
            {
                settings.mage = selectedSettings;
            }
        }

        public void ReturnToLaunch()
        {
            //Always refresh the pools
            foreach (var pool in pools)
            {
                DestroyImmediate(pool);
            }
            SceneManager.LoadScene("Launch");
            CardsGO.SetActive(true);
        }

        public async void PlayGame()
        {
            LoadingScreen.SetActive(true);
            var current = 0;
            var total = 0;
            var simultaneousMax = 3;
            List<Task> tasks = new List<Task>();
            foreach (var character in preloadedCharacters)
            {
                CardsGO.SetActive(false);
                AvatarGO.SetActive(false);
                AvatarUIGO.SetActive(false);
                EquipUIGO.SetActive(false);
                if (character.isPreload)
                {
                    current++;
                    total++;
                    tasks.Add(character.SetupAvatar());
                    LoadingImage.fillAmount = (float)total / (float)preloadedCharacters.Length;
                }
                if (current == simultaneousMax)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                    current = 0;
                }
            }

            LoadingScreen.SetActive(false);
            SceneManager.LoadScene("Main");
        }
    }
}
