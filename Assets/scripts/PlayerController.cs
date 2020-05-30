using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public CharacterController character;
    private float xRot;

    public Camera mainCam;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float sensitivity = 100f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var moveDir = (
                Input.GetAxis("Vertical") * transform.forward + 
                Input.GetAxis("Horizontal") * transform.right
            ).normalized;
        
        var mouse = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            ) * (sensitivity * Time.deltaTime);

        xRot -= mouse.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        
        mainCam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.Rotate(Vector3.up, mouse.x);
        character.Move(moveDir * (speed * Time.deltaTime));

        if (Input.GetKey(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
    }
}
