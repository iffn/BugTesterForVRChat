using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace BugTesterForVRChat
{
    public class OtherIssuesLister : UdonSharpBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI linkedIssueText;
        [SerializeField] LinkField LinkFieldPrefab;
        [SerializeField] Transform LinkHolder;

        void Start()
        {
            AddIssue("Image loader throws error after failed load", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/using-image-loader-after-a-failed-load-results-in-an-error");
            AddIssue("OnTriggerExit does not fire when using stations or leaving the world (also respawn?)", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/1097-onplayertriggerexit-does-not-fire-on-player-leave-the-instance-or-enter-sta");
            AddIssue("Array reference not triggering OnValueChanged", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/changing-array-reference-does-not-trigger-onvariablechange");
            AddIssue("OnDeserialization not called unless RequestSerialization called at least once", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/1287-ondeserialization-no-longer-called-for-late-joiners-for-manual-synced-objec");
            AddIssue("Object pool objects: OnDeserialization not called for late joiners", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/1287-ondeserialization-no-longer-being-called-on-manual-synced-objects-unless-re");
            AddIssue("VRCPlayerApi GetPosition and GetRotation can throw error after remote player leaves world", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/vrcplayerapi-getposition-and-getrotation-can-throw-due-to-a-null-ref-if-checked");
            AddIssue("UdonBehavior[] array fails", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/589-udonbehaviour-array-type-is-not-defined");
            AddIssue("VRCObjectSync.Respawn() throws error if GameObject is always disabled", "https://feedback.vrchat.com/vrchat-udon-closed-alpha-bugs/p/nullreferenceexception-on-vrcobjectsyncrespawn-if-its-gameobject-is-never-activa");
            AddIssue("Pre- and Postserialization not called on ClientSim", "https://github.com/vrchat-community/ClientSim/issues/74");
            AddIssue("(short)((int) myEnum) crashes script", "https://discord.com/channels/652715801714884620/675459259441610807/1098295483413307402");
        }

        public void AddIssue(string description, string link)
        {
            linkedIssueText.text += description + "\n";

            GameObject newHolder = Instantiate(LinkFieldPrefab.gameObject, LinkHolder);

            newHolder.transform.localScale = Vector3.one;

            LinkField field = newHolder.GetComponent<LinkField>();

            field.Setup(link);
        }
    }
}