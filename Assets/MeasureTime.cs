using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	float taskTime;
	float flagTime;
	void Start()
	{
		taskTime = 0;
		flagTime = 0;
	}
	void Update()
	{
		if(flagTime == 1)
		{
			taskTime += Time.deltaTime;
		}
	}
	void StartTask()
	{
		taskTime = 0;
		flagTime = 1;
	}
	void FinishTask()
	{
		flagTime = 0;
	}
}
