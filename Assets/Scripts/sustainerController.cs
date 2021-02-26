using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;



public class sustainerController : MonoBehaviour
{
    //joystick
    [SerializeField] VariableJoystick m_VariableJoystick;
    [SerializeField] Animator m_Animator;
    [SerializeField] float m_Speed = 0.2f;
    private CharacterController m_Controller;
    private Vector3 m_Direction;


    // 地面の位置
    //private float groundLevel = 0.5f;


    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;


    // Use this for initialization
    void Start()
    {
        if (myPV.IsMine)    //自キャラであれば実行
        {
            m_Controller = GetComponent<CharacterController>();

            /*
            //RunボタンのPointerDownにGetMyRunButtonDownを登録
            EventTrigger Run_trigger = GameObject.Find("Variable Joystick").GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(data => GetMyRunButtonDown());
            Run_trigger.triggers.Add(entry);
            //RunButton のUpで止まる
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerUp;
            entry2.callback.AddListener(data => GetMyRunButtonUp());
            Run_trigger.triggers.Add(entry2);
            */
     

        }

    }


    // Update is called once per frame
    void Update()
    {


        //自キャラであれば実行
        if (!myPV.IsMine)
        {
            return;
        }

        //joystick
        if (m_Direction != new Vector3(0, 0, 0))
        {
            transform.localRotation = Quaternion.LookRotation(m_Direction);
        }
        m_Animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(m_Direction.x), Mathf.Abs(m_Direction.z)));
        m_Controller.Move(m_Direction * m_Speed * Time.deltaTime);


    }

    public void FixedUpdate()
    {
        m_Direction = Vector3.forward * m_VariableJoystick.Vertical + Vector3.right * m_VariableJoystick.Horizontal;
    }


    private void OnTriggerEnter(Collider other)
    {

        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            //スコアを加算
            //this.score += 10;

            //ScoreTextに獲得した点数を表示
            //this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //パーティクルの再生
            GetComponent<ParticleSystem>().Play();

            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }



}
