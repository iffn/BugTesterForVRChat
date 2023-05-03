
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class ResultDisplay : UdonSharpBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI LinkedDescription;
    [SerializeField] Image LinkedMainState; 
    [SerializeField] Image LinkedClientSimState; 
    [SerializeField] Image LinkedDesktopState; 
    [SerializeField] Image LinkedPCVRState; 
    [SerializeField] Image LinkedQuestState;
    [SerializeField] InputField LinkedInputField;

    [SerializeField] Color ExpectedPassColor;
    [SerializeField] Color SurprisePassColor;
    [SerializeField] Color ExpectedFailColor;
    [SerializeField] Color SurpriseFailColor;

    public void Setup(string description, bool happensOnClientSim, string knownIssue)
    {

    }

    public void UpdateDisplay(TestResults result)
    {

    }

    void Start()
    {
        
    }
}
