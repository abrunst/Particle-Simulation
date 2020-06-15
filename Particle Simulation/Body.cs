using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Particle_Simulation
{
	/// <summary>
	/// An implementation of a Body
	/// </summary>
	public class Body
	{
		/// <summary>
		/// The mass of the particle
		/// </summary>
		private double mass;

		/// <summary>
		/// A boolean representing whether or not this Body is a moving body
		/// </summary>
		private bool moving;


		public double BoundingCircleRadius { get { return Radius * 2; } }

		/// <summary>
		/// Property for moving
		/// </summary>
		public bool Moving { get { return moving; } }

		/// <summary>
		/// Property for the mass of the particle
		/// </summary>
		public double Mass { get { return mass; } set { mass = value; } }

		/// <summary>
		/// Property for the radius of the Body
		/// </summary>
		public double Radius { get { return ellipseGeometry.RadiusY; } set { ellipseGeometry.RadiusX = value; ellipseGeometry.RadiusY = value; } }

		/// <summary>
		/// Property for the x,y coordinates of the centre of the Particleview
		/// </summary>
		public Point Coordinates { get { return ellipseGeometry.Center; } set { ellipseGeometry.Center = value; } }

		/// <summary>
		/// The previous coordinates of the movingBody
		/// </summary>
		private Point previousCoordinates;

		/// <summary>
		/// Property for the previous coordinates of the movingBody
		/// </summary>
		public Point PreviousCoordinates { get => previousCoordinates; set => previousCoordinates = value; }

		/// <summary>
		/// EllipseGeometry for the Body
		/// </summary>
		public readonly EllipseGeometry ellipseGeometry = new EllipseGeometry();

		/// <summary>
		/// The gravitational constant
		/// </summary>
		private const double gravitationalConstant = 6.674e-11;

		/// <summary>
		/// The velocity of the particle
		/// </summary>
		private Vector velocity;

		/// <summary>
		/// Property for the velocity of the particle
		/// </summary>
		public Vector Velocity { get { return velocity; } set { velocity = value; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="radius">Radius of the particle</param>
		/// <param name="coordinates">Coordinates of the particle</param>
		public Body(Point coordinates, double radius, bool moving)
		{
			Coordinates = coordinates;
			PreviousCoordinates = Coordinates;
			Radius = radius;
			Velocity = new Vector();
			
		
			Mass = 100000;
			this.moving = moving;
			if (moving)
			{
				Mass = 10000000000;
			}
			else
			{
				Mass = 100000000000;
			}
		}


		/// <summary>
		/// Calculates the distance of this Body form another Body otherBody, taking into account radius
		/// </summary>
		/// <param name="otherBody">The other Body to calculate the distance from</param>
		/// <returns>The distance between this Body and otherBody</returns>
		public double DistanceFrom(Body otherBody)
		{
			double distance = Point.Subtract(Coordinates, otherBody.Coordinates).Length - Radius - otherBody.Radius;

			return distance;
		}


		/// <summary>
		/// The distyance of this Body from otherCoordinates
		/// </summary>
		/// <param name="otherCoordinates">A Point</param>
		/// <returns>The distance of this Body from otherCoordinates</returns>
		public double DistanceFrom(Point otherCoordinates)
		{
			double distance = Point.Subtract(Coordinates, otherCoordinates).Length;

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

		/// <summary>
		/// Updates the MovingBody's coordinates with the specificed newCoordinates
		/// </summary>
		/// <param name="newCoordinates">the new coordinates of the MovingBody</param>
		public void UpdateCoordinates(Point newCoordinates)
		{
			PreviousCoordinates = Coordinates;
			Coordinates = newCoordinates;
		}

		/// <summary>
		/// An implementation of Eulers method to update the position and account for lag
		/// Error per step is ]]\]]propertional to dt squared
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		/// <param name="dt">The change in time</param>
		public void Euler(Body body, double dt)
		{
			//Normalizing a Vector of the distance between the movingBody and the Body
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			unitVector.Normalize();

			//Calculating the acceleration with the unitVector
			Vector acceleration = Vector.Multiply(1000, unitVector);

			//Update the Coordinates, integrategrating the Velocity
			Coordinates = Vector.Add(Vector.Multiply(Velocity, dt), Coordinates);

			//Update the Velocity, integrating the Acceleration
			Velocity = Vector.Add(Velocity, Vector.Multiply(acceleration, dt));
		}


		/// <summary>
		/// Calculates the force due to gravity on the movingBody
		/// </summary>
		/// <param name="body">The body applying the force to this MovingBody</param>
		/// <returns>The force being applied to this Body from body</returns>
		public Vector CalculateForce(Body body)
		{
			Vector unitVector = Point.Subtract(body.Coordinates, Coordinates);
			double distance = unitVector.Length;
			unitVector.Normalize();
			return Vector.Multiply((gravitationalConstant * Mass * body.Mass / distance * distance), unitVector);
		}

		/// <summary>
		/// Calculates the intial force for Verlocity Verlet
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		/// <returns>The force being applied to this Body from body</returns>
		public Vector CalculateInitialForce(Body body)
		{
			Vector unitVector = Point.Subtract(body.PreviousCoordinates, Coordinates);
			double distance = unitVector.Length;
			unitVector.Normalize();
			return Vector.Multiply((gravitationalConstant * Mass * body.Mass / distance * distance), unitVector);
		}

		/// <summary>
		/// A Euler's implementation of updating the initial velocity
		/// </summary>
		/// <param name="body">The body applying force to this MovingBody</param>
		/// <param name="dt">The change in time</param>
		public void EulerUpdateVelocity(Body body, double dt)
		{
			//Calculating Acceleration
			Vector acceleration = Vector.Divide(CalculateForce(body), Mass);

			//Updating Velocity, integrating acceleration to get the increase in velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(dt, acceleration));
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
			Vector acceleration = Vector.Divide(CalculateInitialForce(body), Mass);

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt, acceleration));
		}

		/// <summary>
		/// Updates the final verlocity for Verlocity Verlet
		/// </summary>
		/// <param name="body">The body applying force</param>
		/// <param name="dt">The change in time</param>
		public void VerletUpdateFinalVelocity(Body body, double dt)
		{
			//Calculating Acceleration
			Vector acceleration = Vector.Divide(CalculateForce(body), Mass);

			//Updating Velocity
			Velocity = Vector.Add(Velocity, Vector.Multiply(0.5 * dt, acceleration));
		}

		public override String ToString()
		{
			return "Body: " +
				"\n\tCoordinates: (" + Math.Round(Coordinates.X, 2) + ", " + Math.Round(Coordinates.Y, 2) + ") " +
				"\n\tMoving: " + Moving + " " +
				"\n\tVelocity: " + Velocity;
		}
	}
}
