using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour {

	private float m_horizontalInput;
	private float m_verticalInput;
	private float m_steeringAngle;
	private bool m_brake;
	private float brakeTorque;
	private double currentSpeed;
	private UnityEngine.Rigidbody body;

	public WheelCollider frontDriverW, frontPassengerW;
	public WheelCollider rearDriverW, rearPassengerW;
	public Transform frontDriverT, frontPassengerT;
	public Transform rearDriverT, rearPassengerT;
	public GameObject carCounter;
	public GameObject brakeText;

	public int maxSpeed = 90;
	public float maxSteerAngle = 30;
	public float motorForce = 50;

	private void Start(){
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0,-0.25f,0);
	}

	private void Update()
	{
		GetInput();
		Steer();
		Accelerate();
		UpdateWheelPoses();
	}


	private void FixedUpdate()
	{
		
	}

	public void GetInput()
	{
		
		m_horizontalInput = Input.GetAxis("Horizontal");
		body = GetComponent<Rigidbody>();

		currentSpeed = body.velocity.magnitude * 3.6;
		
		if(currentSpeed <= maxSpeed){
			m_verticalInput = Input.GetAxis("Vertical");
		}else{
			m_verticalInput = 0;
		}

		//Affiche la vitesse sur l'UI
		carCounter.GetComponent<UnityEngine.UI.Text>().text = Mathf.Round((float)currentSpeed).ToString();
		
		if(Input.GetButton("Jump")){
			m_brake = true;
			brakeText.GetComponent<UnityEngine.UI.Text>().text = "FAM";
		}else{
			m_brake = false;
		}
	}

	private void Steer()
	{
		m_steeringAngle = maxSteerAngle * m_horizontalInput;
		frontDriverW.steerAngle = m_steeringAngle;
		frontPassengerW.steerAngle = m_steeringAngle;
	}

	private void Accelerate()
	{	
		// if (m_brake){
		// 	rearDriverW.brakeTorque = 999999;
		// 	rearPassengerW.brakeTorque = 999999;
		// }else{
		// 	rearDriverW.brakeTorque = 0;
		// 	rearPassengerW.brakeTorque = 0;
		// }

		// Debug.Log(m_brake);

		if(m_verticalInput < 0 && body.velocity.x < 0){
			brakeTorque = 1000;
			brakeText.GetComponent<UnityEngine.UI.Text>().text = "Je freine";
		}else{
			brakeText.GetComponent<UnityEngine.UI.Text>().text = "Arret ou MA";
			brakeTorque = 0;
		}
		
		Debug.Log(frontDriverW.brakeTorque);
		Debug.Log(frontDriverW.motorTorque * motorForce);

		frontDriverW.brakeTorque = brakeTorque;
		frontPassengerW.brakeTorque = brakeTorque;
		frontDriverW.motorTorque = m_verticalInput * motorForce;
		frontPassengerW.motorTorque = m_verticalInput * motorForce;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontDriverW, frontDriverT);
		UpdateWheelPose(frontPassengerW, frontPassengerT);
		UpdateWheelPose(rearDriverW, rearDriverT);
		UpdateWheelPose(rearPassengerW, rearPassengerT);
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_quat = _quat * Quaternion.Euler(new Vector3(90, 0, -90));

		_transform.position = _pos;
		_transform.rotation = _quat;
	}
}
