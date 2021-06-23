using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public GameObject bulletObject;
    private Transform frontPosition;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public int playerID = -1;
    private int attackPower;
    private int speed;
    private float maxShotDelay;
    private float curShotDelay;
    private float xLength;
    private float yLength;

    private bool isTouchTop;
    private bool isTouchBottom;
    private bool isTouchRight;
    private bool isTouchLeft;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        frontPosition = transform.Find("Front Position");

        if (photonView.IsMine)
            spriteRenderer.color = Color.blue;
        else
            spriteRenderer.color = Color.red;

        attackPower = 1;
        speed = 5;
        maxShotDelay = 0.2f;
        curShotDelay = 0;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] parameters = info.photonView.InstantiationData;

        if (parameters != null)
        {
            if (parameters.Length >= 1)
            {
                playerID = (int)parameters[0];
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine && GameController.gameIng)
        {
            SetLength();
            Move();
            Fire();
            Reload();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }

    private void SetLength()
    {
        xLength = frontPosition.position.x - transform.position.x;
        yLength = frontPosition.position.y - transform.position.y;
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        transform.position += (new Vector3(v * xLength, v * yLength, 0) * speed * Time.deltaTime);
        transform.Rotate(0, 0, h * speed * -30 * Time.deltaTime);

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            anim.SetInteger("Input", (int)h);
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1") && curShotDelay >= maxShotDelay)
        {
            object[] parameters = new object[4];
            parameters[0] = playerID;
            parameters[1] = attackPower;
            parameters[2] = xLength;
            parameters[3] = yLength;

            PhotonNetwork.Instantiate(bulletObject.name, transform.position, transform.rotation, 0, parameters);
            curShotDelay = 0;
        }
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}