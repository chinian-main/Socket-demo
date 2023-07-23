
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace face) : base(face) { }
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    GameObject playerPrefab;
    private GameObject bulletPrefab;
    private Transform spawnPos;
    private MyPool myPool;
    public MyPool BulletPool
    {
        get
        {
            if (myPool == null)
            {
                myPool = new MyPool(bulletPrefab, 10, "Bullet");
            }
            return myPool;
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        playerPrefab = Resources.Load<GameObject>("Prefab/Player");
        bulletPrefab = Resources.Load<GameObject>("Prefab/Bullet");
    }
    public string CurPlayerID
    {
        get; set;
    }

    public void AddPlayer(MainPack pack)
    {
        spawnPos = GameObject.Find("SpawnPos").transform;

        Debug.Log(pack.Playerpack);
        foreach (var item in pack.Playerpack)
        {
            GameObject g = GameObject.Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
            if (item.Playername.Equals(face.UserName))
            {
                //创建本地角色
                g.AddComponent<Rigidbody2D>().gravityScale = 3;
                // g.AddComponent<UpdateStateRequest>();
                g.AddComponent<UpdateState>();
                g.AddComponent<CharacterController>();
                // g.transform.Find("HandGun/Gun").gameObject.AddComponent<FireRequest>();
                g.transform.Find("HandGun/Gun").gameObject.AddComponent<GunController>();
            }
            else
            {
                //创建其他角色
                
                g.AddComponent<RemoteController>();

                // g.GetComponent<Rigidbody2D>().simulated = false;//防止鬼畜--- 没啥用
            }

            players.Add(item.Playername, g);
        }

    }
    public void RemovePlayer(string id)
    {
        if (players.TryGetValue(id, out GameObject g))
        {
            GameObject.Destroy(g);
            players.Remove(id);
        }
        else
        {
            Debug.Log("移除角色失败");
        }
    }

    public void GameExit()
    {
        foreach (GameObject item in players.Values)
        {
            GameObject.Destroy(item);
        }
        players.Clear();
    }
    public void UpdateState(MainPack pack)
    {
        PosPack posPack = pack.Playerpack[0].Pospack;
        if (players.TryGetValue(pack.Playerpack[0].Playername, out GameObject g))
        {
            Vector2 pos = new Vector2(posPack.PosX, posPack.PosY);
            float playerRot = posPack.RotZ;
            float gunRot = posPack.GunRotZ;
            g.GetComponent<RemoteController>().SetState(pos,playerRot,gunRot);
            // g.transform.position = pos;
            // g.transform.eulerAngles = new Vector3(0, 0, playerRot);
            // g.transform.Find("HandGun/Gun").eulerAngles = new Vector3(0, 0, gunRot);
        }
    }
    public void spawnBullet(MainPack pack)
    {
        Vector3 pos = new Vector3(pack.BulletPack.PosX, pack.BulletPack.PosY, 0);
        float rot = pack.BulletPack.RotZ;
        Vector3 mousePos = new Vector3(pack.BulletPack.MousePosX, pack.BulletPack.MousePosY, 0);

        GameObject g = GameFace.Face.BulletPool.Pop(pos, Quaternion.identity);
        g.GetComponent<Bullet>().onRecycleEvent += GameFace.Face.BulletPool.Push;
        g.GetComponent<Bullet>().Init();
        g.transform.eulerAngles = new Vector3(0, 0, rot);
        Vector2 velocity = (mousePos - pos).normalized * 20;
        g.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
