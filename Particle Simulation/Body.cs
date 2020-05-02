using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Particle_Simulation
{
	class Body
	{
		/// <summary>
		/// The mass of the particle
		/// </summary>
		private double mass;

		/// <summary>
		/// Property for the mass of the particle
		/// </summary>
		public double Mass { get { return mass; } set { mass = value; } }

		
		public double Radius { get { return ellipseGeometry.RadiusY; } set { ellipseGeometry.RadiusX = value; ellipseGeometry.RadiusY = value; } }

		/// <summary>
		/// Property for the x,y coordinates of the centre of the Particleview
		/// </summary>
		public Point Coordinates { get { return ellipseGeometry.Center; } set { ellipseGeometry.Center = value; } }

		/// <summary>
		/// EllipseGeometry for the particleview
		/// </summary>
		private EllipseGeometry ellipseGeometry = new EllipseGeometry();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="radius">Radius of the particle</param>
		/// <param name="coordinates">Coordinates of the particle</param>
		public Body(Point coordinates, double radius)
		{
			Coordinates = coordinates;
			Radius = radius;
			Mass = 62;
		}

		/// <summary>
		/// Calculates the distance a x,y coordinate is from the edge of the MovingBody
		/// </summary>
		/// <param name="otherX">The other x coordinate </param>
		/// <param name="otherY">The other y coordinate </param>
		/// <returns>The distance between the Particle and the other coordinates</returns>
		public float DistanceFrom(Point otherCoordinates)
		{
			float distance = (float)Math.Sqrt(((otherCoordinates.X - Coordinates.X) * (otherCoordinates.X - Coordinates.X)) + ((otherCoordinates.Y - Coordinates.Y) * (otherCoordinates.Y - Coordinates.Y)));

			return distance;
		}

		/// <summary>
		/// Determines if a x,y coordinate is inside the MovingBody
		/// </summary>
		/// <param name="otherX">The other x coordinate</param>
		/// <param name="otherY">The other y coordinate</param>
		/// <returns>true if the given coordinate is inside the Particle, otherwise false</returns>
		public bool IsInside(Point otherCoordinates)
		{
			bool inside = false;

			if (DistanceFrom(otherCoordinates) <= Radius)
			{
				inside = true;
			}
			return inside;
		}

		/// <summary>
		/// Adds the GeometryDrawing to the DrawingGroup
		/// </summary>
		/// <param name="drawingGroup">The group of geometry drawings</param>
		public void AddToDrawing(GeometryGroup geometryGroup)
		{
			geometryGroup.Children.Add(ellipseGeometry);
		}
	}
	

}
