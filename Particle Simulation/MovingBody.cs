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
		/// Applies the force from the Body on the movingBody
		/// </summary>
		/// <param name="body">The Body that is applying the force</param>
		public void forceFromBody(Body body, double timeUntilRender)
		{
			//Vector vector = Point.Subtract(body.Coordinates, Coordinates);
			//vector.Normalize();

			//Multiplying the normalized acceleration by the acceleration due to gravity on the earths surface
			//Acceleration = Vector.Multiply(vector, 9.81);

			//Integrating Acceleration
			Acceleration = Vector.Multiply(Acceleration, timeUntilRender);
			//Acceleration = vector;

			//Adding the Acceleration to the Velocity
			Velocity = Vector.Add(Velocity, Acceleration);

			//Integrating Velocity
			Velocity = Vector.Multiply(Velocity, timeUntilRender);
		}

		public void applyVelocity(double timeUntilRender)
		{

			Console.WriteLine(Coordinates);
			Coordinates = Vector.Add(Velocity, Coordinates);
			
		}

		/// <summary>
		/// An implementation of Eulers method to update the position and account for lag
		/// </summary>
		/// <param name="body"></param>
		/// <param name="dt"></param>
		public void eulerPosition(Body body, double dt)
		{
			//Update the Coordinates, integrategrating the Velocity
			Coordinates = Vector.Add(Vector.Multiply(Velocity, dt), Coordinates);

			//Normalizing a Vector of the distance between the movingBody and the Body
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			unitVector.Normalize();

			//Calculating the acceleration with the unitVector
			Acceleration = Vector.Multiply(1000, unitVector);

			//Update the Velocity, integrating the Acceleration
			Velocity = Vector.Add(Velocity, Vector.Multiply(Acceleration, dt));
		}

	}
}
