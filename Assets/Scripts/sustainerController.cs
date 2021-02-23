using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class sustainerController : MonoBehaviour
{

    // 地面の位置
    private float groundLevel = 0.5f;


    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;


    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;

    //キャラを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;

    //左ボタン押下の判定
    private bool isLButtonDown = false;

    //右ボタン押下の判定
    private bool isRButtonDown = false;

    //ジャンプボタン押下の判定
    private bool isJButtonDown = false;

    //前進ボタン押下の判定
    private bool isRunButtonDown = false;

    //前方向の速度
    //private float velocityZ = 16f;
    //横方向の速度（追加）
    //private float velocityX = 10f;
    //左右の移動できる範囲（追加）
    //private float movableRange = 3.4f;
    //スピード設定
    //public float speed = 10.0f;
    //public float rotationSpeed = 10.0f;

    //上方向の速度（追加）
    private float velocityY = 20f;

    // Use this for initialization
    void Start()
    {
        if (myPV.IsMine)    //自キャラであれば実行
        {

            //Animatorコンポーネントを取得
            this.myAnimator = GetComponent<Animator>();

            //いきなり走り始めるアニメーションを開始
            //this.myAnimator.SetFloat("Speed", 1);

            //Rigidbodyコンポーネントを取得（追加）
            this.myRigidbody = GetComponent<Rigidbody>();

            //RunボタンのPointerDownにGetMyRunButtonDownを登録
            EventTrigger Run_trigger = GameObject.Find("RunButton").GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(data => GetMyRunButtonDown());
            Run_trigger.triggers.Add(entry);
            //RunButton のUpで止まる
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerUp;
            entry2.callback.AddListener(data => GetMyRunButtonUp());
            Run_trigger.triggers.Add(entry2);

            //Leftボタン
            EventTrigger Left_trigger = GameObject.Find("LeftButton").GetComponent<EventTrigger>();
            EventTrigger.Entry Left_entry = new EventTrigger.Entry();
            Left_entry.eventID = EventTriggerType.PointerDown;
            Left_entry.callback.AddListener(data => GetMyLeftButtonDown());
            Left_trigger.triggers.Add(Left_entry);
            //LeftButton のUpで止まる
            EventTrigger.Entry Left_entry2 = new EventTrigger.Entry();
            Left_entry2.eventID = EventTriggerType.PointerUp;
            Left_entry2.callback.AddListener(data => GetMyLeftButtonUp());
            Left_trigger.triggers.Add(Left_entry2);

            //Rightボタン
            EventTrigger Right_trigger = GameObject.Find("RightButton").GetComponent<EventTrigger>();
            EventTrigger.Entry Right_entry = new EventTrigger.Entry();
            Right_entry.eventID = EventTriggerType.PointerDown;
            Right_entry.callback.AddListener(data => GetMyRightButtonDown());
            Right_trigger.triggers.Add(Right_entry);
            //LeftButton のUpで止まる
            EventTrigger.Entry Right_entry2 = new EventTrigger.Entry();
            Right_entry2.eventID = EventTriggerType.PointerUp;
            Right_entry2.callback.AddListener(data => GetMyRightButtonUp());
            Right_trigger.triggers.Add(Right_entry2);


            //JumpボタンのPointerDownにGetMyJButtonDownを登録
            EventTrigger J_trigger = GameObject.Find("JumpButton").GetComponent<EventTrigger>();
            EventTrigger.Entry Jump_entry = new EventTrigger.Entry();
            Jump_entry.eventID = EventTriggerType.PointerDown;
            Jump_entry.callback.AddListener(data => GetMyJumpButtonDown());
            J_trigger.triggers.Add(Jump_entry);
            //JumButton のUpで止まる
            EventTrigger.Entry Jump_entry2 = new EventTrigger.Entry();
            Jump_entry2.eventID = EventTriggerType.PointerUp;
            Jump_entry2.callback.AddListener(data => GetMyJumpButtonUp());
            J_trigger.triggers.Add(Jump_entry2);

        }

    }


    // Update is called once per frame
    void Update()
    {



        //キャラに速度を与える（勝手に走らせるため速度を与える）
        //this.myRigidbody.velocity = new Vector3(0, 0, this.velocityZ);

        //自キャラであれば実行
        if (!myPV.IsMine)
        {
            return;
        }



        //上方向の入力による速度（追加）
        float inputVelocityY = 0;

        //ジャンプしていない時にスペースが押されたらジャンプする（追加）
        if ((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生（追加）
            this.myAnimator.SetBool("Jump", true);
            //上方向への速度を代入（追加）
            inputVelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入（追加）
            inputVelocityY = this.myRigidbody.velocity.y;
        }

        //Jumpステートの場合はJumpにfalseをセットする（追加）
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }


        // 着地しているかどうかを調べる
        //Debug.Log(this.transform.position.y);
        bool isGround = (transform.position.y > this.groundLevel) ? false : true;


        //float inputVelocityX = Input.GetAxis("Horizontal");
        //float inputvelocityZ = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.UpArrow) || this.isRunButtonDown)
        {
            //速度を代入
            this.myAnimator.SetFloat("Speed", 1);
            transform.position += transform.forward * 0.05f;
            myAnimator.SetBool("walk", true);

            GetComponent<AudioSource>().volume = (isGround) ? 1 : 0;
            //this.myAnimator.SetBool("isGround", isGround);
            Debug.Log(isGround);

        }
        else
        {
            //走るのをやめる
            this.myAnimator.SetFloat("Speed", 0);
            myAnimator.SetBool("walk", false);
            //ボリュームを0にする
            GetComponent<AudioSource>().volume = 0;

            //this.myAnimator.SetBool("isGround", isGround);
            Debug.Log(isGround);
        }



        if (Input.GetKey("right") || this.isRButtonDown)
        {
            transform.Rotate(0, 10, 0);
        }
        if (Input.GetKey("left") || this.isLButtonDown)
        {
            transform.Rotate(0, -10, 0);
        }

        //速度を与える(Vector3: X, Y, Z)
        //velocity（速度値変更での移動は非推奨らしい。Rigidbody.AddForceが今後のスタンダード？）
        this.myRigidbody.velocity = new Vector3(0, inputVelocityY, 0);

    }

    //走る処理
    public void GetMyRunButtonDown()
    {
        Debug.Log("down");
        //方向キーか、操作ボタンで移動
        this.isRunButtonDown = true;
        Debug.Log(isRunButtonDown);
    }
    //走るのをやめる処理
    public void GetMyRunButtonUp()
    {
        Debug.Log("up");
        //方向キーか、操作ボタンで移動
        this.isRunButtonDown = false;
        Debug.Log(isRunButtonDown);
    }


    //ジャンプボタンを押した場合の処理（追加）
    public void GetMyJumpButtonDown()
    {
        this.isJButtonDown = true;
    }

    //ジャンプボタンを離した場合の処理（追加）
    public void GetMyJumpButtonUp()
    {
        this.isJButtonDown = false;
    }

    //左ボタンを押し続けた場合の処理（追加）
    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理（追加）
    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }

    //右ボタンを押し続けた場合の処理（追加）
    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理（追加）
    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
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
