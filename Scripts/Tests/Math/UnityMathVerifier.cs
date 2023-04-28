
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
        //Default values

        //Float Vector math
        Vector3 V3A;

        V3A = new Vector3(1f, 1f, 1f);
        TestFunction(V3A.x == 1f && V3A.y == 1f && V3A.z == 1f, "Vector3 constructor", "");

        //Int Vector math
        Vector3Int V3IA;
        Vector2Int V2IA;

        V2IA = new Vector2Int(1, 1);
        TestFunction(V2IA.x == 1 && V2IA.y == 1, "Vector2Int constructor", "https://discord.com/channels/652715801714884620/675466846127915019/997185132194705509");

        V3IA = new Vector3Int(5, 5, 5);
        TestFunction(V3IA.x == 5 && V3IA.y == 5 && V3IA.z == 5, "Vector3Int constructor", "https://discord.com/channels/652715801714884620/675466846127915019/997185132194705509");

        V2IA = Vector2Int.zero;
        TestFunction(V2IA.x == 0 && V2IA.y == 0, "Vector2Int.zero", "");

        V3IA = Vector3Int.zero;
        TestFunction(V3IA.x == 0 && V3IA.y == 0 && V3IA.z == 0, "Vector3Int.zero", "");

        V2IA = 3 * Vector2Int.one;
        TestFunction(V2IA.x == 3 && V2IA.y == 3, "n * Vector2Int.one", "");

        V3IA = 3 * Vector3Int.one;
        TestFunction(V3IA.x == 3 && V3IA.y == 3 && V3IA.z == 3, "n * Vector3Int.one", "");

        V3IA.x = 0;
        V3IA.y = 1;
        V3IA.z = 2;

        TestFunction(V3IA.x == 0 && V3IA.y == 1 && V3IA.z == 2, "Vector3Int.x .y .z = value", "");
        
        V2IA.Clamp(Vector2Int.one, Vector2Int.one);
        TestFunction(V2IA.x == 1 && V2IA.y == 1, "Vector2Int.Clamp", "");

        V3IA.Clamp(Vector3Int.one, Vector3Int.one);
        TestFunction(V3IA.x == 1 && V3IA.y == 1 && V3IA.z == 1, "Vector3Int.Clamp", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/vector3intclamp-doesnt-change-anything");
    }

}
