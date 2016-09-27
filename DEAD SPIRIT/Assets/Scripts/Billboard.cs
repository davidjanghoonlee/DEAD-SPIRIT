using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Transform Target;

	void Start () {

        Target = Camera.main.gameObject.transform;
	}
	
	void Update () {

        transform.rotation = Quaternion.LookRotation(Target.forward, Target.up);
	}
}
