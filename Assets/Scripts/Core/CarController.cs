using System;
using Fusion;
using Mods;
using Multiplayer;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class CarController : NetworkBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private AnimationCurve steeringCurve;
    [SerializeField] private Camera camera;
    [SerializeField]
    private Rigidbody playersRigidbody;

    public event Action OnDrifting;
    
    private float speed;
    private float verticalInput;
    private float horizontalInput;
    private float brakeInput;
    private bool canMove;
    [Networked] private NetworkInputData data { get; set; }

    private void Start()
    {
       
    }

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasInputAuthority)
        {
           // camera.gameObject.SetActive(false);
        }
           BasicSpawner.Instance.OnRoundStarted += EnableMovement;
    }

    private async void EnableMovement()
    {
        await Task.Delay(3500);
        canMove = true;
    }

    private void SetRigidBody()
    {
       // playersRigidbody = car.CurrentCarInstance.GetComponent<Rigidbody>();
        playersRigidbody.centerOfMass =  new Vector3(0, -0.5f, 0);
    }

    private void Update()
    {
        if (!canMove) return;
        if (car.CurrentCarInstance == null) return;
        if (playersRigidbody == null)
        {
            SetRigidBody();
        }
        foreach (Wheel wheel in car.CurrentCarInstance.Wheels)
        {
            wheel.UpdateMeshPosition();
        }
        if(!car.CurrentCarInstance || !playersRigidbody) return;
         
        Move();
        Brake();
        WheelSteering();
        CheckInput();   
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        if (GetInput(out NetworkInputData tempdata))
        {
            data = tempdata;
            horizontalInput = data.Horizontal;
            verticalInput = data.Vertical;
        }
    }

    private void FixedUpdate()
    {
        
        
        
    }

    private void Move()
    {
        speed = playersRigidbody.velocity.magnitude;
       
        foreach (Wheel wheel in car.CurrentCarInstance.Wheels)
        { 
            wheel.WheelCollider.motorTorque = motorForce * verticalInput;
            wheel.UpdateMeshPosition();
        }
    }
    
    private void CheckInput()
    {
        float movingDirection = Vector3.Dot(car.CurrentCarInstance.transform.forward, playersRigidbody.velocity);
        brakeInput = (movingDirection < -0.5f && verticalInput > 0) || (movingDirection > 0.5f && verticalInput < 0) ? Math.Abs(verticalInput) : 0;
    }

    private void WheelSteering()
    {
        if (playersRigidbody.velocity.magnitude <= .1)
        {
            return;
        }
        
        float driftAngle = Vector3.Angle(car.CurrentCarInstance.transform.forward, playersRigidbody.velocity);
        float steeringAngle = horizontalInput * steeringCurve.Evaluate(speed);
        float slipAngle = Vector3.Angle(car.CurrentCarInstance.transform.forward, playersRigidbody.velocity - car.CurrentCarInstance.transform.forward);
        if (slipAngle < 120)
            steeringAngle += Vector3.SignedAngle(car.CurrentCarInstance.transform.forward, playersRigidbody.velocity, Vector3.up);
        if (driftAngle > 40 && driftAngle < 120)
            OnDrifting?.Invoke();
        steeringAngle = Math.Clamp(steeringAngle, -45, 45);
      
        foreach (Wheel wheel in car.CurrentCarInstance.Wheels)
        {
            if (wheel.isForwardWheels)
                wheel.WheelCollider.steerAngle = steeringAngle;
        }
    }
    private void Brake()
    {
        foreach (Wheel wheel in car.CurrentCarInstance.Wheels)
        {
            wheel.WheelCollider.brakeTorque = brakeInput * (wheel.isForwardWheels ? 0.7f : 0.3f);
        }
    }
}


[System.Serializable]
public struct Wheel
{
    public Transform WheelMesh;
    public WheelCollider WheelCollider;
    public bool isForwardWheels;

    public void UpdateMeshPosition()
    {
        Vector3 position;
        Quaternion rotation;
        
        WheelCollider.GetWorldPose(out position, out rotation);
        WheelMesh.rotation = rotation;
    }
}
