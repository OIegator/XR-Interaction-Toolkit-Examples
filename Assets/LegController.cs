using System;
using System.Collections;
using UnityEngine;


public class LegController : MonoBehaviour
{ 
	public float SamplingDistance = 0.1f;
	public float LearningRate = 0.5f;
	public float DistanceThreshold = 0.15f;
	public RobotJoint[] joints;
	public float[] angles;
	public GameObject target;
	
	public delegate void DeviationCallback(float distance);
	public event DeviationCallback OnDeviation;
	
	private void Start()
	{
		LearningRate *= gameObject.transform.parent.localScale.x;
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Vector3 direction = joints[i + 1].transform.localPosition; // Вектор от одного узла к другому
			Vector3 newAxis = Vector3.Cross(direction.normalized, joints[i].transform.InverseTransformDirection(joints[i].axis)); // Перпендикуляр к плоскости образованной направлением и осью из инспектора
			joints[i].axis = newAxis;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Gizmos.DrawLine(joints[i].transform.position, joints[i].transform.position + joints[i].transform.InverseTransformDirection(joints[i].axis));
		}
	}
	private void Update()
	{
		Vector3 targetPosition = target.transform.position;

		for (int i = 0; i < 4; i++)
		{
			InverseKinematics(targetPosition, angles);	
		}
		
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(angles[i], joints[i].axis); // Задаем вращение по этому перпендикуляру 
			joints[i].transform.localRotation = joints[i].StartRotation; 
			joints[i].transform.localRotation *= rotation; 
		}

		CheckDeviation(angles, 0.3f);
	}
	
	public Vector3 ForwardKinematics(float[] angles)
	{
		Vector3 prevPoint = joints[0].transform.position;
		Quaternion rotation = joints[0].transform.parent.rotation;
		for (int i = 1; i < joints.Length; i++)
		{
			// Выполняет поворот вокруг новой оси
			rotation *= joints[i - 1].GetComponent<RobotJoint>().StartRotation;
			rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
			Vector3 nextPoint = prevPoint + rotation * joints[i].StartOffset * gameObject.transform.parent.localScale.x;

			prevPoint = nextPoint;
		}
		return prevPoint;
	}

	public float DistanceFromTarget(Vector3 target, float[] angles)
	{
		Vector3 point = ForwardKinematics(angles);
		return Vector3.Distance(point, target);
	}


	public float PartialGradient(Vector3 target, float[] angles, int i)
	{
		// Сохраняет угол,
		// который будет восстановлен позже
		float angle = angles[i];

		// Градиент: [F(x+SamplingDistance) - F(x)] / h
		float f_x = DistanceFromTarget(target, angles);

		angles[i] += SamplingDistance;
		float f_x_plus_d = DistanceFromTarget(target, angles);

		float gradient = (f_x_plus_d - f_x) / SamplingDistance;

		// Восстановление
		angles[i] = angle;

		return gradient;
	}

	public void InverseKinematics(Vector3 target, float[] angles)
	{
		if (DistanceFromTarget(target, angles) < DistanceThreshold)
			return;

		for (int i = joints.Length - 1; i >= 0; i--)
		{
			// Градиентный спуск
			// Обновление : Solution -= LearningRate * Gradient
			float gradient = PartialGradient(target, angles, i);
			angles[i] -= LearningRate * gradient * DistanceFromTarget(target, angles);

			// Ограничение
			angles[i] = Mathf.Clamp(angles[i], joints[i].MinAngle, joints[i].MaxAngle);

			// Преждевременное завершение
			if (DistanceFromTarget(target, angles) < DistanceThreshold)
				return;
		}
	}

	private void CheckDeviation(float[] angles, float threshold)
	{
		float distance = DistanceFromTarget(target.transform.position, angles);

		if (distance > threshold)
		{
			//Debug.Log($"Deviation from target: {distance} - {target.name}");
            
			// Вызываем событие, передавая значение отклонения
			OnDeviation?.Invoke(distance);
		}
	}


}