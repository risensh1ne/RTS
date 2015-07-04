using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float moveSpeed = 3.0f;
	float attackDistance = 1.0f;
	bool isMoving = false;
	bool isAttacking = false;
	public float health = 100.0f;

	private Vector3 destinationPos;
	Transform targetEnemy;

	Animator anim;
	// Use this for initialization
	void Start () {
		destinationPos = transform.position;

		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetMouseButtonDown (1)) {

			Vector3 eventPos = ScreenToWorld (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
			eventPos.y = transform.position.y;

			RaycastHit hitInfo = new RaycastHit ();
			bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);
			if (hit && hitInfo.transform.gameObject.tag == "enemy") {
				targetEnemy = hitInfo.transform;

				if (Vector3.Distance (transform.position, targetEnemy.position) < attackDistance) {
					isMoving = false;
					isAttacking = true;
				} else {
					isMoving = true;
					isAttacking = true;
					destinationPos = targetEnemy.position;
				}
			} else {
				targetEnemy = null;
				float _distance = Vector3.Distance (transform.position, eventPos);
				if (_distance > 0.1) {
					isMoving = true;

					if (isAttacking) 
						isAttacking = false;

					destinationPos = eventPos;
				}
			}
		} else if (Input.GetKeyDown (KeyCode.A)) {
			if (targetEnemy != null) {
				anim.SetTrigger("special01");
			}
		} else if (Input.GetKeyDown (KeyCode.S)) {
			if (targetEnemy != null) {
				anim.SetTrigger("special02");
			}
			return;
		} 

		if (isMoving || isAttacking) {
			float distance = Vector3.Distance (transform.position, destinationPos);

			if (targetEnemy != null) {
				if (Vector3.Distance (transform.position, targetEnemy.position) < attackDistance) {
					isMoving = false;
					isAttacking = true;
				} else {
					Vector3 dir = (destinationPos - transform.position).normalized;
					
					transform.position += dir * Time.fixedDeltaTime * moveSpeed;
					transform.rotation = Quaternion.LookRotation (dir);
				}
			} else {
				if (distance > 0.1) {
					Vector3 dir = (destinationPos - transform.position).normalized;

					transform.position += dir * Time.fixedDeltaTime * moveSpeed;
					transform.rotation = Quaternion.LookRotation (dir);
				} else {
					isMoving = false;
					isAttacking = false;
				}
			}
		} 

		updatePlayerState ();
	}

	void updatePlayerState()
	{
		if (isMoving) {
			anim.SetBool ("isMoving", true);
			anim.SetBool ("isAttacking", false);
		} else if (isAttacking) {
			anim.SetBool ("isMoving", false);
			anim.SetBool ("isAttacking", true);
		} else {
			anim.SetBool ("isMoving", false);
			anim.SetBool ("isAttacking", false);
		}
	}
		
	void ScrewKick()
	{
		float kickDistance = 2.0f;

		Vector3 dir = (destinationPos - transform.position).normalized;

		Vector3 destPos = transform.position + dir * kickDistance;
		//Vector3.Lerp (transform.position, destPos, 0.5f);
		transform.position += dir * kickDistance;

	}

	void DamageEnemy()
	{

		targetEnemy.gameObject.GetComponent<EnemyController> ().damage (20.0f);
	}

	
	Vector3 ScreenToWorld( Vector2 screenPos )
	{
		// Create a ray going into the scene starting 
		// from the screen position provided 
		Ray ray = Camera.main.ScreenPointToRay( screenPos );
		RaycastHit hit;
		
		// ray hit an object, return intersection point
		if( Physics.Raycast( ray, out hit ) )
			return hit.point;
		
		// ray didn't hit any solid object, so return the 
		// intersection point between the ray and 
		// the Y=0 plane (horizontal plane)
		float t = -ray.origin.y / ray.direction.y;
		return ray.GetPoint( t );
	}
}
