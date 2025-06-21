using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UImanager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    [SerializeField] private GameObject BatteryObject;

    public void StartPressed()
    {
        StartCoroutine(controller.ApplyShadows());
    }

    private void Update()
    {
        if (controller._flashlightCharges == 0)
        {
            Destroy(BatteryObject);
        }
        
    }

}