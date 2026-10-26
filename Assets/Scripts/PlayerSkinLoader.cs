using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour
{
    [System.Serializable]
    public class SkinAnimationSet
    {
        public string skinName;

        public AnimationClip idle;
        public AnimationClip normalFlying;
        public AnimationClip tapFlying;
        public AnimationClip hitDamage;
        public AnimationClip gameOver;
    }

    [Header("Player Animator")]
    public Animator playerAnimator;

    [Header("Skin Animations")]
    public SkinAnimationSet[] skinAnimations;

    private int equippedSkin;

    private void Start()
    {
        LoadEquippedSkin();
    }

    private void LoadEquippedSkin()
    {
        equippedSkin = PlayerPrefs.GetInt("EquippedSkin", 0);

        Debug.Log("Equipped Skin Index: " + equippedSkin);

        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is not assigned!");
            return;
        }

        if (skinAnimations == null || skinAnimations.Length < 10)
        {
            Debug.LogError("Please assign all 10 skin animation sets!");
            return;
        }

        if (equippedSkin < 0 || equippedSkin >= skinAnimations.Length)
        {
            equippedSkin = 0;
        }

        ApplySkinAnimations(equippedSkin);
    }

    private void ApplySkinAnimations(int skinIndex)
    {
        RuntimeAnimatorController baseController =
            playerAnimator.runtimeAnimatorController;

        if (baseController == null)
        {
            Debug.LogError("Player Animator Controller is missing!");
            return;
        }

        AnimatorOverrideController overrideController =
            new AnimatorOverrideController(baseController);

        SkinAnimationSet skin = skinAnimations[skinIndex];

        // Override the actual base animation clip names.
        if (skin.idle != null)
            overrideController["Sky Rookie Idle"] = skin.idle;

        if (skin.normalFlying != null)
            overrideController["Sky Rookie NormalFlying"] = skin.normalFlying;

        if (skin.tapFlying != null)
            overrideController["Sky Rookie TapFlying"] = skin.tapFlying;

        if (skin.hitDamage != null)
            overrideController["Sky Rookie HitDamage"] = skin.hitDamage;

        if (skin.gameOver != null)
            overrideController["Sky Rookie GameOver"] = skin.gameOver;

        playerAnimator.runtimeAnimatorController = overrideController;

        Debug.Log("Loaded Skin Animations: " + skin.skinName);
    }
}