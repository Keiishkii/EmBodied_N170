using UnityEngine;

public class CubeRotation : MonoBehaviour {

	private float yawSpeed = 1f;
	private float pitchSpeed = 1f;
	private float rollSpeed = 1f;

	void Update () {

		//OVRInput.Update();

		// returns a float of the secondary (typically the Right) index finger trigger’s current state.
		// (range of 0.0f to 1.0f)
		//if(OVRInput.Get(OVRInput.Button.Four, OVRInput.Controller.LTouch)) // D
			yawSpeed += 1;
		//if (OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.LTouch)) // A
			yawSpeed -= 1;
		//if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch)) // W
			pitchSpeed += 1;
		//if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)) // S
			pitchSpeed -= 1;
		
		if (Input.GetKey("a"))
			yawSpeed += 1;
		if (Input.GetKey("d") && yawSpeed > 0)
				yawSpeed -= 1;

		if (Input.GetKey("w"))
			pitchSpeed += 1;
		if (Input.GetKey("s") && pitchSpeed > 0)
				pitchSpeed -= 1;


		if (Input.GetKey("e"))
			rollSpeed += 1;
		if (Input.GetKey("q") && rollSpeed > 0)
				rollSpeed -= 1;
		
		transform.rotation *= Quaternion.Euler(yawSpeed * Time.deltaTime, pitchSpeed * Time.deltaTime, rollSpeed * Time.deltaTime);
	}
}
