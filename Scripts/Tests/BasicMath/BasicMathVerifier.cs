
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BasicMathVerifier : UdonSharpBehaviour
{
    TestController linkedTestController;

    void Setup(TestController linkedTestController)
    {
        this.linkedTestController = linkedTestController;
    }

    void Start()
    {
        
    }

    void RunTests()
    {
        linkedTestController.TestFunction(1 + 1 == 2, "Basic integer addition", TestTypes.Math);
        linkedTestController.TestFunction(2 * 3 == 6, "Basic integer multiplication", TestTypes.Math);
        linkedTestController.TestFunction(6 / 3 == 2, "Basic integer division", TestTypes.Math);
        linkedTestController.TestFunction(5 / 3 == 2, "Integer division with rounding", TestTypes.Math);
        int a = 2147483647;
        linkedTestController.TestFunction(a + 2 == -2147483647, "Integer division with rounding", TestTypes.Math);
    }
}
