
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace BugTesterForVRChat
{
    public class LinkField : UdonSharpBehaviour
    {
        [SerializeField] InputField LinkedInputField;

        public void Setup(string link)
        {
            gameObject.SetActive(true);

            if (link.Length == 0)
            {
                LinkedInputField.gameObject.SetActive(false);
            }
            else
            {
                LinkedInputField.gameObject.SetActive(true);
                LinkedInputField.text = link;
            }
        }
    }
}