using UnityEngine;
using System.Collections;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die
}

public class Enemy : MonoBehaviour {

    public EnemyState ES;
    public Animator anim;
    float Speed;

    public float MoveSpeed;
    public float AttackSpeed;

    public float FindRange = 10f;
    public float Damage = 20f;
    public Transform Player;

    public Transform FX_Point;
    public GameObject Hit_FX;
    public AudioClip Hit_Sound;
    public AudioClip Death_Sound;

    public GameObject GUI_Pivot;
    public UISlider LifeBar;
    public float Max_hp = 100f;
    public float hp = 100f;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
        anim = this.GetComponent<Animator>();
    }

    void DistanceCheck()
    {
        if (Vector3.Distance(Player.position, transform.position) >= FindRange)
        {
            ES = EnemyState.Idle;
            anim.SetBool("Run", false);
            Speed = 0;
        }
        else
        {
            ES = EnemyState.Move;
            anim.SetBool("Run", true);
            Speed = MoveSpeed;
        }
    }

    void MoveUpdate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(Player.position.x, this.transform.position.y,Player.position.z) - transform.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    void Update()
    {
        if (ES == EnemyState.Idle)
        {
            DistanceCheck();
        }
        else if(ES == EnemyState.Move)
        {
            MoveUpdate();
            AttackRangeCheck();
        }
    }

    void AttackRangeCheck()
    {
        if (Vector3.Distance(Player.position, transform.position) < 1.5f && ES != EnemyState.Attack)
        {
            Speed = 0;
            ES = EnemyState.Attack;
            anim.SetTrigger("Attack");
        }
    }

    public void Attack_On()
    {
        Player.GetComponent<Player_Ctrl>().Hurt(Damage);
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            ES = EnemyState.Hurt;
            Speed = 0;
            anim.SetTrigger("Hurt");

            //Effect when the enemy is hit
            GameObject FX = Instantiate(Hit_FX, FX_Point.position, Quaternion.LookRotation(FX_Point.forward)) as GameObject;

            //Dedcut hp and change UI
            hp -= damage;
            LifeBar.sliderValue = hp / Max_hp;

            //Sound played when hit
            GetComponent<AudioSource>().clip = Hit_Sound;
            GetComponent<AudioSource>().Play();

            //When hp below 0, call Death function
            if (hp <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        ES = EnemyState.Die;
        anim.SetTrigger("Die");
        Speed = 0;

        //Turn of the GUI and collider
        GUI_Pivot.gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GetComponent<AudioSource>().clip = Death_Sound;
        GetComponent<AudioSource>().Play();

        //Find PlayManager and bring as PM to call EnemyDie Function.
        PlayManager PM = GameObject.Find("PlayManager").GetComponent<PlayManager>();
        PM.EnemyDie();
    }
}
