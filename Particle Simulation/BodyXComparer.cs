using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Simulation
{
	/// <summary>
	/// A class for comparing the x values of the bottom left of the bounds of Bodys
	/// </summary>
	class BodyXComparer : IComparer<Body>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">A Body</param>
		/// <param name="y">A Body</param>
		/// <returns></returns>
		public int Compare(Body x, Body y)
		{
			int result;
			if (x.ellipseGeometry.Bounds.BottomLeft.X > y.ellipseGeometry.Bounds.BottomLeft.X)
			{
				result = 1;
			} else if (x.ellipseGeometry.Bounds.BottomLeft.X < y.ellipseGeometry.Bounds.BottomLeft.X)
			{
				result = -1;
			} else
			{
				result = 0;
			}

			return result;
		}
	}
}
