using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
}

public class Player_Ctrl : MonoBehaviour {

    public PlayerState PS;

    public Vector3 lookDirection;
    public float Speed = 0f;
    public float WalkSpeed = 6f;
    public float RunSpeed = 12f;

    Animation animation;
    public AnimationClip Idle_Ani;
    public AnimationClip Walk_Ani;
    public AnimationClip Run_Ani;

    public GameObject Bullet;
    public Transform ShotPoint;
    public GameObject ShotFX;
    public AudioClip ShotSound;

    public UISlider LifeBar;
    public float Max_hp = 100;
    public float hp = 100;

    void KeyboardInput()
    {
        float xx = CrossPlatformInputManager.GetAxisRaw("Vertical");
        float ZZ = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        if (PS != PlayerState.Attack)
        {
                lookDirection = xx * Vector3.forward + ZZ * Vector3.right;
                Speed = WalkSpeed;
                PS = PlayerState.Walk;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Speed = RunSpeed;
                    PS = PlayerState.Run;
                }
           

            if (xx == 0 && ZZ == 0 && PS != PlayerState.Idle)
            {
                PS = PlayerState.Idle;
                Speed = 0f;
            }
        }

        if (CrossPlatformInputManager.GetButton("Fire1") && PS != PlayerState.Dead)
        {
            StartCoroutine("Shot");
        }
    }


    void LookUpdate()
    {
        Quaternion R = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, R, 7f);

        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    void Update()
    {
        if (PS != PlayerState.Dead)
        {
            KeyboardInput();
            LookUpdate();
        }

        AnimationUpdate();
    }

    void Start()
    {
        animation = this.GetComponent<Animation>();
    }

    void AnimationUpdate()
    {
        if (PS == PlayerState.Idle)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Walk)
        {
            animation.CrossFade(Walk_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Run)
        {
            animation.CrossFade(Run_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Attack)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
        else if (PS == PlayerState.Dead)
        {
            animation.CrossFade(Idle_Ani.name, 0.2f);
        }
    }

    public IEnumerator Shot()
    {
        GameObject bullet = Instantiate(Bullet,
                                        ShotPoint.position,
                                        Quaternion.LookRotation(ShotPoint.forward)) as GameObject;

        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

        GetComponent<AudioSource>().clip = ShotSound;
        GetComponent<AudioSource>().Play();

        ShotFX.SetActive(true);

        PS = PlayerState.Attack;
        Speed = 0f;

        yield return new WaitForSeconds(0.15f);
        ShotFX.SetActive(false);

        yield return new WaitForSeconds(0.15f);
        PS = PlayerState.Idle;
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            LifeBar.sliderValue = hp / Max_hp;
        }

        if (hp <= 0)
        {
            Speed = 0;
            PS = PlayerState.Dead;

            //Find PlayerManager and bring as PM to call GameOver function
            PlayManager PM = GameObject.Find("PlayManager").GetComponent<PlayManager>();
            PM.GameOver();
        }
    }
}
