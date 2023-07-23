using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyPool
{
    public string poolName;
    public Queue<GameObject> queue;
    public Transform poolHolder;
    public GameObject poolPrefab;
    public MyPool(GameObject Prefab, int iniSize = 0, string Name = "", Transform Holder = null)
    {
        this.poolPrefab = Prefab;
        this.poolHolder = Holder;
        this.poolName = Name;
        if (poolPrefab == null)
        {
            Debug.LogWarning("pool");
        }
        queue = new Queue<GameObject>();
        if (poolHolder == null)                                 //生成所有资源的父节点
        {
           if(this.poolName.Equals("")) poolName = Random.Range(0, 101).ToString();
            this.poolHolder = (new GameObject("Pool-" + this.poolName)).transform;
        }
        if (iniSize > 0)                                        //预加载一些资源，备用
        {
            for (int i = 0; i < iniSize; i++)
            {
                GameObject obj = GameObject.Instantiate(this.poolPrefab, this.poolHolder);
                this.Push(obj);
            }
        }
    }
    //从队列头部取出一个资源
    public GameObject Pop()                                     
    {
        if (queue.Count == 0)
        {
            GameObject obj = GameObject.Instantiate(poolPrefab, poolHolder);
            return obj;
        }
        else
        {
            queue.Peek().SetActive(true);
            return queue.Dequeue();
        }
    }
    //从队列头部取出一个资源,并为它设置位置和旋转
    public GameObject Pop(Vector3 position, Quaternion rotation)
    {
        if (queue.Count == 0)
        {
            GameObject obj = GameObject.Instantiate(poolPrefab, position, rotation);
            obj.transform.SetParent(poolHolder);
            return obj;
        }
        else
        {
            GameObject obj = queue.Dequeue();
            obj.SetActive(true);
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }
    }
    //从队尾添加一个资源，并设置active=false;
    public void Push(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("push空对象");
            return;
        }
        item.SetActive(false);
        queue.Enqueue(item.gameObject);
    }
}

public interface IEnableRecycled
{
    public event System.Action<GameObject> onRecycleEvent;
    public void OnStartRecycle();
}