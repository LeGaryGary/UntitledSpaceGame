using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    public new Camera camera;
    public float thrust;
    public float maxSpeed;
    public float buildCameraSpeed;

    private Rigidbody2D rb;
    private Inventory inventory;

    private Vector2 shipLastPos;

    public ShipMode Mode = ShipMode.Flight;

    private Vector2 thrustOrDirectionVector;
    private bool braking;

    private float zoomTarget;
    private float zoomMax = 30;
    private float zoomMin = 1f;
    private float zoomSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();

        zoomTarget = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mode == ShipMode.Flight)
        {
            ManageFlightControls();
            ManageRotation();
            FlightCamera();
        }
        else
        {
            // HighlightBlockPlacment();
            BuildCamera();
        }

        shipLastPos = rb.position;
    }

    private void FlightCamera()
    {
        camera.transform.position = new Vector3(rb.position.x, rb.position.y, camera.transform.position.z);
        CameraZoom();
    }

    private void BuildCamera()
    {
        var cameraDirection = Vector2.ClampMagnitude(thrustOrDirectionVector, 1) * buildCameraSpeed;
        var oldPos = camera.transform.position;
        var shipPosChange = rb.position - shipLastPos;
        camera.transform.position = new Vector3(oldPos.x + cameraDirection.x + shipPosChange.x, oldPos.y + cameraDirection.y + shipPosChange.y, oldPos.z);
        CameraZoom();
    }

    private void CameraZoom()
    {
        zoomTarget -= Mouse.current.scroll.ReadValue().y / 100;
        if (zoomTarget > zoomMax) zoomTarget = zoomMax;
        if (zoomTarget < zoomMin) zoomTarget = zoomMin;

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomTarget, Time.deltaTime * zoomSpeed);
    }

    private void ManageRotation()
    {
        var mouse = Mouse.current.position.ReadValue();
        var mouseWorld3 = camera.ScreenToWorldPoint(mouse);

        var mouseWorld = new Vector2(mouseWorld3.x, mouseWorld3.y);
        var angle = Vector2.SignedAngle(rb.transform.up, mouseWorld - rb.position);
        var transformAngles = rb.transform.eulerAngles;
        rb.transform.eulerAngles = new Vector3(transformAngles.x, transformAngles.y, Mathf.Lerp(transformAngles.z, transformAngles.z + angle, 5 * Time.deltaTime));
    }

    private void ManageFlightControls()
    {
        rb.AddForce(Vector2.ClampMagnitude(thrustOrDirectionVector.Rotate(rb.transform.eulerAngles.z), thrust), ForceMode2D.Impulse);
        if (braking)
        {
            var brakeforce = Vector2.ClampMagnitude(rb.velocity, 1) * thrust * -1;
            rb.AddForce(brakeforce, ForceMode2D.Impulse);
        }
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public void Up(InputAction.CallbackContext context){
        var force = Vector2.up * thrust;
        if (context.started) thrustOrDirectionVector += force;
        if (context.canceled) thrustOrDirectionVector -= force;
    }

    public void Down(InputAction.CallbackContext context)
    {
        var force = Vector2.down * thrust;
        if (context.started) thrustOrDirectionVector += force;
        if (context.canceled) thrustOrDirectionVector -= force;
    }

    public void Left(InputAction.CallbackContext context)
    {
        var force = Vector2.left * thrust;
        if (context.started) thrustOrDirectionVector += force;
        if (context.canceled) thrustOrDirectionVector -= force;
    }

    public void Right(InputAction.CallbackContext context)
    {
        var force = Vector2.right * thrust;
        if (context.started) thrustOrDirectionVector += force;
        if (context.canceled) thrustOrDirectionVector -= force;
    }

    public void Brake(InputAction.CallbackContext context)
    {
        if (context.started) braking = true;
        if (context.canceled) braking = false;
    }

    public void ToggleBuildOrFlight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Mode == ShipMode.Flight)
            {
                Mode = ShipMode.Build;
                inventory.EnableHotbar();
            }
            else { Mode = ShipMode.Flight; inventory.DisableHotbar(); }
            
        }
    }
    public enum ShipMode
    {
        Build,
        Flight
    }
}

