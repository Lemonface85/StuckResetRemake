using Il2CppRUMBLE.Environment;
using Il2CppRUMBLE.Managers;
using Il2CppRUMBLE.Players.Subsystems;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(StuckResetRemake.StuckResetRemake), "StuckResetRemake", "1.0.0", "Lemonface85")]
[assembly: MelonGame("Buckethead Entertainment", "RUMBLE")]
[assembly: MelonColor(255, 132, 66, 255)]

namespace StuckResetRemake;

public class StuckResetRemake : MelonMod {

    private readonly float timer = 2f;

    private float currentTime;

    private readonly InputAction leftInput = new InputAction(binding: "<XRController>{LeftHand}/joystickClicked");

    private readonly InputAction rightInput = new InputAction(binding: "<XRController>{RightHand}/joystickClicked");


    public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
        if (sceneName != "Park" && sceneName != "Gym") {
            leftInput.Disable();
            rightInput.Disable();
            MelonLogger.Msg($"Inputs Disabled. Scene:  {sceneName}");
        }
        else {
            leftInput.Enable();
            rightInput.Enable();
            MelonLogger.Msg($"Inputs Enabled. Scene: {sceneName}");
        }
    }

    public override void OnFixedUpdate() {
        if (!(rightInput.ReadValue<float>() >= 0.999f && leftInput.ReadValue<float>() >= 0.999f) || !leftInput.enabled) {
            currentTime = 0f;
            return;
        }

        currentTime += Time.fixedDeltaTime;

        if (currentTime > timer) {
            currentTime = 0f;
            PlayerResetSystem component =
                ((Component)GameObject.Find("Game Instance/Initializable/PlayerManager").GetComponent<PlayerManager>()
                    .localPlayer.Controller).gameObject.GetComponent<PlayerResetSystem>();

            component.ResetPlayerController();
            MelonLogger.Msg("Reset Player.");
        }
    }
}