using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    public float jumpSpeed = 15;
    public Collider2D bottomCollider;
    public CompositeCollider2D terrainCollider;
    

    float vx = 0;
    float prevVx = 0;
    bool isGrounded;
    Vector2 originPosition;
    float lastShoot;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position; //게임이 시작됬을 때 위치를 오리진으로
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;

        //eulerAngles 몇도로 돌아갔는지 알려주는 벡터
        transform.eulerAngles = Vector3.zero;
        transform.position = originPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //떨어질때 받은 가속도 상관없이 다시 리스폰되면 멈춘상태로 돌아오게
	}

    // Update is called once per frame
    void Update()
    {
        vx = Input.GetAxisRaw("Horizontal");

        //vx가 마이너스면 왼쪽을 보고있는 것
        if (vx < 0 )
        {
            //flipX라는 변수가 참이면 좌우반전
            GetComponent<SpriteRenderer>().flipX = true;  
		}
        //if문 2개로 쓴이유는 vx가 0일 때 이전 방향을 유지하기 위해서
        if (vx > 0 )
        {
            GetComponent<SpriteRenderer>().flipX = false;  
		}

        //지금 땅에 붙어있다
        if(bottomCollider.IsTouching(terrainCollider))
        {   
            //붙어있지 않았다가 지금은 붙어있음. 착지
            if (!isGrounded)
            {
                if (vx == 0)
                {
                    GetComponent<Animator>().SetTrigger("Idle");        
				}
                else
                {
                    GetComponent<Animator>().SetTrigger("Run");      
				}
			}   
            //바닥에있었고 지금도 바닥, 그러면 속도변화로 모션변화 줘야함, 계속 걷는중
            else
            {   
                //이전속도와 지금 속도가 다르다면
                if (prevVx != vx)
                {
                    if (vx == 0) 
                    {
                        GetComponent<Animator>().SetTrigger("Idle");           
					}
                    else 
                    {
                        GetComponent<Animator>().SetTrigger("Run");               
					}
                }
			}
            isGrounded = true;
		}
        //현재 땅에 붙어있지 않다
        else
        {   
            //좀 전까진 땅에 붙어있었다
            if (isGrounded)
            {   //점프 시작
                GetComponent<Animator>().SetTrigger("Jump");     
			}
            isGrounded = false;
		}


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;  
		}

        prevVx = vx;

        //총알 발사
        if (Input.GetButtonDown("Fire1") && lastShoot + 0.5f < Time.time)
        {
            Vector2 bulletV = Vector2.zero;

            if (GetComponent<SpriteRenderer>().flipX)
            {
                bulletV = new Vector2(-10,0);         
			}
            else
            {
                bulletV = new Vector2(10,0);         
			}

            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().velocity = bulletV;
            lastShoot = Time.time;

		}
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * vx * speed * Time.fixedDeltaTime);

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
		}
	}

    void Die()
    {   //회전, 각속도, 순간적인 힘으로 위쪽으로 힘, 콜라이더를 꺼서 캐릭터가 부딪치지않고 떨어지게.
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().angularVelocity = 720;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10),ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        //다시 시작하는 함수에도 똑같이 넣어서 다시시작할땐 원래모습으로 돌아오게.

        GameManager.Instance.Die();
	}


}
