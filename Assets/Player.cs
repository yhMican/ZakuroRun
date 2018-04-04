using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	int score;
	public UnityEngine.UI.Text scoreValue;
	public UnityEngine.UI.Image nekokanImage;

	public SpriteRenderer nekobako;
	public Sprite[] nekobakoImages;


	float downSpeed; //落下速度
	Rigidbody2D rb; //物理演算コンポーネント
	Animator animCtrl; //アニメーションコントロール
	AudioSource audioSource; //オーディオコンポーネント

	public AudioClip[] sounds; //音を配列で管理する

	public bool IsStop; //停止モード

	// Use this for initialization
	void Start ()
	{ //初期化処理
		rb = GetComponent<Rigidbody2D> ();
		animCtrl = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();

		Reset ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float forwardSpeed;
		if (IsStop)
			forwardSpeed = 0;
		else
			forwardSpeed = 1;

		RaycastHit2D hit;

		//下方向当たり判定設定
		hit = Physics2D.Raycast (transform.position + new Vector3 (-0.32f, -0.32f), Vector2.right, 0.64f); 		
		if (hit.transform != null) { //接地したとき
			downSpeed = 0; 
			animCtrl.SetBool ("IsGround", true);
			if (Input.GetButtonDown ("Jump")  && !IsStop) { //ジャンプのボタン判定
				downSpeed = 6.5f;
				transform.Translate (Vector3.up * 0.01f); //ジャンプがたまにdownspeed = 0 でキャンセルされるのを防ぐ
				audioSource.PlayOneShot(sounds[0]);
			}
		} else { //接地していない時
			animCtrl.SetBool("IsGround", false);
			downSpeed += -0.3f; //落下速度をどんどん速くする
		}
			
		//前方向当たり判定設定
		hit = Physics2D.Raycast (transform.position + new Vector3 (0.34f, 0.26f), Vector2.down, 0.52f); 		
		if (hit.transform != null || IsStop) {
			//障害物に当たった
			animCtrl.SetBool("IsIdle", true);
		} else {
			//障害物に当たっていない
			animCtrl.SetBool("IsIdle", false);
		}

		//上方向チェック
		hit = Physics2D.Raycast (transform.position + new Vector3 (-0.32f, 0.32f), Vector2.right, 0.64f);
		if (hit.transform != null) {
			downSpeed = -0.1f;
			transform.position += new Vector3 (0, -0.1f, 0);
		}

		//動きの管理
		Vector2 nowpos = rb.position;
		nowpos += new Vector2 (forwardSpeed, downSpeed) * Time.deltaTime;
		rb.MovePosition (nowpos);

		animCtrl.SetFloat ("DownSpeed", downSpeed); //Animator内のDownSpeedと接続する
	}


	//Box Collider 2D -> Is Trigger -> on のobjectと接触した時そのobjectを無効化する
	//つまり餌に触れたら消す
	private void OnTriggerEnter2D (Collider2D  collision) {
		collision.gameObject.SetActive (false);
		if (collision.name == "nekokan") {
			audioSource.PlayOneShot (sounds [2]);
			nekokanImage.gameObject.SetActive (true);
		} else {
			score += 1;
			scoreValue.text = score.ToString (); //スコアの表示
			audioSource.PlayOneShot(sounds[1]);
		}
	}

	public void Reset() {
		downSpeed = 0;
		score = 0;
		scoreValue.text = score.ToString ();
		nekokanImage.gameObject.SetActive (false);
		IsStop = true;
		animCtrl.SetBool ("IsIdle", true);
		transform.position = new Vector3 (0, -1.65f, 0);
		animCtrl.SetTrigger ("DemoEnd");
		nekobako.sprite = nekobakoImages [0];

	}

	public void DemoStart(){
		animCtrl.SetTrigger ("DemoStart");
	}

	void PlayJumpSound() {
		audioSource.PlayOneShot (sounds [0]);
	}

	void ChangeNekobako(){
		nekobako.sprite = nekobakoImages [1];
	}
}
