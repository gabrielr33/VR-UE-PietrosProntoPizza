using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class CapsuleManager : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private XROrigin rig;
    private float additionalHeight = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        characterController.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsulecenter = transform.InverseTransformPoint(rig.Camera.gameObject.transform.position);
        characterController.center = new Vector3(capsulecenter.x, characterController.height / 2.0f + characterController.skinWidth, capsulecenter.z);
    }
}
