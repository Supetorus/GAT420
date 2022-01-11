using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	public static class Utilities
	{
		public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max)
		{
			Vector3 result = v;

			if (result.x > max.x) result.x = min.x + (result.x - max.x);
			if (result.x < min.x) result.x = max.x + (min.x - result.x);
			if (result.y > max.y) result.y = min.y + (result.y - max.y);
			if (result.y < min.y) result.y = max.y + (min.y - result.y);
			if (result.z > max.z) result.z = min.z + (result.z - max.z);
			if (result.z < min.z) result.z = max.z + (min.z - result.z);

			return result;
		}
	}
}
