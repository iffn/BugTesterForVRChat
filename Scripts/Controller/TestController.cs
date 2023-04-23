
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestController : UdonSharpBehaviour
{
    string passedTests = "";
    string failedTests = "";

    int passCount = 0;
    int failCount = 0;

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
}

public enum TestTypes
{
    Math,
    Input
}
