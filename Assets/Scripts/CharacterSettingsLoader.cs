using System.Collections;
using System.Collections.Generic;
using UGM.Examples.ThirdPersonController;
using UnityEngine;

namespace UnityRoyale
{
    public class CharacterSettingsLoader : MonoBehaviour
    {
        public string characterName;
        public AvatarLoader avatarLoader;
        public HumanoidEquipmentLoader equipmentLoader;

        private async void Start()
        {
            if(characterName == LaunchSettingsManager.Instance.settings.warrior.name)
            {
                //Set the avatar id
                await avatarLoader.LoadAsync(LaunchSettingsManager.Instance.settings.warrior.avatarId);
                //Set the weapon id to load after the avatar
                await equipmentLoader.LoadAsync(LaunchSettingsManager.Instance.settings.warrior.weaponId);
            }
            else if (characterName == LaunchSettingsManager.Instance.settings.archer.name)
            {
                //Set the avatar id
                await avatarLoader.LoadAsync(LaunchSettingsManager.Instance.settings.archer.avatarId);
                //Set the weapon id to load after the avatar
                await equipmentLoader.LoadAsync(LaunchSettingsManager.Instance.settings.archer.weaponId);
            }
            else if (characterName == LaunchSettingsManager.Instance.settings.mage.name)
            {
                //Set the avatar id
                await avatarLoader.LoadAsync(LaunchSettingsManager.Instance.settings.mage.avatarId);
                //Set the weapon id to load after the avatar
                await equipmentLoader.LoadAsync(LaunchSettingsManager.Instance.settings.mage.weaponId);
            }
        }
    }
}