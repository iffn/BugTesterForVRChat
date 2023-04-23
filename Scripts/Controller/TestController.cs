
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using VRC.SDKBase;
using VRC.Udon;

public class TestController : UdonSharpBehaviour
{
    string passedTests = "";
    string failedTests = "";

    int passCount = 0;
    int failCount = 0;

    [SerializeField] InputTester linkedInputTester;
    [SerializeField] TMPro.TextMeshProUGUI linkedInputTesterOutput;

    string inputResult = "";

    string[] testStateStrings = new string[]
    {
        "NotYetRun",
        "Passed",
        "Failed"
    };

    private void Start()
    {
        linkedInputTester.Setup(this);

        UpdateInputTester();
    }

    public void TestFunction(bool hasPassed, string description, TestTypes testType)
    {
        if (hasPassed)
        {
            passedTests += $"{description} \n";
            passCount++;
        }
        else
        {
            failedTests += $"{description} \n";
            failCount++;
        }

    }
    
    public void UpdateInputTester()
    {
        inputResult = "Inputs:\n";

        linkedInputTester.SendTestStatesToController();

        linkedInputTesterOutput.text = inputResult;
    }

    public void TestFunctionReply(TestStates testState, string description, TestTypes testType)
    {
        switch (testType)
        {
            case TestTypes.Math:
                break;
            case TestTypes.Input:
                inputResult += $"{description}: {testStateStrings[(int)testState]}\n";
                break;
            default:
                break;
        }
    }

    
}

public enum TestTypes
{
    Math,
    Input
}

public enum TestStates
{
    NotYetRun,
    Passed,
    Failed
}
