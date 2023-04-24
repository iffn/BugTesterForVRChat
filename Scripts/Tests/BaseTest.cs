
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public abstract class BaseTest : UdonSharpBehaviour
{
    protected TestController linkedTestController;

    public int TestIndex { get; private set; }

    public abstract string TestName { get; }

    public virtual void Setup(TestController linkedTestController, int testIndex)
    {
        this.linkedTestController = linkedTestController;

        this.TestIndex = testIndex;
    }

    public abstract void SendTestStatesToController();
}
