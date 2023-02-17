using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestTime : MonoBehaviour
{
	float restTime;
	float flagTime;
	float endTime;
	void Start()
	{
		restTime = 0;
		flagTime = 0;
		endTime = 60;
	}
	void Update()
	{
		if(flagTime == 1) restTime += Time.deltaTime;
		if(restTime >= endTime) FinishRest();
	}
	void StartRest()
	{
		restTime = 0;
		flagTime = 1;
	}

	void FinishRest()
	{
		flagTime = 0;
		// scene遷移
	}
}
