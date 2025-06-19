using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UImanager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    public void StartPressed()
    {
        StartCoroutine(ApplyShadows(controller._shadowTranstionTime));
    }

    private IEnumerator ApplyShadows(float durationInSeconds)
    {
        float elpasedTime = 0;
        while(elpasedTime < durationInSeconds && durationInSeconds > 0)
        {
            controller._shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, elpasedTime / durationInSeconds));
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        controller._shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, 1));
        controller.Enabled = true;
    }

}
