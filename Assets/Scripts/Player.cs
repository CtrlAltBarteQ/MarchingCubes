using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public CharacterController cc;
    public Camera camera;

    public float sensitivity = 8f;
    private float upDown;

	public float moveSpeed = 9f;

	private float currentJumpHeight = 0f;
	public float jumpHeight = 9f;

	public float gravity = -9.7f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Mouse

        float leftRight = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(0, leftRight, 0);
        
        upDown -= Input.GetAxis("Mouse Y") * sensitivity;
        upDown = Mathf.Clamp(upDown, -89f, 89f);
        camera.transform.localRotation = Quaternion.Euler(upDown, 0, 0);

		#endregion

		#region Keyboard

		float fwdBwd = Input.GetAxis("Vertical") * moveSpeed;
		float moveLeftRight = Input.GetAxis("Horizontal") * moveSpeed;

		if (cc.isGrounded && Input.GetButton("Jump"))
		{
			currentJumpHeight = jumpHeight;
		}
		else if (!cc.isGrounded)
		{
			currentJumpHeight += gravity * Time.deltaTime;
		}

		Vector3 ruch = new Vector3(moveLeftRight, currentJumpHeight, fwdBwd);
		ruch = transform.rotation * ruch;

		cc.Move(ruch * Time.deltaTime);

        #endregion

        #region Edit Terrain

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.point);
            }
        }

        #endregion
    }
}
