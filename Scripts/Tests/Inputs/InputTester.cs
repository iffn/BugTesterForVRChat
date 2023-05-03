
using System.Runtime.InteropServices.WindowsRuntime;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Collections;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class InputTester : BaseTest
{
    [SerializeField] InputEventOnDisabledScriptTester[] LinkedInputEventOnDisabledScriptTesters;

    //General
    bool setupComplete = false;
    bool inVR;
    bool useAndGrabAreTheSame;

    //Double events
    string[] doubleInputMessages;
    float[] lastInputTimes;
    TestStates[] doubleInputTested;
    DoubleInputTests[] allDoubleInputTestValues;
    bool[] inputEventHappensOnCurrentPlatform;
    string[] doubleInputTestsAsString = new string[] //Enum to string conversion not avaialble in U#
    {
        "UseDownRight",
        "UseUpRight",
        "GrabDownRight",
        "GrabUpRight",
        "DropDownRight",
        "DropUpRight",
        "UseDownLeft",
        "UseUpLeft",
        "GrabDownLeft",
        "GrabUpLeft",
        "DropDownLeft",
        "DropUpLeft",
        "JumpDown",
        "JumpUp",
        "LookHorizontal",
        "LookVertical",
        "MoveHorizontal",
        "MoveVertical"
    };

    bool[] inputEventHappensOnPC = new bool[]
    {
        true, //UseDownRight
        true, //UseUpRight
        true, //GrabDownRight
        true, //GrabUpRight
        true, //DropDownRight
        true, //DropUpRight
        false, //UseDownLeft
        false, //UseUpLeft
        false, //GrabDownLeft
        false, //GrabUpLeft
        false, //DropDownLeft
        false, //DropUpLeft
        true, //JumpDown
        true, //JumpUp
        true, //LookHorizontal
        true, //LookVertical
        true, //MoveHorizontal
        true //MoveVertical
    };

    bool[] inputEventHappensInVR = new bool[]
    {
        true, //UseDownRight
        true, //UseUpRight
        true, //GrabDownRight
        true, //GrabUpRight
        true, //DropDownRight
        true, //DropUpRight
        true, //UseDownLeft
        true, //UseUpLeft
        true, //GrabDownLeft
        true, //GrabUpLeft
        true, //DropDownLeft
        true, //DropUpLeft
        true, //JumpDown
        true, //JumpUp
        true, //LookHorizontal
        true, //LookVertical
        true, //MoveHorizontal
        true //MoveVertical
    };

    string[] knownDoubleInputLink = new string[]
    {
        "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/1275-inputuse-is-called-twice-per-mouse-click", //UseDownRight
        "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/1275-inputuse-is-called-twice-per-mouse-click", //UseUpRight
        "", //GrabDownRight
        "", //GrabUpRight
        "", //DropDownRight
        "", //DropUpRight
        "", //UseDownLeft
        "", //UseUpLeft
        "", //GrabDownLeft
        "", //GrabUpLeft
        "", //DropDownLeft
        "", //DropUpLeft
        "", //JumpDown
        "", //JumpUp
        "", //LookHorizontal
        "", //LookVertical
        "", //MoveHorizontal
        "" //MoveVertical
    };

    bool[] knownDoubleInputIssueClientSim = new bool[]
    {
        false, //UseDownRight
        false, //UseUpRight
        false, //GrabDownRight
        false, //GrabUpRight
        false, //DropDownRight
        false, //DropUpRight
        false, //UseDownLeft
        false, //UseUpLeft
        false, //GrabDownLeft
        false, //GrabUpLeft
        false, //DropDownLeft
        false, //DropUpLeft
        false, //JumpDown
        false, //JumpUp
        false, //LookHorizontal
        false, //LookVertical
        false, //MoveHorizontal
        false //MoveVertical
    };

    bool[] knownDoubleInputIssueDesktop = new bool[]
    {
        true, //UseDownRight
        true, //UseUpRight
        false, //GrabDownRight
        false, //GrabUpRight
        false, //DropDownRight
        false, //DropUpRight
        false, //UseDownLeft
        false, //UseUpLeft
        false, //GrabDownLeft
        false, //GrabUpLeft
        false, //DropDownLeft
        false, //DropUpLeft
        false, //JumpDown
        false, //JumpUp
        false, //LookHorizontal
        false, //LookVertical
        false, //MoveHorizontal
        false //MoveVertical
    };

    bool[] knownDoubleInputIssuePCVR = new bool[]
    {
        false, //UseDownRight
        false, //UseUpRight
        false, //GrabDownRight
        false, //GrabUpRight
        false, //DropDownRight
        false, //DropUpRight
        false, //UseDownLeft
        false, //UseUpLeft
        false, //GrabDownLeft
        false, //GrabUpLeft
        false, //DropDownLeft
        false, //DropUpLeft
        false, //JumpDown
        false, //JumpUp
        false, //LookHorizontal
        false, //LookVertical
        false, //MoveHorizontal
        false //MoveVertical
    };

    bool[] knownDoubleInputIssueQuest = new bool[]
    {
        false, //UseDownRight
        false, //UseUpRight
        false, //GrabDownRight
        false, //GrabUpRight
        false, //DropDownRight
        false, //DropUpRight
        false, //UseDownLeft
        false, //UseUpLeft
        false, //GrabDownLeft
        false, //GrabUpLeft
        false, //DropDownLeft
        false, //DropUpLeft
        false, //JumpDown
        false, //JumpUp
        false, //LookHorizontal
        false, //LookVertical
        false, //MoveHorizontal
        false //MoveVertical
    };

    //Disabled scripts
    TestStates[] inputsOnDisabledScriptNotGettingCalledStates;
    string[] disabledScriptDescriptions;
    string[] disabledScriptLinks;
    bool[] knownDisabledScriptIssue;
    bool disabledScriptInputsChecked = false;

    //CallUseBeforeGrab
    TestStates UseCalledBeforeGrabOnPCandVive;
    float lastInputUseTimeForInputOrder;
    float lastInputGrabTimeForInputOrder;

    string callUseBeforeGrabLink = "https://github.com/vrchat-community/ClientSim/issues/71";

    bool[] callUseBeforeGrabIssues = new bool[]
    {
        true, //ClientSim
        false, //Desktop
        false, //PCVR
        false //Quest
    };

    public override string TestName
    {
        get
        {
            return "VRChat input events";
        }
    }

    public override void Setup(TestController linkedTestController, int testIndex, Platforms currentPlatform)
    {
        //General
        base.Setup(linkedTestController, testIndex, currentPlatform);
        inVR = Networking.LocalPlayer.IsUserInVR();

        useAndGrabAreTheSame = !inVR;

        string[] controllers = Input.GetJoystickNames();

        foreach (string controller in controllers)
        {
            if (!controller.ToLower().Contains("vive")) continue;

            useAndGrabAreTheSame = true;
            break;
        }

        //Double inputs
        // int maxTestTypeValue = Enum.GetValues(typeof(TestTypes)).Cast<int>().Max(); //Not exposed in U#
        int maxTestTypeValue = doubleInputTestsAsString.Length;

        inputEventHappensOnCurrentPlatform = inVR ? inputEventHappensInVR : inputEventHappensOnPC;

        doubleInputMessages = new string[maxTestTypeValue];
        lastInputTimes = new float[maxTestTypeValue];
        allDoubleInputTestValues = new DoubleInputTests[maxTestTypeValue];
        doubleInputTested = new TestStates[maxTestTypeValue];

        for(int i = 0; i< maxTestTypeValue; i++)
        {
            doubleInputMessages[i] = $"{doubleInputTestsAsString[i]} not getting called twice";
            lastInputTimes[i] = Mathf.Infinity;
            allDoubleInputTestValues[i] = (DoubleInputTests)i;
            doubleInputTested[i] = TestStates.NotYetRun; //Should be done by default and hopefully is
        }

        //InputsOnDisabledScriptNotGettingCalled
        inputsOnDisabledScriptNotGettingCalledStates = new TestStates[LinkedInputEventOnDisabledScriptTesters.Length];
        disabledScriptDescriptions = new string[LinkedInputEventOnDisabledScriptTesters.Length];
        disabledScriptLinks = new string[LinkedInputEventOnDisabledScriptTesters.Length];
        knownDisabledScriptIssue = new bool[LinkedInputEventOnDisabledScriptTesters.Length];

        for (int i = 0;i< LinkedInputEventOnDisabledScriptTesters.Length; i++)
        {
            inputsOnDisabledScriptNotGettingCalledStates[i] = TestStates.NotYetRun;
            LinkedInputEventOnDisabledScriptTesters[i].Setup(this, i);
            disabledScriptLinks[i] = LinkedInputEventOnDisabledScriptTesters[i].KnownLink;
            disabledScriptDescriptions[i] = LinkedInputEventOnDisabledScriptTesters[i].Description;
            knownDisabledScriptIssue[i] = LinkedInputEventOnDisabledScriptTesters[i].KnownIssue;
        }

        //Finalize
        setupComplete = true;
    }

    public override void SendTestStatesToController()
    {
        for(int i = 0; i< doubleInputTestsAsString.Length; i++)
        {
            if (!inputEventHappensOnCurrentPlatform[i]) continue;

            linkedTestController.TestFunctionReply(doubleInputTested[i], doubleInputMessages[i], knownDoubleInputIssueClientSim[i], knownDoubleInputIssueDesktop[i], knownDoubleInputIssuePCVR[i], knownDoubleInputIssueQuest[i], knownDoubleInputLink[i], TestTypes.Input, this);
        }

        for (int i = 0; i < LinkedInputEventOnDisabledScriptTesters.Length; i++)
        {
            linkedTestController.TestFunctionReply(inputsOnDisabledScriptNotGettingCalledStates[i], disabledScriptDescriptions[i], knownDisabledScriptIssue[i], disabledScriptLinks[i], TestTypes.Input, this);
        }

        if(useAndGrabAreTheSame) linkedTestController.TestFunctionReply(UseCalledBeforeGrabOnPCandVive, "Use is called before grab on Desktop and Vive", callUseBeforeGrabIssues[0], callUseBeforeGrabIssues[1], callUseBeforeGrabIssues[2], callUseBeforeGrabIssues[3], callUseBeforeGrabLink, TestTypes.Input, this);
    }

    //Example function replicated with arrays:
    /*
    float lastInputUseDownTime = Mathf.Infinity;
    bool inputUseDownDoubleInputTested = false;
    readonly string inputUseDownDoubleInputMessage = "inputUseDown not getting called twice";

    private void Update()
    {
        if (!inputUseDownDoubleInputTested)
        {
            if (Time.time < lastInputUseDownTime) return;

            inputUseDownDoubleInputTested = true;
            linkedTestController.TestFunction(true, inputUseDownDoubleInputMessage, TestTypes.Input);
        }
    }

    public override void InputUse(bool value, UdonInputEventArgs args)
    {
        if (value)
        {
            if (lastInputUseDownTime == Time.time)
            {
                inputUseDownDoubleInputTested = true;
                linkedTestController.TestFunction(false, inputUseDownDoubleInputMessage, TestTypes.Input);
            }
            else lastInputUseDownTime = Time.time;
        }
    }
    */

    private void Update()
    {
        if (!setupComplete)
        {
            Debug.Log("Setup not complete");
            return;
        }

        //Double input tests
        foreach(DoubleInputTests test in allDoubleInputTestValues)
        {
            CheckInputInUpdate(test);
        }
    }

    void CheckInputInUpdate(DoubleInputTests test)
    {
        int index = (int)test;

        if (Time.time < lastInputTimes[index]) return;

        if (!disabledScriptInputsChecked)
        {
            for (int i = 0; i < LinkedInputEventOnDisabledScriptTesters.Length; i++)
            {
                if (inputsOnDisabledScriptNotGettingCalledStates[i] == TestStates.NotYetRun)
                {
                    inputsOnDisabledScriptNotGettingCalledStates[i] = LinkedInputEventOnDisabledScriptTesters[i].ShouldBeCalled ? TestStates.Failed : TestStates.Passed;
                }
            }

            disabledScriptInputsChecked = true;
        }

        if (doubleInputTested[index] != TestStates.NotYetRun) return;

        doubleInputTested[index] = TestStates.Passed;

        linkedTestController.UpdateTest(this);
    }

    void CheckInputInEvent(DoubleInputTests test)
    {
        int index = (int)test;

        if (doubleInputTested[index] == TestStates.NotYetRun) //Assumption: Doesn't happen when confirmed it doesn't.
        {
            if (lastInputTimes[index] == Time.time)
            {
                doubleInputTested[index] = TestStates.Failed;

                linkedTestController.UpdateTest(this);
            }
        }

        lastInputTimes[index] = Time.time;
    }

    public override void InputUse(bool value, UdonInputEventArgs args)
    {
        if(useAndGrabAreTheSame && UseCalledBeforeGrabOnPCandVive == TestStates.NotYetRun)
        {
            if(lastInputGrabTimeForInputOrder == Time.time)
            {
                UseCalledBeforeGrabOnPCandVive = TestStates.Failed;
            }

            lastInputUseTimeForInputOrder = Time.time;
        }

        if (value)
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.UseDownRight);
            else CheckInputInEvent(DoubleInputTests.UseDownLeft);
        }
        else
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.UseUpRight);
            else CheckInputInEvent(DoubleInputTests.UseUpLeft);
        }
    }

    public override void InputGrab(bool value, UdonInputEventArgs args)
    {
        if (useAndGrabAreTheSame && UseCalledBeforeGrabOnPCandVive == TestStates.NotYetRun)
        {
            if (lastInputUseTimeForInputOrder == Time.time)
            {
                UseCalledBeforeGrabOnPCandVive = TestStates.Passed;
            }

            lastInputGrabTimeForInputOrder = Time.time;
        }

        if (value)
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.GrabDownRight);
            else CheckInputInEvent(DoubleInputTests.GrabDownLeft);
        }
        else
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.GrabUpRight);
            else CheckInputInEvent(DoubleInputTests.GrabUpLeft);
        }
    }

    public override void InputDrop(bool value, UdonInputEventArgs args)
    {
        if (value)
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.DropDownRight);
            else CheckInputInEvent(DoubleInputTests.DropDownLeft);
        }
        else
        {
            if (args.handType == HandType.RIGHT) CheckInputInEvent(DoubleInputTests.DropUpRight);
            else CheckInputInEvent(DoubleInputTests.DropUpLeft);
        }
    }

    public override void InputLookVertical(float value, UdonInputEventArgs args)
    {
        CheckInputInEvent(DoubleInputTests.LookHorizontal);
    }

    public override void InputLookHorizontal(float value, UdonInputEventArgs args)
    {
        CheckInputInEvent(DoubleInputTests.LookVertical);
    }

    public override void InputMoveHorizontal(float value, UdonInputEventArgs args)
    {
        CheckInputInEvent(DoubleInputTests.MoveHorizontal);
    }

    public override void InputMoveVertical(float value, UdonInputEventArgs args)
    {
        CheckInputInEvent(DoubleInputTests.MoveVertical);
    }

    public override void InputJump(bool value, UdonInputEventArgs args)
    {
        if (value)
        {
            CheckInputInEvent(DoubleInputTests.JumpDown);
        }
        else
        {
            CheckInputInEvent(DoubleInputTests.JumpUp);
        }
    }

    public void InputEventReceivedFromDisabledScript(InputEventOnDisabledScriptTester linkedTester, TestStates newState)
    {
        if (inputsOnDisabledScriptNotGettingCalledStates[linkedTester.Index] == newState) return;

        inputsOnDisabledScriptNotGettingCalledStates[linkedTester.Index] = newState;
        linkedTestController.UpdateTest(this);
    }
}

public enum DoubleInputTests
{
    UseDownRight,
    UseUpRight,
    GrabDownRight,
    GrabUpRight,
    DropDownRight,
    DropUpRight,
    UseDownLeft,
    UseUpLeft,
    GrabDownLeft,
    GrabUpLeft,
    DropDownLeft,
    DropUpLeft,
    JumpDown,
    JumpUp,
    LookHorizontal,
    LookVertical,
    MoveHorizontal,
    MoveVertical//Make sure to also update doubleInputTestsAsString
}
