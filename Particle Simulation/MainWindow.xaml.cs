using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Particle_Simulation
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// The GeometryGroup for the Bodies
		/// </summary>
		private GeometryGroup bodyGroup = new GeometryGroup();

		private Broadphase broadphase = new Broadphase();

		private Narrowphase narrowphase = new Narrowphase();

		/// <summary>
		/// The GeometryGroup for the movingBodies
		/// </summary>
		private GeometryGroup movingBodyGroup = new GeometryGroup();

		/// <summary>
		/// The path for the Bodies
		/// </summary>
		private Path bodyPath = new Path();

		/// <summary>
		/// The Path for the movingBodies
		/// </summary>
		private Path movingBodyPath = new Path();

		/// <summary>
		/// List of all the Bodies and movingBodies
		/// </summary>
		private List<Body> bodies = new List<Body>();

		/// <summary>
		/// The radius of the earth
		/// </summary>
		private double earthRadius = 6.38e+6;

		/// <summary>
		/// The mass of the earth
		/// </summary>
		private const double earthMass = 5.972e+24;

		/// <summary>
		/// The gravitational constant
		/// </summary>
		private const double gravitationalConstant = 6.674e-11;

		/// <summary>
		/// The Body that is being dragged, else null
		/// </summary>
		private Body dragging;

		/// <summary>
		/// The time of the last render
		/// </summary>
		private TimeSpan lastRender;

		/// <summary>
		/// The time until the next render
		/// </summary>
		private double dt = 0;

		/// <summary>
		/// The currently selected ball
		/// </summary>
		private Body selectedBody;

		/// <summary>
		/// Constructor
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			SetDefaults();
		}

		/// <summary>
		/// Sets the default values for WPF elements
		/// </summary>
		public void SetDefaults()
		{
			bodyGroup.FillRule = FillRule.Nonzero;
			movingBodyGroup.FillRule = FillRule.Nonzero;

			drawingArea.Children.Add(bodyPath);
			drawingArea.Children.Add(movingBodyPath);

			bodyColorPicker.SelectedColor = Color.FromArgb(200, 105, 105, 105);
			movingBodyColorPicker.SelectedColor = Color.FromArgb(100, 192, 192, 192);


			bodyPath.Data = bodyGroup;
			bodyPath.Fill = new SolidColorBrush(bodyColorPicker.SelectedColor.Value);
			bodyPath.Stroke = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
			bodyPath.StrokeThickness = 4;
			bodyPath.Fill.Freeze();
			bodyPath.Stroke.Freeze();

			movingBodyPath.Data = movingBodyGroup;
			movingBodyPath.Fill = new SolidColorBrush(movingBodyColorPicker.SelectedColor.Value);
			movingBodyPath.Stroke = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
			movingBodyPath.StrokeThickness = 4;
			movingBodyPath.Fill.Freeze();
			movingBodyPath.Stroke.Freeze();

			//Setting the size of the window to 80% of the screen
			this.Height = (System.Windows.SystemParameters.FullPrimaryScreenHeight * 0.80);
			this.Width = (System.Windows.SystemParameters.FullPrimaryScreenWidth * 0.80);

			///Setting lastRender
			lastRender = TimeSpan.FromTicks(DateTime.Now.Ticks);


			CompositionTarget.Rendering += CompositionTarget_Rendering;
		}

		/// <summary>
		/// Handler for the rendering event
		/// Updates the positions of the Bodies
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			//Casting to RenderingEventArgs to get the rendering time
			RenderingEventArgs renderArgs = (RenderingEventArgs)e;

			//Gets the time until next render
			dt = (renderArgs.RenderingTime - lastRender).TotalSeconds;
			lastRender = renderArgs.RenderingTime;

			

			

			//Determine which numerical method to use to integrate motion
			if (movementMethodComboBox.SelectedValue.ToString() == "Velocity Verlet")
			{
				VelocityVerlet();
			}

			
			else if (movementMethodComboBox.SelectedValue.ToString() == "Euler")
			{
				Euler();
			}

			//If a Body is being dragged, update its coordinates to the current mouse coordinates
			if (dragging != null)
			{
				dragging.UpdateCoordinates(Mouse.GetPosition(drawingArea));
			}

			if (collisionCheckbox.IsChecked.Value)
			{

				List<List<Body>> bodiesTocheck = broadphase.SweepBodies(bodies);

				narrowphase.FindColliding(bodiesTocheck);

			}
			UpdateSelected();
		}


		/// <summary>
		/// On MouseLeftButtonDown in the drawingArea, decides what to do based on the selection in onClickComboBox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">The mouse event that triggered the handler</param>
		private void DrawingArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (onClickComboBox.SelectedValue.ToString() == "Click")
			{
				foreach (Body body in bodies)
				{
					if (body.IsInside(e.GetPosition(drawingArea)))
					{
						selectedBody = body;
					}
				}
			}
			else if (onClickComboBox.SelectedValue.ToString() == "Create Static Body")
			{
				double radius = (double)staticBodyRadiusIUD.Value;
				Point coordinates = e.GetPosition(drawingArea);

				AddBody(radius, coordinates);
			}
			else if (onClickComboBox.SelectedValue.ToString() == "Create Moving Body" )
			{
				float radius = (float)movingBodyRadiusIUD.Value;
				Point coordinates = e.GetPosition(drawingArea);

				AddMovingBody(radius, coordinates);
			}
			else if (onClickComboBox.SelectedValue.ToString() == "Drag Body")
			{
				foreach (Body body in bodies)
				{
					if (dragging is null && body.IsInside(e.GetPosition(drawingArea)))
					{
						dragging = body;
						if (dragging is Body)
						{
							dragging.Velocity.Normalize();
							dragging.Velocity = Vector.Multiply(dragging.Velocity, 0);
						}
						
					}
				}
			}
			
		}

		/// <summary>
		/// Determines if a click is inside a Body
		/// </summary>
		/// <param name="x">The x coordinate of the click</param>
		/// <param name="y">The y coordinate of the click</param>
		public void ClickedInside(Point coordinates)
		{
			foreach (Body body in bodies)
			{
				if (body.IsInside(coordinates))
				{

				}
			}
			
		}

		/// <summary>
		/// Adds a Body to the geometryGroup
		/// </summary>
		/// <param name="radius">The radius of the particle</param>
		/// <param name="coordinates">The cooridnates of the particle</param>
		public void AddBody(double radius, Point coordinates)
		{
			Body body = new Body(coordinates, radius, false);

			body.AddToDrawing(bodyGroup);
			broadphase.AddBodies(bodies, new List<Body> { body });
		}

		/// <summary>
		/// Adds a MovingBody to the GeometryGroup
		/// </summary>
		/// <param name="radius">The radius of the particle</param>
		/// <param name="coordinates">The cooridnates of the particle</param>
		public void AddMovingBody(float radius, Point coordinates)
		{
			Body body = new Body(coordinates, radius, true);

			body.AddToDrawing(movingBodyGroup);
			broadphase.AddBodies(bodies, new List<Body> { body });
		}

		/// <summary>
		/// Handler for the left mouse button being released
		/// Sets draggin to null if it is not already
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DrawingArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (dragging != null)
			{
				dragging = null;
			}
		}

		/// <summary>
		/// Handler for a click on the clearButton
		/// On click, clears the Bodies in bodyGroup and the movingBodies in movingBodyGroup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			bodyGroup.Children.Clear();
			movingBodyGroup.Children.Clear();
			bodies = new List<Body>();
		}

		/// <summary>
		/// Event handler for the selectedColor changed in bodyColorPicker
		/// Changes the bodyPath fill to the new colour
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BodyColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			bodyPath.Fill = new SolidColorBrush(e.NewValue.Value);
		}

		/// <summary>
		/// Event handler for the selectedColor changed in movingBodyColorPicker
		/// Changes the movingBodyPath fill to the new colour
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MovingBodyColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			movingBodyPath.Fill = new SolidColorBrush(e.NewValue.Value);
		}

		/// <summary>
		/// An implementation of the Verlocity Verlet method for integrating motion
		/// </summary>
		private void VelocityVerlet()
		{
			//For every movingbody that isn't being dragged, 
			//go through all the bodies in allbodies,
			//and update the movingBodys velocity and coordinates if the body is not the movingBody
			foreach (Body body in bodies)
			{
				if (body != dragging && body.Moving)
				{
					foreach(Body otherBody in bodies)
					{
						if (otherBody != body)
						{
							body.VerletUpdateInitialVelocity(otherBody, dt);
						}
					}
					
					body.UpdateCoordinates(dt);
				}
			}

			//For every movingbody that isn't being dragged, 
			//go through all the bodies in allbodies,
			//and update the movingBodys velocity if the body is not the movingBody
			foreach (Body body in bodies)
			{
				if (body != dragging && body.Moving)
				{
					foreach(Body otherBody in bodies)
					{
						if (otherBody != body)
						{
							body.VerletUpdateFinalVelocity(otherBody, dt);
						}
						
					}
					
				}
			}
		}

		/// <summary>
		/// An implementaion of Euler's method for integrating motion
		/// </summary>
		public void Euler()
		{
			//For every movingbody that isn't being dragged, 
			//go through all the bodies in allbodies,
			//and update the movingBodys velocity and coordinates if the body is not the movingBody
			foreach (Body body in bodies)
			{
				if (body != dragging && body.Moving)
				{
					body.EulerUpdateVelocity(body, dt);
					body.UpdateCoordinates(dt);

				}
			}
		}

		/// <summary>
		/// Calculates the new normal velocity of a body after collision with another body
		/// </summary>
		/// <param name="normalVelocity1">The normal velocity to update</param>
		/// <param name="body1">The body with the velocity to update</param>
		/// <param name="normalVelocity2">The other bodies normal velocity</param>
		/// <param name="body2">The other body</param>
		/// <returns>The updated normalVelocity1</returns>
		public double CalculateNewNormalVelocity(double normalVelocity1, Body body1, double normalVelocity2, Body body2)
		{
			return normalVelocity1 * (body1.Mass - body2.Mass) + 2 * body2.Mass * normalVelocity2 / body1.Mass + body2.Mass;
		}
		public void UpdateSelected()
		{
			Console.WriteLine(selectedBody);
			if (selectedBody is Body)
			{
				selectedVelocityLabel.Content = "" + Math.Round(((Body)selectedBody).Velocity.Length, 2);
			}

		}
	}
}
