
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class ResultDisplay : UdonSharpBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI Description;
    [SerializeField] Image MainState; 
    [SerializeField] Image ClientSimState; 
    [SerializeField] Image DesktopState; 
    [SerializeField] Image PCVRState; 
    [SerializeField] Image QuestState;

    [SerializeField] Color ExpectedPassColor;
    [SerializeField] Color SurprisePassColor;
    [SerializeField] Color ExpectedFailColor;
    [SerializeField] Color SurpriseFailColor;

    void Start()
    {
        
    }
}
