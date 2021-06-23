using Photon.Pun;
using UnityEngine;

public class BulletInfo : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private int playerID;
    private int attackPower;
    private float xLength;
    private float yLength;
    private int bulletSpeed = 20;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] parameters = info.photonView.InstantiationData;

        if(parameters != null)
        {
            if(parameters.Length >= 4)
            {
                playerID = (int)parameters[0];
                attackPower = (int)parameters[1];
                xLength = (float)parameters[2];
                yLength = (float)parameters[3];
            }
        }
    }

    private void FixedUpdate()
    {
        //if (IsMasterClientLocal)  //  && PhotonNetwork.PlayerList.Length >= 2
            transform.position += (new Vector3(xLength, yLength, 0) * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (IsMasterClientLocal)
        //{
        if (GetComponent<PhotonView>().IsMine)
        {
            GameObject gameObj = collision.gameObject;
            string target = gameObj.tag;
            int damagedPlayerID = -1;
            PlayerController playerInfo = gameObj.GetComponent<PlayerController>();

            if (playerInfo != null)
                damagedPlayerID = playerInfo.GetPlayerID();

            if (target.Equals("Player") && playerID == damagedPlayerID)
                return;

            //if (PhotonNetwork.IsMasterClient)
            //photonView.RPC("RPCRemoveBullet", RpcTarget.All, damagedPlayerID, attackPower);

            if (damagedPlayerID != -1)
                GameController.Instance.HitDamage(damagedPlayerID, attackPower);

            PhotonNetwork.Destroy(gameObject);
        }
        //}
    }

    /*
    [PunRPC]
    private void RPCRemoveBullet(int damagedPlayerID, int attackPower)
    {
        if(damagedPlayerID != -1)
            GameController.Instance.HitDamage(damagedPlayerID, attackPower);

        PhotonNetwork.Destroy(gameObject);
    }
    */
}
