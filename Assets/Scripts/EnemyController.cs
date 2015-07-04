using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float health = 100.0f;

	public void damage(float d)
	{
		health -= d;

		if (health <= 0) 
			Die ();
	}

	void Die()
	{
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
                                                                                                                                                                                                                                          