
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

    [SerializeField] string NotYetRunColorHash;
    [SerializeField] string ExpectedPassColorHash;
    [SerializeField] string SurprisePassColorHash;
    [SerializeField] string ExpectedFailColorHash;
    [SerializeField] string SurpriseFailColorHash;

    string ColorHashFromResult(TestResults result)
    {
        string mainColor = NotYetRunColorHash;

        switch (result)
        {
            case TestResults.NotYetRun:
                mainColor = NotYetRunColorHash;
                break;
            case TestResults.ExpectedPass:
                mainColor = ExpectedPassColorHash;
                break;
            case TestResults.SurprisePass:
                mainColor = SurprisePassColorHash;
                break;
            case TestResults.ExpectedFail:
                mainColor = ExpectedFailColorHash;
                break;
            case TestResults.SurpriseFail:
                mainColor = SurpriseFailColorHash;
                break;
            default:
                break;
        }

        return mainColor;
    }

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

        Debug.Log("Controller complete");

        //Initialize first results
        UpdateAllTests();

        Debug.Log("All tests initialized");
    }

    void UpdateAllTests()
    {
        foreach (BaseTest test in Tests)
        {
            testResults[test.TestIndex] = $"{test.TestName}:\n";
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
        testResults[test.TestIndex] = $"{test.TestName}:\n";

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

        string resultString = "";

        switch (result)
        {
            case TestResults.NotYetRun:
                resultString = "Not yet run";
                break;
            case TestResults.ExpectedPass:
                resultString = "Passed as expected";
                break;
            case TestResults.SurprisePass:
                resultString = "Passed surprisingly";
                break;
            case TestResults.ExpectedFail:
                resultString = "Failed as expected";
                break;
            case TestResults.SurpriseFail:
                resultString = "Failed unexpectedly";
                break;
            default:
                break;
        }

        string colorValue = ColorHashFromResult(result);

        string knownIssueString = PlatformString(knownIssueCliendSim, knownIssueDesktop, knownIssuePCVR, knownIssueQuest);

        testResults[source.TestIndex] += $"{description}: <color=#{colorValue}>{resultString}</color>. {knownIssueString}.\n";
    }

    string PlatformString(bool knownIssueCliendSim, bool knownIssueDesktop, bool knownIssuePCVR, bool knownIssueQuest)
    {
        string returnString;
        if (!knownIssueCliendSim && !knownIssueDesktop && !knownIssuePCVR && !knownIssueQuest)
        {
            returnString = "Not a known issue";
        }
        else if (knownIssueCliendSim && knownIssueDesktop && knownIssuePCVR && knownIssueQuest)
        {
            returnString = $"<color=#{ExpectedFailColorHash}>Known issue on all platforms</color>";
        }
        else
        {
            int counter = 0;
            if (knownIssueCliendSim) counter++;
            if (knownIssueDesktop) counter++;
            if (knownIssuePCVR) counter++;
            if (knownIssueQuest) counter++;

            returnString = $"<color=#{ExpectedFailColorHash}>Known issue on ";

            if(counter == 1)
            {
                if (knownIssueCliendSim) returnString += "ClientSim";
                else if (knownIssueDesktop) returnString += "Desktop";
                else if (knownIssuePCVR) returnString += "PCVR";
                else if (knownIssueQuest) returnString += "Quest";

                return returnString += "</color>";
            }

            if (knownIssueCliendSim)
            {
                returnString += "ClientSim";
                counter--;
                if (counter > 1) returnString += ", ";
            }
            if (knownIssueDesktop)
            {
                if (counter == 1)
                {
                    returnString += " and Desktop</color>";
                    return returnString;
                }

                returnString += "Desktop";
                counter--;
                if (counter > 1) returnString += ", ";
            }
            if (knownIssuePCVR)
            {
                if (counter == 1)
                {
                    returnString += " and PCVR</color>";
                    return returnString;
                }

                returnString += "PCVR";
            }

            returnString += " and Quest</color>";
        }

        return returnString;
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