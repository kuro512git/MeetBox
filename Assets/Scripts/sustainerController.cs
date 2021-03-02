using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;



public class sustainerController : Photon.Pun.MonoBehaviourPun
{
    //joystick
    [SerializeField] VariableJoystick m_VariableJoystick;
    [SerializeField] Animator m_Animator;
    [SerializeField] float m_Speed = 1;
    private CharacterController m_Controller;
    private Vector3 m_Direction;
    private Vector3 y_Direction;
    private Camera m_Camera;

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
            //インスタンスにjoystickを登録
            m_VariableJoystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick> ();
            m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            m_Controller = this.gameObject.GetComponent<CharacterController>();

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

        var charaTransformRotate = m_Controller.transform.localRotation.eulerAngles;
        var q = Quaternion.Euler(0.0f, charaTransformRotate.y, 0.0f);
        Vector3 y_Direction = q.eulerAngles;

        //m_Controller.Move(m_Direction * m_Speed * Time.deltaTime);
        m_Controller.Move(m_Direction * m_Speed * Time.deltaTime);

        //if (m_Camera.transform.localRotation.eulerAngles)
        var angle = m_Camera.transform.localRotation.eulerAngles;



        var x = new Vector3(-1, 0, 0);
        Debug.Log(charaTransformRotate.y);


        if (Quaternion.Euler(x) == Quaternion.Euler(angle))
        {
            
        }
        //var v_angle = new Vector3(angle.x, angle.y, angle.z);
       // m_Controller.Move(v_angle * m_Speed * Time.deltaTime);

        //Debug.Log(m_Direction);

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
