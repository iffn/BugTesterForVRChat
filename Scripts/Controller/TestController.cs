
using System.Collections.Generic;
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

    [SerializeField] TMPro.TextMeshProUGUI linkedOutput;

    [SerializeField] BaseTest[] Tests;
    string[] testResults;

    string[] testStateStrings = new string[]
    {
        "<color=yellow>NotYetRun</color>",
        "<color=#00FFFF>Passed</color>",
        "<color=red>Failed</color>"
    };

    private void Start()
    {
        //Setup
        for(int i = 0; i < Tests.Length; i++)
        {
            Tests[i].Setup(this, i);
        }

        testResults = new string[Tests.Length];

        //Initialize first results
        UpdateAllTests();
    }

    void UpdateAllTests()
    {
        foreach (BaseTest test in Tests)
        {
            testResults[test.TestIndex] = "";
            test.SendTestStatesToController();
        }

        OutputTestResults();
    }

    void OutputTestResults()
    {
        //Update test result field
        string testResult = "";

        foreach (string result in testResults)
        {
            testResult += result + "\n";
        }

        linkedOutput.text = testResult;
    }
    
    public void UpdateTest(BaseTest test)
    {
        //Update results of test
        testResults[test.TestIndex] = "";

        test.SendTestStatesToController();

        OutputTestResults();
    }

    public void TestFunctionReply(TestStates testState, string description, TestTypes testType, BaseTest source)
    {
        testResults[source.TestIndex] += $"{description}: {testStateStrings[(int)testState]}\n";
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
