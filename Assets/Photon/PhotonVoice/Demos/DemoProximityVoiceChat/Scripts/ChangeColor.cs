using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(PhotonView))]

public class ChangeColor : MonoBehaviour
{
    private PhotonView photonView;

    private void Start()
    {
        this.photonView = this.GetComponent<PhotonView>();
        if (this.photonView.IsMine)
        {
            Color random = Random.ColorHSV();
            this.photonView.RPC("ChangeColour", RpcTarget.AllBuffered, new Vector3(random.r, random.g, random.b));
        }
    }

    public void ChangeColors()
    {
        this.photonView = this.GetComponent<PhotonView>();
        if (this.photonView.IsMine)
        {
            Color random = Random.ColorHSV();
            this.photonView.RPC("ChangeColour", RpcTarget.AllBuffered, new Vector3(random.r, random.g, random.b));
        Debug.Log(PlayerPrefs.GetString("token"));
        }
    }

    [PunRPC]
    public void ChangeColour(Vector3 randomColor)
    {
        Renderer renderer = this.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", new Color(randomColor.x, randomColor.y, randomColor.z));
    }
}
