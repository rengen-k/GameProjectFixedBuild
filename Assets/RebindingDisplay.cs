using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingDisplay : MonoBehaviour
{

    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameObject waitingForInputObject = null;
    [SerializeField] private InputAction Action;
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI camRight;
    public TextMeshProUGUI camLeft;
    public TextMeshProUGUI movRight;
    public TextMeshProUGUI movLeft;
    public TextMeshProUGUI throwObj;
    public TextMeshProUGUI pickupObj;

    public CurrentSettings currentSettings;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Update()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        try
        {
            currentSettings = GameObject.FindGameObjectWithTag("CurrentSettings").GetComponent<CurrentSettings>();
            currentSettings.rebindingDisplay = this.gameObject.GetComponent<RebindingDisplay>();
        } catch {
            currentSettings = GameObject.FindGameObjectWithTag("LevelCurrentSettings").GetComponent<CurrentSettings>();
            currentSettings.rebindingDisplay = this.gameObject.GetComponent<RebindingDisplay>();
        }

    }

    public void RebindRotateCameraRight()
    {

        Action = playerController.playerActionsScript.Player.Look;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(4)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        camRight.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[4].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.RotateCameraRight = Action.bindings[4];
        })
        .Start();

        Action.Enable();
    }

    public void RebindRotateCameraLeft()
    {

        Action = playerController.playerActionsScript.Player.Look;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(3)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        camLeft.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[3].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.RotateCameraLeft = Action.bindings[3];
        })
        .Start();

        Action.Enable();
    }

    public void RebindMoveRight()
    {

        Action = playerController.playerActionsScript.Player.Move;
        var vector = Action.ChangeBinding("2D Vector");
        var right = vector.NextPartBinding("Right");
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(right.bindingIndex)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        movRight.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[right.bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.MoveRight = Action.bindings[right.bindingIndex];
        })
        .Start();

        Action.Enable();
    }

    public void RebindMoveLeft()
    {

        Action = playerController.playerActionsScript.Player.Move;
        var vector = Action.ChangeBinding("2D Vector");
        var left = vector.NextPartBinding("Left");
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(left.bindingIndex)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        movLeft.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[left.bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.MoveLeft = Action.bindings[left.bindingIndex];
        })
        .Start();

        Action.Enable();
    }

    public void RebindJump()
    {
        Action = playerController.playerActionsScript.Player.Jump;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding()
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        jumpText.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.Jump = Action.bindings[0];
        })
        .Start();

        Action.Enable();
    }

    public void RebindThrow()
    {
        Action = playerController.playerActionsScript.Player.Fire;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding(5)
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        throwObj.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[5].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.ThrowObject = Action.bindings[5];
        })
        .Start();

        Action.Enable();
    }

    public void RebindPickup()
    {
        Action = playerController.playerActionsScript.Player.Interact;
        waitingForInputObject.SetActive(true);

        Action.Disable();

        rebindingOperation = Action.PerformInteractiveRebinding()
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => {
        rebindingOperation.Dispose();
        waitingForInputObject.SetActive(false);
        pickupObj.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        currentSettings.PickupObject = Action.bindings[0];
        })
        .Start();

        Action.Enable();
    }

    public void ResetJump()
    {
        Action = playerController.playerActionsScript.Player.Jump;
        Action.Disable();
        //Action.Rebind()
        pickupObj.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
