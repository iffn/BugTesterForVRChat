
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

    public Platforms currentPlatform { get; private set; }

    [SerializeField] TMPro.TextMeshProUGUI linkedOutput;
    [SerializeField] Transform TestsResultHolder;
    [SerializeField] ResultBlockDisplay ResultDisplayPrefab;
    [SerializeField] BaseTest[] Tests;

    ResultBlockDisplay[][] resultDisplays;

    string[] testResults;

    string[] testStateStrings = new string[]
    {
        "<color=yellow>NotYetRun</color>",
        "<color=#00FFFF>Passed</color>",
        "<color=red>Failed</color>"
    };

    private void Start()
    {
        #if UNITY_EDITOR
        currentPlatform = Platforms.ClientSim;
        #elif UNITY_ANDROID
            currentPlatform = Platforms.Quest;
        #else
        if (Networking.LocalPlayer.IsUserInVR())
        {
            currentPlatform = Platforms.PCVR;
        }
        else
        {
            currentPlatform = Platforms.Desktop;
        }
        #endif

        //Setup tests
        for (int i = 0; i < Tests.Length; i++)
        {
            Tests[i].Setup(this, i, currentPlatform);
        }
        
        testResults = new string[Tests.Length]; //To be removed

        //Setup results
        resultDisplays = new ResultBlockDisplay[Tests.Length][];

        for(int i = 0; i < resultDisplays.Length; i++)
        {
            resultDisplays[i] = new ResultBlockDisplay[50];

            for (int j = 0; j < resultDisplays[i].Length; j++)
            {
                GameObject newObject = GameObject.Instantiate(ResultDisplayPrefab.gameObject);

                newObject.transform.parent = TestsResultHolder;

                newObject.SetActive(false);

                resultDisplays[i][j] = newObject.transform.GetComponent<ResultBlockDisplay>();
            }
        }

        Debug.Log("Controller complete");

        //Initialize first results
        UpdateAllTests();

        Debug.Log("All tests initialized");
    }

    int testCounter = 0;

    void UpdateAllTests()
    {
        foreach (BaseTest test in Tests)
        {
            testResults[test.TestIndex] = "";
            testCounter = 0;
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

    public void TestFunctionReply(TestStates testState, string description, bool knownIssue, string issueLink, TestTypes testType, BaseTest source)
    {
        TestFunctionReply(testState, description, knownIssue, knownIssue, knownIssue, knownIssue, issueLink, testType, source);
    }

    public void TestFunctionReply(TestStates testState, string description, bool knownIssueCliendSim, bool knownIssueDesktop, bool knownIssuePCVR, bool knownIssueQuest, string issueLink, TestTypes testType, BaseTest source)
    {
        TestResults result = TestResults.NotYetRun;

        if (testState != TestStates.NotYetRun)
        {
            bool knownIssue = false;

            switch (currentPlatform)
            {
                case Platforms.ClientSim:
                    knownIssue = knownIssueCliendSim;
                    break;
                case Platforms.Desktop:
                    knownIssue = knownIssueDesktop;
                    break;
                case Platforms.PCVR:
                    knownIssue = knownIssuePCVR;
                    break;
                case Platforms.Quest:
                    knownIssue = knownIssueQuest;
                    break;
                default:
                    break;
            }

            switch (testState)
            {
                case TestStates.NotYetRun:
                    break;
                case TestStates.Passed:
                    result = knownIssue ? TestResults.SurprisePass : TestResults.ExpectedPass;
                    break;
                case TestStates.Failed:
                    result = knownIssue ? TestResults.ExpectedFail : TestResults.SurpriseFail;
                    break;
                default:
                    break;
            }
        }

        resultDisplays[source.TestIndex][testCounter].SetDisplay(description, knownIssueCliendSim, knownIssueDesktop, knownIssuePCVR, knownIssueQuest, issueLink, result);
        resultDisplays[source.TestIndex][testCounter].gameObject.SetActive(true);

        testCounter++;
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

public enum TestResults
{
    NotYetRun,      //Grey
    ExpectedPass,   //Green
    SurprisePass,   //Blue
    ExpectedFail,   //Yellow
    SurpriseFail    //Purple
}

public enum Platforms
{
    ClientSim,
    Desktop,
    PCVR,
    Quest
}