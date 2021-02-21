using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    //キャラオブジェクト
    private GameObject sustainer;

    //キャラオブジェクトとカメラの距離
    private float difference;

    // Use this for initialization
    void Start()
    {
        //キャラオブジェクトを取得
        this.sustainer = GameObject.Find("sustainer_prefab");
        //キャラオブジェクとカメラの位置（z座標）の差を求める
        this.difference = sustainer.transform.position.z - this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Unityちゃんの位置に合わせてカメラの位置を移動
        this.transform.position = new Vector3(0, this.transform.position.y, this.sustainer.transform.position.z - difference);
    }
}
