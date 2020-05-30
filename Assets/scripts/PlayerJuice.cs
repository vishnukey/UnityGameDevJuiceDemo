using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJuice : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private float gunStrength = 500f;
    [SerializeField] private float shotTTL = 5f;
    [SerializeField] private PlayerController controller;
    [SerializeField] private float knockBack = .1f;
    [SerializeField] private Properties shakeProperties;
    
    const float maxAngle = 10f;
    IEnumerator currentShakeCoroutine;
    
    float completionPercent = 0;
    float movePercent = 0;
    private bool shakeRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shoot();
        if (currentShakeCoroutine != null && !shakeRunning) currentShakeCoroutine = null;
    }

    private void Shoot()
    {
        var shot = Instantiate(projectile, hand.position, hand.rotation);
        shot.AddForce(hand.forward * gunStrength);
        Destroy(shot.gameObject, shotTTL);
        StartShake(shakeProperties);
        controller.character.Move(controller.transform.forward * -knockBack);
    }

    public void StartShake(Properties properties) {
		if (currentShakeCoroutine != null) {
			//StopCoroutine (currentShakeCoroutine);
			completionPercent = 0;
			movePercent = 0;
		}
		else
		{

			currentShakeCoroutine = Shake(properties, controller.mainCam.transform);
			StartCoroutine(currentShakeCoroutine);
		}
    }

	IEnumerator Shake(Properties properties, Transform transform) {
		completionPercent = 0;
		movePercent = 0;
		shakeRunning = true;

		float angle_radians = properties.angle * Mathf.Deg2Rad - Mathf.PI;
		Vector3 startPosition = transform.localPosition;
		Vector3 previousWaypoint = transform.localPosition;
		Vector3 currentWaypoint = transform.localPosition;
		float moveDistance = 0;
		float speed = 0;

		Quaternion startRotation = transform.localRotation;
		Quaternion targetRotation = transform.localRotation;
		Quaternion previousRotation = transform.localRotation;

		do {
			if (movePercent >= 1 || completionPercent == 0) {
				float dampingFactor = DampingCurve (completionPercent, properties.dampingPercent);
				float noiseAngle = (Random.value - .5f) * Mathf.PI;
				angle_radians += Mathf.PI + noiseAngle * properties.noisePercent;
				currentWaypoint = startPosition + new Vector3 (Mathf.Cos (angle_radians), Mathf.Sin (angle_radians)) * properties.strength * dampingFactor;
				previousWaypoint = transform.localPosition;
				moveDistance = Vector3.Distance (currentWaypoint, previousWaypoint);

				targetRotation = startRotation * Quaternion.Euler (new Vector3 (currentWaypoint.y, currentWaypoint.x).normalized * properties.rotationPercent * dampingFactor * maxAngle);
				previousRotation = transform.localRotation;

				speed = Mathf.Lerp(properties.minSpeed,properties.maxSpeed,dampingFactor);

				movePercent = 0;
			}

			completionPercent += Time.deltaTime / properties.duration;
			movePercent += Time.deltaTime / moveDistance * speed;
			transform.localPosition = Vector3.Lerp (previousWaypoint, currentWaypoint, movePercent);
			transform.localRotation = Quaternion.Slerp (previousRotation, targetRotation, movePercent);
	

			yield return null;
		} while (moveDistance > 0);

		transform.localPosition = startPosition;
		transform.localRotation = startRotation;
		shakeRunning = false;
	}

	float DampingCurve(float x, float dampingPercent) {
		x = Mathf.Clamp01 (x);
		float a = Mathf.Lerp (2, .25f, dampingPercent);
		float b = 1 - Mathf.Pow (x, a);
		return b * b * b;
	}


	[System.Serializable]
	public class Properties {
		public float angle;
		public float strength;
		public float maxSpeed;
		public float minSpeed;
		public float duration;
		[Range(0,1)]
		public float noisePercent;
		[Range(0,1)]
		public float dampingPercent;
		[Range(0,1)]
		public float rotationPercent;

		public Properties (float angle, float strength, float speed, float duration, float noisePercent, float dampingPercent, float rotationPercent)
		{
			this.angle = angle;
			this.strength = strength;
			this.maxSpeed = speed;
			this.duration = duration;
			this.noisePercent = Mathf.Clamp01(noisePercent);
			this.dampingPercent = Mathf.Clamp01(dampingPercent);
			this.rotationPercent = Mathf.Clamp01(rotationPercent);
		}
		

	}
}
