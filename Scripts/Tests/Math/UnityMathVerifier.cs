
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UnityMathVerifier : BaseTest
{
    public override string TestName
    {
        get
        {
            return "Unity math functions";
        }
    }

    public override void Setup(TestController linkedTestController, int testIndex, Platforms currentPlatform)
    {
        base.Setup(linkedTestController, testIndex, currentPlatform);
    }

    void Start()
    {

    }

    void TestFunction(bool passed, string message, string knownLink = "")
    {
        linkedTestController.TestFunctionReply(passed ? TestStates.Passed : TestStates.Failed, message, knownLink, TestTypes.Input, this);
    }

    public override void SendTestStatesToController()
    {
        Vector3Int V3IA;

        //Default values


        //Vector math
        V3IA = new Vector3Int(0, 1, 2);
        V3IA.Clamp(Vector3Int.one, Vector3Int.one);
        TestFunction(V3IA.x == 1 && V3IA.y == 1 && V3IA.z == 1, "Clamp Vector3Int", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/vector3intclamp-doesnt-change-anything");
    }

}
