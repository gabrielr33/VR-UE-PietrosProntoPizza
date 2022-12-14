using System.IO;
using Input;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PlateSpawner : MonoBehaviour
    {
        private PlayerInputController _inputController;

        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
        }

        private void OnTriggerStay(Collider other)
        {
            if ((other.tag.Equals("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (other.tag.Equals("LeftController") && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                HandGrabber handGrabber = other.GetComponent<HandGrabber>();

                if (handGrabber != null && handGrabber.GrabbedObject == null)
                {
                    GameObject plate = PhotonNetwork.Instantiate("Prefabs\\Plate", handGrabber.transform.position, Quaternion.identity);
                    plate.transform.SetParent(handGrabber.transform);
                    plate.transform.localPosition = new Vector3(0.016f, -0.071f, 0.13f);
                    plate.GetComponent<Rigidbody>().isKinematic = true;

                    handGrabber.GrabbedObject = plate.transform;
                }
            }
        }
    }
}