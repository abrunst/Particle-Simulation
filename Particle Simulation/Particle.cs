using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Particle_Simulation
{
	/// <summary>
	/// A Particle with a radius and coordinates
	/// </summary>
	class Particle
	{
		/// <summary>
		/// The radius of the ball
		/// </summary>
		private float radius;

		/// <summary>
		/// Property for radius
		/// </summary>
		public float Radius { get { return radius; } set { radius = value; } }

		/// <summary>
		/// The x,y coordinates of the centre of the Particle
		/// </summary>
		private Point coordinates;

		/// <summary>
		/// Property for the x,y coordinates of the centre of the Particle
		/// </summary>
		public Point Coordinates {  get { return coordinates; } set { coordinates = value; } }

		/// <summary>
		/// The vector of the particle
		/// Representing 
		/// </summary>
		private Vector vector;

		public Vector Vector { get { return vector; } set { vector = value; } }

		/// <summary>
		/// The mass of the particle
		/// </summary>
		private float mass;

		/// <summary>
		/// Property for the mass of the particle
		/// </summary>
		public float Mass { get { return mass; } set { mass = value; } }

		/// <summary>
		/// The velocity of the particle
		/// </summary>
		private float velocity;

		/// <summary>
		/// Property for the velocity of the particle
		/// </summary>
		public float Velocity { get { return velocity; } set { velocity = value; } }

		/// <summary>
		/// The acceleration of the particle
		/// </summary>
		private float acceleration;

		/// <summary>
		/// Property for the acceleration of the particle
		/// </summary>
		public float Acceleration { get { return acceleration; } set { acceleration = value; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="radius">Radius of the particle</param>
		/// <param name="x">x coordinate of the centre of the particle</param>
		/// <param name="y">y coordinate of the centre of the particle</param>
		public Particle(float radius, Point coordinates)
		{
			Radius = radius;
			Coordinates = coordinates;
		}

		/// <summary>
		/// Calculates the distance a x,y coordinate is from the edge of the Particle
		/// </summary>
		/// <param name="otherX">The other x coordinate </param>
		/// <param name="otherY">The other y coordinate </param>
		/// <returns>The distance between the Particle and the other coordinates</returns>
		public float DistanceFrom(Point otherCoordinates)
		{
			float distance = (float)Math.Sqrt(((otherCoordinates.X - coordinates.X) * (otherCoordinates.X - coordinates.X)) + ((otherCoordinates.Y - coordinates.Y) * (otherCoordinates.Y - coordinates.Y)));

			return distance;
		}

		/// <summary>
		/// Determines if a x,y coordinate is inside the Particle
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
		/// ToString
		/// </summary>
		/// <returns>Radius of particle, and x,y coordinates</returns>
		public override string ToString()
		{
			return $"Circle " +
				$"\tRadius: {Radius} " +
				$"\tCoordinates: {Coordinates.X},{Coordinates.Y}";
		}
	}
}
