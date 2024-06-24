using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Bytesized
{
	public class SmoothMouse : MonoBehaviour 
	{
		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
		public RotationAxes axes = RotationAxes.MouseXAndY;
		public float sensitivityX = 15F;
		public float sensitivityY = 15F;
	
		public float minimumX = -360F;
		public float maximumX = 360F;
	
		public float minimumY = -60F;
		public float maximumY = 60F;
	
		float rotationX = 0F;
		float rotationY = 0F;
	
		private List<float> rotArrayX = new List<float>();
		float rotAverageX = 0F;	

		private List<float> rotArrayY = new List<float>();
		float rotAverageY = 0F;
	
		public float frameCounter = 20;

		public bool invertY = true;
	
		Quaternion originalRotation;
		private bool _lockCursor = true;
		public Texture2D crosshairImage;
	
		void OnGUI()
		{
			if(!_lockCursor) return;
			const float size = 0.25f;
			float xMin = (Screen.width / 2) - (crosshairImage.width * size / 2);
			float yMin = (Screen.height / 2) - (crosshairImage.height * size / 2);
			GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width * size, crosshairImage.height * size), crosshairImage);
		}

		void Update ()
		{
			if(_lockCursor)
				HandleCamera();
			HandleCursor();
		}

		private void HandleCamera()
		{
			if (axes == RotationAxes.MouseXAndY)
			{			
				rotAverageY = 0f;
				rotAverageX = 0f;
	
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
	
				rotArrayY.Add(rotationY);
				rotArrayX.Add(rotationX);
	
				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt(0);
				}
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt(0);
				}
	
				for(int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY[j];
				}
				for(int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX[i];
				}
	
				rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;
	
				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
	
				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
	
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			}
			else if (axes == RotationAxes.MouseX)
			{			
				rotAverageX = 0f;
	
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
	
				rotArrayX.Add(rotationX);
	
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt(0);
				}
				for(int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX[i];
				}
				rotAverageX /= rotArrayX.Count;
	
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
	
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;			
			}
			else
			{			
				rotAverageY = 0f;
	
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY * (invertY ? 1 : -1);
	
				rotArrayY.Add(rotationY);
	
				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt(0);
				}
				for(int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY[j];
				}
				rotAverageY /= rotArrayY.Count;
	
				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
	
				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}

		private void HandleCursor()
		{
			Cursor.visible = !_lockCursor;
			Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
			if (Input.GetKeyDown(KeyCode.Escape))
				_lockCursor = !_lockCursor;
		}

		private void HandleMovement()
		{
			var speed = 4f;
			if(Input.GetKey(KeyCode.W))
				transform.position += transform.forward * Time.deltaTime * speed;
			if(Input.GetKey(KeyCode.A))
				transform.position -= transform.right * Time.deltaTime * speed;
			if(Input.GetKey(KeyCode.S))
				transform.position -= transform.forward * Time.deltaTime * speed;
			if(Input.GetKey(KeyCode.D))
				transform.position += transform.right * Time.deltaTime * speed;
		}
	
		void Start ()
		{		
			Rigidbody rb = GetComponent<Rigidbody>();	
			if (rb)
				rb.freezeRotation = true;
			originalRotation = transform.localRotation;
		}
	
		public static float ClampAngle (float angle, float min, float max)
		{
			angle = angle % 360;
			if ((angle >= -360F) && (angle <= 360F)) {
				if (angle < -360F) {
					angle += 360F;
				}
				if (angle > 360F) {
					angle -= 360F;
				}			
			}
			return Mathf.Clamp (angle, min, max);
		}
	}
}