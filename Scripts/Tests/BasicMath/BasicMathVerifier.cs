
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BasicMathVerifier : BaseTest
{
    public override string TestName
    {
        get
        {
            return "Basic math functions";
        }
    }

    public override void Setup(TestController linkedTestController, int testIndex)
    {
        base.Setup(linkedTestController, testIndex);
    }

    void Start()
    {
        
    }

    void TestFunction(bool passed, string message)
    {
        linkedTestController.TestFunctionReply(passed ? TestStates.Passed : TestStates.Failed, message, TestTypes.Input, this);
    }

    public override void SendTestStatesToController()
    {
        int ia;
        short sa;

        //Default values


        //Basic math
        TestFunction(1 + 1 == 2, "Basic integer addition");
        TestFunction(2 * 3 == 6, "Basic integer multiplication");
        TestFunction(6 / 3 == 2, "Basic integer division");
        TestFunction(5 / 2 == 2, "Integer division with rounding");

        ia = 2147483647;
        TestFunction(ia + 2 == -2147483647, "Integer division with rounding");

        //Casting
        ia = 3;
        sa = (short)ia;
        TestFunction(sa == 3, "Basic int to short casting");

        ia = -2147483647;
        sa = (short)ia;
        TestFunction(sa == 1, "Int to short casting with rounding");
    }

}
