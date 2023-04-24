
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class InputTester : BaseTest
{
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
        "JumpUp"
    };

    string[] doubleInputMessages;
    float[] lastInputTimes;
    TestStates[] doubleInputTested;
    DoubleInputTests[] allDoubleInputTestValues;

    bool setupComplete = false;

    public override string TestName
    {
        get
        {
            return "VRChat input events";
        }
    }

    public override void Setup(TestController linkedTestController, int testIndex)
    {
        base.Setup(linkedTestController, testIndex);

        // int maxTestTypeValue = Enum.GetValues(typeof(TestTypes)).Cast<int>().Max(); //Not exposed in U#
        int maxTestTypeValue = doubleInputTestsAsString.Length;
        
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

        setupComplete = true;
        Debug.Log("Setup complete");
    }

    public override void SendTestStatesToController()
    {
        for(int i = 0; i< doubleInputTestsAsString.Length; i++)
        {
            linkedTestController.TestFunctionReply(doubleInputTested[i], doubleInputMessages[i], TestTypes.Input, this);
        }
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

        if (doubleInputTested[index] == TestStates.NotYetRun)
        {
            if (Time.time < lastInputTimes[index]) return;

            doubleInputTested[index] = TestStates.Passed;

            linkedTestController.UpdateTest(this);
        }
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
        
    }

    public override void InputLookHorizontal(float value, UdonInputEventArgs args)
    {
        
    }

    public override void InputMoveHorizontal(float value, UdonInputEventArgs args)
    {
        
    }

    public override void InputMoveVertical(float value, UdonInputEventArgs args)
    {
        
    }

    public override void InputJump(bool value, UdonInputEventArgs args)
    {
        
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
    DropUp,
    JumpDown,
    JumpUp //Make sure to also update doubleInputTestsAsString
}
