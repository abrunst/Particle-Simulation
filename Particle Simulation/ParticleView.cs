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
	/// The winform viewing data for the Particle
	/// Meant for use with Particle
	/// </summary>
	class ParticleView
	{
		/// <summary>
		/// The Particle
		/// </summary>
		private Particle particle;

		/// <summary>
		/// Property for particle
		/// </summary>
		public Particle Particle { get { return particle; } set { particle = value; } }

		/// <summary>
		/// Colour of the particle
		/// </summary>
		private Color color;

		/// <summary>
		/// Property for the colour of the particle
		/// </summary>
		public Color Color { get { return color;  } set { color = value; } }

		/// <summary>
		/// Path for the particleview
		/// </summary>
		private Path path = new Path();

		/// <summary>
		/// Property for the colour of the particle
		/// </summary>
		public Path Path { get { return path; } set { path = value; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="radius">Radius of the particle</param>
		/// <param name="coordinates">Coordinates of the particle</param>
		public ParticleView(float radius, Point coordinates, Color color)
		{
			Particle = new Particle(radius, coordinates);
			Color = color;
		}

		/**
		/// <summary>
		/// Draws the particle on the Panel
		/// </summary>
		/// <param name="drawingArea">The Panel to draw on</param>
		public void Draw(Panel drawingArea)
		{
			EllipseGeometry ellipseGeometry = new EllipseGeometry(particle.Coordinates, particle.Radius, particle.Radius);

			path.Fill = new SolidColorBrush(color);
			path.Data = ellipseGeometry;

			drawingArea.Children.Add(path);
		}
		**/

		/// <summary>
		/// Adds the ellipseGeometry to a GeometryGroup
		/// </summary>
		/// <param name="drawingArea">The Panel to draw on</param>
		public void Draw(GeometryGroup geometryGroup)
		{
			EllipseGeometry ellipseGeometry = new EllipseGeometry(particle.Coordinates, particle.Radius, particle.Radius);

			///Creating the geometrydrawing
			GeometryDrawing geometryDrawing = new GeometryDrawing();
			geometryDrawing.Geometry = ellipseGeometry;
			geometryDrawing.Brush = new SolidColorBrush(color);
			geometryDrawing.Pen = new Pen(Brushes.Black, 4);

			

			geometryGroup.Children.Add(geometryDrawing(;


		}

	}
}
