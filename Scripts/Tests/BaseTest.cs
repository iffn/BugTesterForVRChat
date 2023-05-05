using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace BugTesterForVRChat
{
    public abstract class BaseTest : UdonSharpBehaviour
    {
        protected TestController linkedTestController;

        public int TestIndex { get; private set; }
        public abstract string TestName { get; }

        public Platforms CurrentPlatform { get; private set; }

        public virtual void Setup(TestController linkedTestController, int testIndex, Platforms currentPlatform)
        {
            this.linkedTestController = linkedTestController;

            TestIndex = testIndex;

            CurrentPlatform = currentPlatform;
        }

        public abstract void SendTestStatesToController();
    }
}