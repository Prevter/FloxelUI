using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace FloxelUI.Controls;

public sealed class Arc : FrameworkElement
{
	public Point Center
	{
		get { return (Point)GetValue(CenterProperty); }
		set { SetValue(CenterProperty, value); }
	}

	public static readonly DependencyProperty CenterProperty =
		DependencyProperty.Register("Center", typeof(Point), typeof(Arc), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

	public bool OverrideCenter
	{
		get { return (bool)GetValue(OverrideCenterProperty); }
		set { SetValue(OverrideCenterProperty, value); }
	}

	public static readonly DependencyProperty OverrideCenterProperty =
		DependencyProperty.Register("OverrideCenter", typeof(bool), typeof(Arc), new FrameworkPropertyMetadata((bool)false, FrameworkPropertyMetadataOptions.AffectsRender));

	public double StartAngle
	{
		get { return (double)GetValue(StartAngleProperty); }
		set { SetValue(StartAngleProperty, value); }
	}

	public static readonly DependencyProperty StartAngleProperty =
		DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

	public double SweepAngle
	{
		get { return (double)GetValue(SweepAngleProperty); }
		set { SetValue(SweepAngleProperty, value); }
	}

	public static readonly DependencyProperty SweepAngleProperty =
		DependencyProperty.Register("SweepAngle", typeof(double), typeof(Arc), new FrameworkPropertyMetadata((double)180, FrameworkPropertyMetadataOptions.AffectsRender));

	public double Radius
	{
		get { return (double)GetValue(RadiusProperty); }
		set { SetValue(RadiusProperty, value); }
	}

	public static readonly DependencyProperty RadiusProperty =
		DependencyProperty.Register("Radius", typeof(double), typeof(Arc), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

	public Brush Stroke
	{
		get { return (Brush)GetValue(StrokeProperty); }
		set { SetValue(StrokeProperty, value); }
	}

	public static readonly DependencyProperty StrokeProperty =
		DependencyProperty.Register("Stroke", typeof(Brush), typeof(Arc), new FrameworkPropertyMetadata((Brush)Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

	public double StrokeThickness
	{
		get { return (double)GetValue(StrokeThicknessProperty); }
		set { SetValue(StrokeThicknessProperty, value); }
	}

	public static readonly DependencyProperty StrokeThicknessProperty =
		DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Arc), new FrameworkPropertyMetadata((double)1, FrameworkPropertyMetadataOptions.AffectsRender));

	protected override void OnRender(DrawingContext dc)
	{
		base.OnRender(dc);
		Draw(dc);
	}

	private void Draw(DrawingContext dc)
	{
		Point center;
		if (OverrideCenter)
		{
			Rect rect = new(RenderSize);
			center = Polar.CenterPoint(rect);
		}
		else
		{
			center = Center;
		}

		Point startPoint = Polar.PolarToCartesian(StartAngle, Radius, center);
		Point endPoint = Polar.PolarToCartesian(StartAngle + SweepAngle, Radius, center);
		Size size = new(Radius, Radius);

		bool isLarge = (StartAngle + SweepAngle) - StartAngle > 180;

		List<PathSegment> segments = new(1)
		{
			new ArcSegment(endPoint, size, 0.0, isLarge, SweepDirection.Clockwise, true)
		};

		List<PathFigure> figures = new(1);
		PathFigure pf = new(startPoint, segments, true)
		{
			IsClosed = false
		};
		figures.Add(pf);
		Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

		dc.DrawGeometry(null, new Pen(Stroke, StrokeThickness), g);
	}
}

public static class Polar
{
	public static Point PolarToCartesian(double angle, double radius, Point center)
	{
		return new Point((center.X + (radius * Math.Cos(DegreesToRadian(angle)))), (center.Y + (radius * Math.Sin(DegreesToRadian(angle)))));
	}

	public static Rect RectFromCenterPoint(Point centerPoint, int radius)
	{
		Point p = new Point(centerPoint.X - radius, centerPoint.Y - radius);
		return new Rect(p, new Size(radius * 2, radius * 2));
	}

	public static Point CenterPoint(Rect rect)
	{
		return new Point(rect.Width / 2, rect.Height / 2);
	}

	public static double Radius(Rect rect)
	{
		double dbl = Math.Min(rect.Width, rect.Height);
		return dbl / 2;
	}

	public static float ReversePolarDirection(float Angle, int Offset)
	{
		return ((360 - Angle) + Offset) % 360;
	}

	public static double CircumferenceD(double Diameter)
	{
		return Diameter * Math.PI;
	}
	
	public static double CircumferenceR(double Radius)
	{
		return Radius * Math.PI;
	}
	public static double ScaleWithParam(double Input, double InputMin, double InputMax, double ScaledMin, double ScaledMax)
	{
		return (((ScaledMax - ScaledMin) / (InputMax - InputMin)) * Input) + (ScaledMin - (InputMin * ((ScaledMax - ScaledMin) / (InputMax - InputMin))));
	}

	public static double DegreesToRadian(double degrees)
	{
		return degrees * (Math.PI / 180);
	}

	private static double RadianToDegrees(double radian)
	{
		return radian * 180 / Math.PI;
	}
	
	public static double ArcLength(double radius, double radian)
	{
		return radius * radian;
	}
}
