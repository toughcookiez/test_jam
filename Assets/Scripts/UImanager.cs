using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UImanager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    public void StartPressed()
    {
        StartCoroutine(controller.ApplyShadows());
    }

    

}
