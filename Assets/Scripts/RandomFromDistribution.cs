using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class RandomFromDistribution  {
	
	public enum ConfidenceLevel_e { _60=0, _80, _90, _95, _98, _99, _998, _999 };
	
	/// <summary>
	/// Get a random number from a normal distribution in [min,max].
	/// </summary>
	/// <description>
	/// Get a random number between min [inclusive] and max [inclusive] with probability matching
	/// a normal distribution along this range. The width of the distribution is described by the
	/// confidence_level_cutoff, which describes what percentage of the bell curve should be over
	/// the provided range. For example, a confidence level cutoff of 0.999 will result in a bell
	/// curve from min to max that contains 99.9% of the area under the complete curve. 0.80 gives
	/// a curve with 80% of the distribution's area.
	/// Because a normal distribution flattens of towards the ends, this means that 0.80 will have
	/// more even distribution between min and max than 0.999.
	/// </description>
	/// <returns>
	/// A random number between min [inclusive] and max [inclusive], with probability described
	/// by the distribution.
	/// </returns>
	/// <param name="min">The min value returned [inclusive].</param>
	/// <param name="max">The max min value returned [inclusive].</param>
	/// <param name="confidence_level_cutoff">
	/// The percentage of a standard normal distribution that should be represented in the range.
	/// </param>
	public static float RandomRangeNormalDistribution(float mean, float min, float max,
	                                                  ConfidenceLevel_e confidence_level_cutoff /*, float confidence_level_cutoff*/) {

		//float mean = 0.5f * (min + max);
		
		// TODO formula for this?
		float z_score_cutoff = confidence_to_z_score[(int)confidence_level_cutoff];

		float new_width = (max - min) / 2.0f;
		float sigma = new_width / z_score_cutoff;


		// Get random normal from Normal Distribution that's within the confidence level cutoff requested
		float random_normal_num;
		do {
			random_normal_num = RandomNormalDistribution(mean, sigma);
			
		} while (random_normal_num > max || random_normal_num < min);

		// now you have a number selected from a bell curve stretching from min to max!
		return random_normal_num;

	}


	/// <summary>
	/// Get a random number from a normal distribution with given mean and standard deviation.
	/// </summary>
	/// <description>
	/// Get a random number with probability following a normal distribution with given mean
	/// and standard deviation. The likelihood of getting any given number corresponds to
	/// its value along the y-axis in the distribution described by the parameters.
	/// </description>
	/// <returns>
	/// A random number between -infinity and infinity, with probability described by the distribution.
	/// </returns>
	/// <param name="mean">The Mean (or center) of the normal distribution.</param>
	/// <param name="std_dev">The Standard Deviation (or Sigma) of the normal distribution.</param>
	public static float RandomNormalDistribution(float mean, float std_dev) {
		
		// Get random normal from Standard Normal Distribution
		float random_normal_num = RandomFromStandardNormalDistribution();
		
		// Stretch distribution to the requested sigma variance
		random_normal_num *= std_dev;
		
		// Shift mean to requested mean:
		random_normal_num += mean;
		
		// now you have a number selected from a normal distribution with requested mean and sigma!
		return random_normal_num;
		
	}

	/// <summary>
	/// Get a random number from the standard normal distribution.
	/// </summary>
	/// <returns>
	/// A random number in range [-inf, inf] from the standard normal distribution (mean == 1, stand deviation == 1).
	/// </returns>
	public static float RandomFromStandardNormalDistribution() {
		
		// This code follows the polar form of the muller transform:
		// https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform#Polar_form
		// also known as Marsaglia polar method 
		// https://en.wikipedia.org/wiki/Marsaglia_polar_method


		// calculate points on a circle
		float u, v;

		float s; // this is the hypotenuse squared.
		do {
			u = Random.Range (-1f, 1f);
			v = Random.Range (-1f, 1f);
			s = (u * u) + (v * v);
		} while (!(s != 0 && s < 1)); // keep going until s is nonzero and less than one

		// TODO allow a user to specify how many random numbers they want!
		// choose between u and v for seed (z0 vs z1)
		float seed;
		if (Random.Range(0,2) == 0) {
			seed = u;
		}
		else {
			seed = v;
		}

		// create normally distributed number.
		float z = seed * Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);

		return z;
	}

	
	private static float[] confidence_to_z_score = {
		0.84162123f,
		1.28155156f,
		1.64485363f,
		1.95996399f,
		2.32634787f,
		2.57582931f,
		3.0902323f,
		3.29052673f
	};
}
