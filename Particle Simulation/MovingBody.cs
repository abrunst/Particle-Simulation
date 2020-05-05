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

		private const double gravitationalConstant = 6.674e-11;

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


		/// <summary>
		/// An implementation of Eulers method to update the position and account for lag
		/// Error per step is propertional to dt squared
		/// </summary>
		/// <param name="body"></param>
		/// <param name="dt"></param>
		public void Euler(Body body, double dt)
		{
			//Normalizing a Vector of the distance between the movingBody and the Body
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			unitVector.Normalize();

			//Calculating the acceleration with the unitVector
			Acceleration = Vector.Multiply(1000, unitVector);

			//Update the Coordinates, integrategrating the Velocity
			Coordinates = Vector.Add(Vector.Multiply(Velocity, dt), Coordinates);

			//Update the Velocity, integrating the Acceleration
			Velocity = Vector.Add(Velocity, Vector.Multiply(Acceleration, dt));
		}

		/// <summary>
		/// Calculates the force due to gravity on the movingBody
		/// </summary>
		/// <param name="body"></param>
		public void CalculateForce(Body body)
		{
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			double distance = unitVector.Length;
			unitVector.Normalize();
			Force = Vector.Multiply((gravitationalConstant * Mass * body.Mass / distance * distance), unitVector);
		}

		/// <summary>
		/// Implementation of Velocity Verlet method
		/// Accurate for a constant acceleration
		/// </summary>
		/// <param name="body"></param>
		/// <param name="dt"></param>
		public void VelocityVerlet(Body body, double dt)
		{
			//Normalizing a Vector of the distance between the movingBody and the Body
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			unitVector.Normalize();

			//Calculating the acceleration with the unitVector
			Acceleration = Vector.Multiply(500, unitVector);

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt * dt, Acceleration));

			//Updating Coordinates
			Coordinates = Vector.Add(Vector.Multiply(Velocity, dt), Coordinates);

			//Updating Acceleration here if acceleration is not constant

			//Updating Velocity
			//Normalizing a Vector of the distance between the movingBody and the Body
			unitVector = Point.Subtract(body.Coordinates, Coordinates);
			unitVector.Normalize();

			//Calculating the acceleration with the unitVector
			Acceleration = Vector.Multiply(500, unitVector);

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt, Acceleration));
		}
	}
}
