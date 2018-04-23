using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

/// <summary>
/// Controls the input devices, checks if any device "Start" is pressed and creates a new avatar controller
/// </summary>
public class ExplorationController : MonoBehaviour
{
    [SerializeField]
    private Color[] playerColors;

    [SerializeField]
    private AvatarController avatarControllerPrefab;

    [SerializeField]
    private Transform avatarControllerParent;
    
    private List<AvatarController> _controllers;

    public List<AvatarController> controllers
    {
        get 
        {
            if(_controllers == null)
            {
                _controllers = new List<AvatarController>();
            }

            return _controllers;
        }
    }

    internal void Initialize(SerializableGame game)
    {
        foreach (InputModule module in DeviceController.inputModules)
        {
            if (!moduleInUse(module))
            {
                createAvatarController(module);
            }
        }
        
        foreach(AvatarController controller in controllers)
        {
            if (controller.character == null)
            {
                controller.character = GameController.instance.party[controller.preferredCharacterId];

                if (controller.character != null)
                {
                    controller.character.solid.Show();
                    controller.character.GetComponentInChildren<NavMeshAnimator>().Reset();
                    NeverdawnCamera.AddTargetLerped(controller.character.transform);
                }
                else
                {
                    Debug.LogWarning("A character was lost during scene transition");
                    return;
                }
            }
        }

        foreach(AvatarController controller in controllers)
        {
            controller.character.position = game.currentPosition.ToVector3();
            controller.character.eulerAngles = game.currentRotation.ToVector3();
        }

        DeviceController.onModuleActivated -= createAvatarController;
        DeviceController.onModuleActivated += createAvatarController;
    }

    private bool moduleInUse(InputModule module)
    {
        foreach(AvatarController controller in controllers)
        {
            if (controller.inputModule == module)
            {
                return true;
            }
        }

        return false;
    }

    private void createAvatarController(InputModule module)
    {
        AvatarController controller = GameObject.Instantiate(avatarControllerPrefab);
        controller.inputModule = module; 
        controller.transform.SetParent(avatarControllerParent);
        controller.color = playerColors[controllers.Count];

        controller.character = GameController.instance.party.GetNextCharacter();

        if (controller.character)
        {
            controller.characterMenu = UICharacterMenus.GetMenu(controller.character);
            controller.preferredCharacterId = controller.character.id;
            NeverdawnCamera.AddTargetLerped(controller.character.transform);
        }

        controllers.Add(controller);
    }

    public void UpdateExplorationController(float timekey)
    {
        foreach(AvatarController controller in controllers)
        {
            controller.UpdateExplorationControls();
        }
    }
}
