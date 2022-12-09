using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class BenchmarkExample : MonoBehaviour {

	Stopwatch sw;
	[NeverProfileMethod]
	void Start () {
		sw = new Stopwatch();
		sw.Start();
	}
	
	[NeverProfileMethod]
	void Update () {
		// Very simplistic example of a fps benchmark. What you should be doing is taking control of the camera and moving it alone a route of your own - carrying out any actions etc. as needed.	
		if (sw.Elapsed.TotalSeconds > 3 && sw.IsRunning && !Benchy.Benchmark)
		{
			// Kick off the benchmark
			BenchyRuntime.ResetBenchmark();
			BenchyRuntime.ToggleBenchmark();
			sw.Reset();
			sw.Start();
		}

		if (sw.Elapsed.TotalSeconds > 15 && Benchy.Benchmark)
		{
			// Stop the benchmark
			BenchyRuntime.ToggleBenchmark();
			sw.Reset();
			sw.Start();
		}
	}
}
