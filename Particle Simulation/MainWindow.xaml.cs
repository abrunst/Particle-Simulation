using System;
using System.Collections.Generic;
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
		private GeometryGroup geometryGroup = new GeometryGroup();

		private Path path = new Path();

		private List<ParticleView> particles = new List<ParticleView>();

		/// <summary>
		/// The time of the last render
		/// </summary>
		private TimeSpan lastRender;

		/// <summary>
		/// 
		/// </summary>
		private double time = 0;

		/// <summary>
		/// The time until the next render
		/// </summary>
		private double timeUntilRender = 0;

		public MainWindow()
		{
			InitializeComponent();
			SetDefaults();
		}

		//Sets the default values for fields and properties
		public void SetDefaults()
		{
			geometryGroup.FillRule = FillRule.Nonzero;

			drawingArea.Children.Add(path);

			path.Data = geometryGroup;
			path.Fill = new SolidColorBrush(colorPicker.SelectedColor.Value);

			//Setting the size of the window to 80% of the screen
			this.Height = (System.Windows.SystemParameters.FullPrimaryScreenHeight * 0.80);
			this.Width = (System.Windows.SystemParameters.FullPrimaryScreenWidth * 0.80);

			///Setting lastRender
			lastRender = TimeSpan.FromTicks(DateTime.Now.Ticks);


			CompositionTarget.Rendering += CompositioinTarget_Rendering;
			//AddParticle(50, new Point(250, 250), Color.FromArgb(255, 200, 73, 92));
		}

		private void CompositioinTarget_Rendering(object sender, EventArgs e)
		{
			//Casting to RenderingEventArgs to get the rendering time
			RenderingEventArgs renderArgs = (RenderingEventArgs)e;

			//Gets the time until next render
			timeUntilRender = (renderArgs.RenderingTime - lastRender).TotalSeconds;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		}

		/// <summary>
		/// When the selected color is changed in the colorPicker, change the fill of the path
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			path.Fill = new SolidColorBrush(e.NewValue.Value);
		}

		/// <summary>
		/// On MouseDown event in the PictureBox, adds a new Particle to particles and refershes the drawingPanel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">The mouse event that triggered the handler</param>
		private void DrawingArea_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (onClickComboBox.SelectedIndex == 0)
			{
				ClickedInside(e.GetPosition(drawingArea));
			}
			else if (onClickComboBox.SelectedIndex == 1)
			{
				Console.WriteLine(e.GetPosition(drawingArea));
				float radius = (float)radiusIUD.Value;
				Point coordinates = e.GetPosition(drawingArea);

				AddParticle(radius, coordinates);
			}
		}

		/// <summary>
		/// Determines if a click is inside a particle and if it is, changes its colour to red
		/// </summary>
		/// <param name="x">The x coordinate of the click</param>
		/// <param name="y">The y coordinate of the click</param>
		public void ClickedInside(Point coordinates)
		{
			foreach (ParticleView particle in particles)
			{
				if (particle.IsInside(particle.Coordinates))
				{

				}
			}
		}

		/// <summary>
		/// Adds a particle to particles and draws it
		/// </summary>
		/// <param name="radius">The radius of the particle</param>
		/// <param name="coordinates">The cooridnates of the particle</param>
		/// <param name="color">The color of the particle</param>
		public void AddParticle(float radius, Point coordinates)
		{
			ParticleView particle = new ParticleView(coordinates, radius);
			particles.Add(particle);

			particle.AddToDrawing(geometryGroup);

			
		}

		public void Animation()
		{
		}

		
	}
}
