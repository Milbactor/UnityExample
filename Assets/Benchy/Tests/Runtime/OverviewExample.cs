using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class OverviewExample : MonoBehaviour {

	Stopwatch sw;
	[NeverProfileMethod]
	void Start () {
		sw = new Stopwatch();
		sw.Start();
	}
	
	[NeverProfileMethod]
	void Update () {
		if (sw.Elapsed.TotalSeconds > 2 && sw.IsRunning && !Benchy.Benchmark)
		{
			// Kick off the runtime stats update
			BenchyRuntime.ToggleRuntime() ;
		}

		if (sw.Elapsed.TotalSeconds > 35 && Benchy.Benchmark)
		{
			// Stop the runtime stats update
			BenchyRuntime.ToggleRuntime();
			sw.Reset();
			sw.Start();
		}
	}
}
