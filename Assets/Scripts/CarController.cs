using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
};
public class CarController : MonoBehaviour
{
    private Rigidbody playerRB;
    public WheelColliders colliders;
    public WheelMeshes meshes;
    public WheelParticles wheelParticles;
    public LightingManager lightingManager;
    public Timer_Manager timeMannager;
    private float gasInput;
    public float brakeInput;
    public float steeringInput;
    public float motorPower;
    public float brakePower;
    public float slipAngle;
    public float speed;
    private float speedClamped;
    public AnimationCurve steeringcurve;
    public GameObject smokePrefab;
    public List<Light> backLights;
    public bool isBreaking;
    public bool lightOner;
    public float maxSpeed;
    public int IsEngineRunning;
    public float RPM;
    public float redLine;
    public float idleRPM;
    public TMP_Text rpmText;
    public TMP_Text geartext;
    public RectTransform rpmNeedle;
    public float minNeedleRotation;
    public float maxNeedleRotation;
    public int currentGear;
    public float[] gearRatios;
    public float differentialRatio;
    private float currentTorque;
    private float clutch;
    private float wheelrpm;
    public AnimationCurve horsePowerToRPMcurve;
    private GearState gearState;
    public float increaseGearRPM;
    public float decreaseGearRPM;
    public float changeGeartime = 0.1f;
    float intensity;
    public GameObject tireMark;
    [SerializeField] float originalSpeed;
    [SerializeField] AudioSource DriftSFX;
    public ButtonInputs gasPedal;
    public ButtonInputs brakePedal;
    public SteeringWheel SteerWheels;

    #region WheelFriction Curves 
    WheelFrictionCurve BFcurve;
    WheelFrictionCurve BScurve;
    WheelFrictionCurve FFcurve;
    WheelFrictionCurve FScurve;
    #endregion


    void Start()
    {
        playerRB =gameObject.GetComponent<Rigidbody>();
        InstantiateParticles();
        BFcurve = colliders.BLwheel.forwardFriction;
        BScurve = colliders.BLwheel.sidewaysFriction;
        FFcurve = colliders.FLwheel.forwardFriction;
        FScurve = colliders.FLwheel.sidewaysFriction;
    }

    void InstantiateParticles()
    {
        if(smokePrefab)
        {
            wheelParticles.FRWheel = Instantiate(smokePrefab, colliders.FRwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.FRwheel.transform)
            .GetComponent<ParticleSystem>();
            wheelParticles.FLWheel = Instantiate(smokePrefab, colliders.FLwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.FLwheel.transform)
            .GetComponent<ParticleSystem>();
            wheelParticles.BRWheel = Instantiate(smokePrefab, colliders.BRwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.BRwheel.transform)
            .GetComponent<ParticleSystem>();
            wheelParticles.BLWheel = Instantiate(smokePrefab, colliders.BLwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.BLwheel.transform)
            .GetComponent<ParticleSystem>();
        }
        if(tireMark)
        {
            wheelParticles.BRWheelMark = Instantiate(tireMark, colliders.BRwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.BRwheel.transform)
           .GetComponent<TrailRenderer>();
            wheelParticles.BLWheelMark = Instantiate(tireMark, colliders.BLwheel.transform.position - Vector3.up * colliders.FRwheel.radius, Quaternion.identity, colliders.BLwheel.transform)
            .GetComponent<TrailRenderer>();
        }
    }
    void Update()
    {
        originalSpeed = playerRB.velocity.magnitude * 3.6f;
        RpmNdSpeed();
        CheckInput();
        ApplyMotor();
        ApplyBrake();
        ApplySteering();
        CheckParticles();
        ApplyWheelPositions();
        BackLighting();
        HeadLightsOner();
        FrictionHandler();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Time_Adder adder))
        {
            if (!adder.TimeAdded)
            {
                timeMannager.AddingTime(adder);
            }

        }
    }

    void RpmNdSpeed()
    {
        rpmNeedle.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, originalSpeed / maxSpeed));
        rpmText.text = Mathf.RoundToInt(RPM) + " rpm";
        speed = colliders.BRwheel.rpm * colliders.BLwheel.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
    }

    private void HeadLightsOner()
    {
        lightOner = Input.GetKeyDown(KeyCode.L);
        if (lightOner)
        {
            lightingManager.ToggleHeadLights();
        }
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        #region Mobile Input
        // if (gasPedal.IsPressed)
        // {
        //     gasInput += gasPedal.dampeningPress;
        // }
        // if(brakePedal.IsPressed)
        // {
        //     gasInput -= brakePedal.dampeningPress;
        // }


        // steeringInput = SteerWheels.Output; // For Steering Control
        // steeringInput = Input.GetAxis("Horizontal");  // For A-D control
        #endregion
        slipAngle = Vector3.Angle(transform.forward,playerRB.velocity - transform.forward);
        if(gearState!=GearState.Changing)
        {
            if(gearState == GearState.Neutral)
            {
                clutch = 0;
                if(gasInput >= 0)
                {
                    gearState = GearState.Running;
                }
            }
            else
            {
                clutch = Input.GetKey(KeyCode.LeftShift) ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
            }
           
        }
        else
        {
            clutch = 0;
        } 

        if ( Mathf.Abs(gasInput) > 0 && IsEngineRunning == 0)
        {
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
            gearState = GearState.Running;
        }
        if (slipAngle < 120f)
        {
            if (gasInput <= 0f)
            {
                brakeInput = Mathf.Abs(gasInput);
                if(brakeInput> 0)
                {
                    geartext.text = "R";
                    isBreaking = true;
                } 
                else isBreaking = false;
            }
            else
            {
                geartext.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();

            }
        }
        else
        {
            brakeInput = 0f;
        }

    }
    void ApplyMotor()
    {

         currentTorque = CalculateTorque();
         colliders.BRwheel.motorTorque = currentTorque * gasInput;
         colliders.BLwheel.motorTorque = currentTorque * gasInput;
    }
    float CalculateTorque()
    {
        float torque = 0f;
        if(RPM<idleRPM&&gasInput==0&&currentGear==0)
        {
            gearState = GearState.Neutral;
        }
        if(gearState == GearState.Running&&clutch>0)
        {
            if (RPM > increaseGearRPM)
            {
                StartCoroutine(ChangeGear(1));
            }
            else if(RPM < decreaseGearRPM)
            {
                StartCoroutine(ChangeGear(-1));
            }
        }
        if(IsEngineRunning > 0)
        {
            if(clutch < 0.1f)
            {
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLine * gasInput) + Random.Range(-50, 50),Time.deltaTime);
            }
            else
            {
                wheelrpm = Mathf.Abs((colliders.BRwheel.rpm + colliders.BLwheel.rpm) / 2)*gearRatios[currentGear]*differentialRatio;
                RPM = Mathf.Lerp ( RPM ,Mathf.Max ( idleRPM - 100 , wheelrpm ),Time.deltaTime * 3 );
                torque = (horsePowerToRPMcurve.Evaluate(RPM / redLine) * motorPower/RPM)*gearRatios[currentGear]*differentialRatio*5252*clutch;
            }
        }
        return torque;
    }
    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringcurve.Evaluate(speed);
        if(slipAngle<90f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, playerRB.velocity + transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -85f, 85f);
        colliders.FRwheel.steerAngle = steeringAngle;
        colliders.FLwheel.steerAngle = steeringAngle;
    }
    void ApplyBrake()
    {
        colliders.FRwheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FLwheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.BRwheel.brakeTorque = brakeInput * brakePower * 0.4f;
        colliders.BLwheel.brakeTorque = brakeInput * brakePower * 0.4f;
    }
    void ApplyWheelPositions()
    {
        UpdateWheels(colliders.FRwheel,meshes.FRWheel);
        UpdateWheels(colliders.FLwheel,meshes.FLWheel);
        UpdateWheels(colliders.BRwheel,meshes.BRWheel);
        UpdateWheels(colliders.BLwheel,meshes.BLWheel);
    }
    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        colliders.FRwheel.GetGroundHit(out wheelHits[0]);
        colliders.FLwheel.GetGroundHit(out wheelHits[1]);
        colliders.BRwheel.GetGroundHit(out wheelHits[2]);
        colliders.BLwheel.GetGroundHit(out wheelHits[3]);
        float slipAllowance = 1.2f;
        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip)) > slipAllowance)
        {
            wheelParticles.BRWheel.Play();
            if (wheelParticles.BRWheelMark != null)
                wheelParticles.BRWheelMark.emitting = true;
            if (!DriftSFX.isPlaying) DriftSFX.Play();
        }
        else
        {
            wheelParticles.BRWheel.Stop();
            wheelParticles.BRWheelMark.emitting = false;
            DriftSFX.Stop();
        }

        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip)) > slipAllowance)
        {
            wheelParticles.BLWheel.Play();
            if(wheelParticles.BLWheelMark != null)
                wheelParticles.BLWheelMark.emitting = true;
            if (!DriftSFX.isPlaying) DriftSFX.Play();
        }
        else
        {
            wheelParticles.BLWheel.Stop();
            wheelParticles.BLWheelMark.emitting = false;
            DriftSFX.Stop();
        }
    }
    void UpdateWheels(WheelCollider collider,MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        collider.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
    private void BackLighting()
    {
        if (gameObject == GameObject.FindGameObjectWithTag("Lilly"))
        {
            intensity = 70;
        }
        else intensity = 4f;
        foreach (Light light in backLights)
        {
            light.intensity = isBreaking ? intensity : 0f;
        }
    }

    public float GetSpeedratio()
    {
        var gas = Mathf.Clamp(Mathf.Abs(gasInput), 0.5f, 0.9f);
        return RPM * gas / redLine;
    }
    private void FrictionHandler()  // Changes friction of the wheel colliders to ensure smooth reverse 
    {
        if(Vehical_Camera_follow.isCamFacingBack)
        {
            WheelFrictionCurve bfcurve = colliders.BLwheel.forwardFriction;
            WheelFrictionCurve bscurve = colliders.BLwheel.sidewaysFriction;
            WheelFrictionCurve ffcurve = colliders.FRwheel.forwardFriction;
            WheelFrictionCurve fscurve = colliders.FRwheel.sidewaysFriction;

            bfcurve.asymptoteSlip = 0.8f;
            bfcurve.asymptoteValue = 0.5f;
            bfcurve.stiffness = 1f;

            bscurve.asymptoteValue = 0.3f;

            ffcurve.asymptoteSlip = 0.8f;
            ffcurve.stiffness = 1f;
            ffcurve.asymptoteValue = 0.5f;

            fscurve.asymptoteSlip = 0.5f;
            ffcurve.asymptoteValue = 0.75f;


            // forward friction 
            colliders.BLwheel.forwardFriction = bfcurve;
            colliders.BRwheel.forwardFriction = bfcurve;
            colliders.FRwheel.forwardFriction = ffcurve;
            colliders.FLwheel.forwardFriction = ffcurve;

            // sideways friction 
            colliders.BLwheel.sidewaysFriction = bscurve;
            colliders.BRwheel.sidewaysFriction = bscurve;
            colliders.FRwheel.sidewaysFriction = fscurve;
            colliders.FLwheel.sidewaysFriction = fscurve;




        }
        else
        {
            // forward friction 
            colliders.BLwheel.forwardFriction = BFcurve;
            colliders.BRwheel.forwardFriction = BFcurve;
            colliders.FRwheel.forwardFriction = FFcurve;
            colliders.FLwheel.forwardFriction = FFcurve;

            // sideways friction 
            colliders.BLwheel.sidewaysFriction = BScurve;
            colliders.BRwheel.sidewaysFriction = BScurve;
            colliders.FRwheel.sidewaysFriction= FScurve;
            colliders.FLwheel.sidewaysFriction = FScurve;


        }
    }
    IEnumerator ChangeGear(int gearChange)
    {
        gearState = GearState.CheckingChange;
        if(currentGear + gearChange > 0)
        {
            if(gearChange > 0)
            {
                //decrease the gear

                yield return new WaitForSeconds(3f);
                if(RPM<increaseGearRPM|| currentGear>=gearRatios.Length-1)
                {
                    gearState= GearState.Running;
                    yield break;
                }
            }
            if ( gearChange < 0)
            {
                // increase the gear
                 yield return new WaitForSeconds(0.1f);
                if( RPM/2 > decreaseGearRPM *1.5||currentGear <= 0||gasInput<1)
                {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGeartime);
            currentGear += gearChange;
        }
        if(gearState != GearState.Neutral)
        {
            gearState = GearState.Running;
        }
       
    }  
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRwheel;
    public WheelCollider FLwheel;
    public WheelCollider BRwheel;
    public WheelCollider BLwheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer BRWheel;
    public MeshRenderer BLWheel;
}
[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem BRWheel;
    public ParticleSystem BLWheel;
    
   // public TrailRenderer FRWheelMark;
   // public TrailRenderer FLWheelMark;
    public TrailRenderer BRWheelMark;
    public TrailRenderer BLWheelMark;
    
}