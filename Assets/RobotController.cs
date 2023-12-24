using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class RobotController : MonoBehaviour
{
    // How fast we can turn and move full throttle
    [SerializeField] float turnSpeed;

    [SerializeField] float moveSpeed;

    [SerializeField] XRKnob steeringWheel;
    
    [SerializeField] XRLever steeringLever;

    // How fast we will reach the above speeds
    [SerializeField] float turnAcceleration;

    [SerializeField] float moveAcceleration;

    // World space velocity
    Vector3 currentVelocity;

    // We are only doing a rotation around the up axis, so we only use a float here
    float currentAngularVelocity;

    [SerializeField] LegStepper frontLeftLegStepper;
    [SerializeField] LegStepper frontRightLegStepper;
    [SerializeField] LegStepper backLegStepper;
    [SerializeField] LayerMask layerMask;

// Only allow diagonal leg pairs to step together
    IEnumerator LegUpdateCoroutine()
    {
        // Run continuously
        while (true)
        {
            // Move front legs
            frontLeftLegStepper.TryMove();
            yield return null;
            yield return new WaitUntil(() => !frontLeftLegStepper.Moving);

            frontRightLegStepper.TryMove();
            yield return null;
            yield return new WaitUntil(() => !frontRightLegStepper.Moving);

            // Move back leg
            backLegStepper.TryMove();
            yield return null;
            yield return new WaitUntil(() => !backLegStepper.Moving);
        }
    }


    public float rotationSpeed = 30.0f;

    // Метод, вызываемый при изменении значения руля
    private void Start()
    {
        // Подписываемся на событие изменения значения руля
        steeringWheel.onValueChange.AddListener(RotateObject);
        steeringLever.onLeverActivate.AddListener(StopObject);
        steeringLever.onLeverDeactivate.AddListener(MoveObjectForward);
    }

    public void RotateObject(float knobValue)
    {
        // Задаем угол вращения объекта в зависимости от значения руля
        float targetRotation = Mathf.Lerp(-180.0f, 180.0f, knobValue);


        // Плавно вращаем объект
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, targetRotation, 0.0f),
            Time.deltaTime * rotationSpeed);
    }
    
    public void MoveObjectForward()
    {
        Vector3 forwardDirection = transform.forward;
        
        // Используем плавное изменение скорости с учетом moveAcceleration
        float targetMoveVelocity = moveSpeed * Time.deltaTime;
        float smoothedMoveVelocity = Mathf.Lerp(currentVelocity.magnitude, targetMoveVelocity, 1 - Mathf.Exp(-moveAcceleration * Time.deltaTime));

        // Обновляем текущую скорость
        currentVelocity = forwardDirection * smoothedMoveVelocity;

        // Обновляем позицию объекта
        currentVelocity = forwardDirection * moveSpeed;
    }
    
    public void StopObject()
    {
        // Сбрасываем текущую скорость, чтобы остановить объект
        currentVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        // Важно отписываться от события при уничтожении объекта или компонента
        steeringWheel.onValueChange.RemoveListener(RotateObject);
        steeringLever.onLeverActivate.RemoveListener(StopObject);
        steeringLever.onLeverDeactivate.RemoveListener(MoveObjectForward);
    }

    void LateUpdate()
    {
         RootMotionUpdate();
    }

    void RootMotionUpdate()
    {
        
        // Keep the object at a constant height above the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            // Calculate the desired height above the ground
            float desiredHeight = hit.point.y + 1.4f;

            // Smoothly interpolate between the current height and the desired height
            float smoothHeight = Mathf.Lerp(transform.position.y, desiredHeight, Time.deltaTime * 2.0f);

            // Update the position with the new height
            transform.position = new Vector3(transform.position.x, smoothHeight, transform.position.z);
        }
        transform.position += currentVelocity * Time.deltaTime;
    }

    void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
    }
}