
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Image;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

namespace BugTesterForVRChat
{
    public class ImageLoaderTest : UdonSharpBehaviour
    {
        [SerializeField] VRCImageDownloader currentImageDownloader;

        [SerializeField] Material linkedMaterial;
        [SerializeField] VRCUrl url;
        [SerializeField] Image linkedImage;

        void Start()
        {
            currentImageDownloader = new VRCImageDownloader();

            currentImageDownloader.DownloadImage(url, linkedMaterial, (IUdonEventReceiver)this);
        }

        public override void OnImageLoadSuccess(IVRCImageDownload result)
        {
            //Set material to image after load to refresh Unity image
            linkedImage.material = linkedMaterial;
        }
    }
}

