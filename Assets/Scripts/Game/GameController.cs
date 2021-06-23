using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{
    public Slider[] healthBar;  // HealthBar1.value -= 0.1f;
    public GameObject window;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    private List<PhotonView> players = new List<PhotonView>();
    private static GameController instance;

    public static bool gameIng = false;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameController>();

            return instance;
        }
    }

    private void Start()
    {
        window.SetActive(false);
        SpawnPlayer();

        for (int i = 0; i < healthBar.Length; i++)
            healthBar[i].value = 5;

        gameIng = true;
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        object[] parameters = new object[1];
        parameters[0] = localPlayerIndex;
        players.Add( PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation, 0, parameters).GetComponent<PhotonView>() );
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LogIn");
    }

    public void HitDamage(int playerNumber, int attackPower)
    {
        //if (GetComponent<PhotonView>().IsMine)
            photonView.RPC("RPCUpdateHealthBar", RpcTarget.All, playerNumber, attackPower);
    }

    [PunRPC]
    private void RPCUpdateHealthBar(int playerNumber, int attackPower)
    {
        if (healthBar[playerNumber].value > 0)
            healthBar[playerNumber].value -= attackPower;

        Debug.Log(healthBar[0].value + " " + healthBar[1].value);

        if (healthBar[playerNumber].value <= 0)
        {
            GameObject player = players[playerNumber].transform.gameObject;
            player.SetActive(false);

            window.transform.Find("Message Text").GetComponent<Text>().text = "Player " + (2-playerNumber) + " Win!";
            window.SetActive(true);
            gameIng = false;
        }
    }
}
