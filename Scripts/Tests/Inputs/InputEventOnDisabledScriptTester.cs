
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class InputEventOnDisabledScriptTester : UdonSharpBehaviour
{
    InputTester linkedInputTester;

    public bool changeGameObjectStateInSetup;
    public bool changeScriptStateInSetup;

    bool alreadyCalled = false;

    int index;
    public int Index
    {
        get
        {
            return index;
        }
    }

    string description;
    public string Description
    {
        get
        {
            return description;
        }
    }

    public string KnownLink { get; private set; } = "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/input-events-like-inputjump-inputuse-still-triggered-if-the-gameobject-is-disabl";

    public bool KnownIssue { get; private set; }

    bool shouldBeCalled;
    public bool ShouldBeCalled;
        /*
    {
        get
        {
            return shouldBeCalled;
        }
    }
        */

    public void Setup(InputTester linkedInputTester, int index)
    {
        this.linkedInputTester = linkedInputTester;
        this.index = index;

        bool gameObjectEnabled = gameObject.activeInHierarchy ^ changeGameObjectStateInSetup;
        bool scriptEnabled = !changeScriptStateInSetup;

        KnownIssue = (gameObject.activeInHierarchy && changeGameObjectStateInSetup);

        shouldBeCalled = gameObjectEnabled && scriptEnabled;

        description = $"Input event on GO from active {gameObject.activeSelf} to {gameObject.activeSelf ^ changeGameObjectStateInSetup} and script {(changeScriptStateInSetup ? "disabled" : "kept enabled")}. Event {(shouldBeCalled ? "should be called" : "should not be called")}";

        enabled = scriptEnabled;
        gameObject.SetActive(gameObjectEnabled);
    }

    void CheckTestState()
    {
        linkedInputTester.InputEventReceivedFromDisabledScript(this, (shouldBeCalled ? TestStates.Passed : TestStates.Failed));
    }

    public override void InputUse(bool value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputGrab(bool value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputDrop(bool value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputLookVertical(float value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputLookHorizontal(float value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputMoveHorizontal(float value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputMoveVertical(float value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }

    public override void InputJump(bool value, UdonInputEventArgs args)
    {
        if (alreadyCalled) return;
        alreadyCalled = true;

        CheckTestState();
    }
}
