using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float Speed = 30f;
    public float Power = 12f;
    public float Life = 2f;

	void Update () {

        Life -= Time.deltaTime;
        if (Life <= 0)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
	}

    void OnCollisionEnter(Collision collision)
    {
        //Destroy bullets
        Destroy(this.gameObject);

        if (collision.gameObject.tag == "Enemy")
        {
            //Access Enemy script
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            //If ES of enemy script is not Enemy call Hurt function
            if (enemy.ES != EnemyState.Die)
            {
                enemy.Hurt(Power);
            }
        }
    }
}
