
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace BugTesterForVRChat
{
    public class ResultBlockDisplay : UdonSharpBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI LinkedDescription;
        [SerializeField] Image LinkedMainState;
        [SerializeField] Image LinkedClientSimState;
        [SerializeField] Image LinkedDesktopState;
        [SerializeField] Image LinkedPCVRState;
        [SerializeField] Image LinkedQuestState;
        [SerializeField] InputField LinkedInputField;

        [SerializeField] Color NotYetRunColor;
        [SerializeField] Color ExpectedPassColor;
        [SerializeField] Color SurprisePassColor;
        [SerializeField] Color ExpectedFailColor;
        [SerializeField] Color SurpriseFailColor;

        public void SetDisplay(string description, bool happensOnClientSim, bool happensOnDesktop, bool happensInPCVR, bool happensOnQuest, string knownIssue, TestResults result)
        {
            Debug.Log(description);

            LinkedDescription.text = description;

            Color mainColor = NotYetRunColor;

            switch (result)
            {
                case TestResults.NotYetRun:
                    mainColor = NotYetRunColor;
                    break;
                case TestResults.ExpectedPass:
                    mainColor = ExpectedPassColor;
                    break;
                case TestResults.SurprisePass:
                    mainColor = SurprisePassColor;
                    break;
                case TestResults.ExpectedFail:
                    mainColor = ExpectedFailColor;
                    break;
                case TestResults.SurpriseFail:
                    mainColor = SurpriseFailColor;
                    break;
                default:
                    break;
            }

            LinkedMainState.color = mainColor;

            LinkedClientSimState.color = happensOnClientSim ? ExpectedFailColor : NotYetRunColor;
            LinkedDesktopState.color = happensOnDesktop ? ExpectedFailColor : NotYetRunColor;
            LinkedPCVRState.color = happensInPCVR ? ExpectedFailColor : NotYetRunColor;
            LinkedQuestState.color = happensOnQuest ? ExpectedFailColor : NotYetRunColor;

            LinkedInputField.text = knownIssue;
        }

        void Start()
        {

        }
    }
}