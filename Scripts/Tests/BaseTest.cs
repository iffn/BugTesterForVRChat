
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public abstract class BaseTest : UdonSharpBehaviour
{
    protected TestController linkedTestController;

    public int TestIndex { get; private set; }
    public abstract string TestName { get; }

    public Platforms CurrentPlatform { get; private set; }

    public virtual void Setup(TestController linkedTestController, int testIndex, Platforms currentPlatform)
    {
        this.linkedTestController = linkedTestController;

        this.TestIndex = testIndex;

        this.CurrentPlatform = currentPlatform;
    }

    public abstract void SendTestStatesToController();
}

