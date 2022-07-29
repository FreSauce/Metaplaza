using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class MapTransform
{
    public Transform vrTarget;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        IKTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
public class AvatarController : MonoBehaviourPun
{
    [SerializeField] private MapTransform head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransform rightHand;

    [SerializeField] private float turnSmoothness;


    [SerializeField] private Transform IKHead;

    [SerializeField] private Vector3 headBodyOffset;

    private GameObject _VRCamera;

    private void Awake()
    {
        if (photonView.IsMine)
        {
        _VRCamera = GameObject.FindGameObjectWithTag("VRCamera");
        Transform cameraOffset = _VRCamera.transform.Find("Camera Offset");
        Debug.Log(cameraOffset);
        head.vrTarget = cameraOffset.Find("Main Camera"); ;
        leftHand.vrTarget = cameraOffset.Find("LeftHand Controller"); ;
        rightHand.vrTarget = cameraOffset.Find("RightHand Controller");
        }
        
    }

    void LateUpdate()
    {
        if (photonView.IsMine)
        {

        transform.position = IKHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness); ;
        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        rightHand.MapVRAvatar();
        }
    }
}