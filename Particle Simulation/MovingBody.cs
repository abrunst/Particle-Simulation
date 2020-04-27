using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Particle_Simulation
{
	/// <summary>
	/// A Particle
	/// </summary>
	class MovingBody : Body
	{

		/// <summary>
		/// The force on the particle
		/// </summary>
		private Vector force;

		/// <summary>
		/// Property for the force on the particle
		/// </summary>
		public Vector Force { get { return force; } set { force = value; } }

		/// <summary>
		/// The velocity of the particle
		/// </summary>
		private Vector velocity;

		/// <summary>
		/// Property for the velocity of the particle
		/// </summary>
		public Vector Velocity { get { return velocity; } set { velocity = value; } }

		/// <summary>
		/// The acceleration of the particle
		/// </summary>
		private Vector acceleration;

		/// <summary>
		/// Property for the acceleration of the particle
		/// </summary>
		public Vector Acceleration { get { return acceleration; } set { acceleration = value; } }

		/// <summary>
		/// EllipseGeometry for the particleview
		/// </summary>
		private EllipseGeometry ellipseGeometry = new EllipseGeometry();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="radius">Radius of the particle</param>
		/// <param name="coordinates">Coordinates of the particle</param>
		public MovingBody(Point coordinates, double radius) : base(coordinates, radius)
		{
			Coordinates = coordinates;
			Radius = radius;
			Mass = 62;
		}

		public void ApplyForce(Body body)
		{
			Vector vector = Point.Subtract(body.Coordinates, Coordinates);
			vector.Normalize();
			Acceleration = vector;
			Velocity = Vector.Add(velocity, Acceleration);
			Coordinates = Vector.Add(Velocity, Coordinates);

		}

	}
}
