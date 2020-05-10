using System.Windows;
using System.Windows.Media;

namespace Particle_Simulation
{
	/// <summary>
	/// A Body that can move
	/// </summary>
	class MovingBody : Body
	{
		/// <summary>
		/// The gravitational constant
		/// </summary>
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
			PreviousCoordinates = Coordinates;
			Radius = radius;
			Mass = 100000000000;
		}

		/// <summary>
		/// An implementation of Eulers method to update the position and account for lag
		/// Error per step is propertional to dt squared
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
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
		/// <param name="body">The body applying the force to this MovingBody</param>
		public void CalculateForce(Body body)
		{
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			double distance = unitVector.Length;
			unitVector.Normalize();
			Force = Vector.Multiply((gravitationalConstant * Mass * body.Mass / distance * distance), unitVector);
		}

		/// <summary>
		/// Calculates the intial force for Verlocity Verlet
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		public void CalculateInitialForce(Body body)
		{
			Vector unitVector = Point.Subtract(body.PreviousCoordinates, Coordinates);
			double distance = unitVector.Length;
			unitVector.Normalize();
			Force = Vector.Multiply((gravitationalConstant * Mass * body.Mass / distance * distance), unitVector);
		}

		/// <summary>
		/// A Euler's implementation of updating the initial velocity
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		/// <param name="dt">The change in time</param>
		public void EulerUpdateVelocity(Body body, double dt)
		{
			//Calculating Acceleration
			CalculateInitialForce(body);
			Acceleration = Force / Mass;

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(dt, Acceleration));
		}

		/// <summary>
		/// Updates the MovingBody's coordinates
		/// </summary>
		/// <param name="dt">The change in time</param>
		public void UpdateCoordinates(double dt)
		{
			PreviousCoordinates = Coordinates;
			Coordinates = Vector.Add(Vector.Multiply(Velocity, dt), Coordinates);
		}



		/// <summary>
		/// A Velocity Verlet implementation of updating the initial velocity
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		/// <param name="dt">The change in time</param>
		public void VerletUpdateInitialVelocity(Body body, double dt)
		{
			//Calculating Acceleration
			CalculateInitialForce(body);
			Acceleration = Force / Mass;

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt, Acceleration));
		}

		/// <summary>
		/// Updates the final verlocity for Verlocity Verlet
		/// </summary>
		/// <param name="body">The body applying force</param>
		/// <param name="dt">The change in time</param>
		public void VerletUpdateFinalVelocity(Body body, double dt)
		{
			//Calculating Acceleration
			CalculateForce(body);
			Acceleration = Force / Mass;

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt, Acceleration));
		}

		/// <summary>
		/// Updates the previous coordinates to the current coordinates
		/// </summary>
		public void UpdatePreviousCoordinates()
		{
			PreviousCoordinates = Coordinates;
		}
	}
}
