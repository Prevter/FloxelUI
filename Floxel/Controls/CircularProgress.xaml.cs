using FloxelLib.Common;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace FloxelLib.Controls;

/// <summary>
/// Interaction logic for CircularProgress.xaml
/// </summary>
public partial class CircularProgress : UserControl
{
	private const double _speed = 1.4;
	private double _progress = 0.0;
	private long _lastTick = 0;

	private static readonly Dictionary<double, double> _startAngleKeyframes = new()
	{
		{ 0, 0 },
		{ 0.5, 30 },
		{ 1, 355 }
	};
	private static readonly Dictionary<double, double> _endAngleKeyframes = new()
	{
		{ 0, 5 },
		{ 0.5, 330 },
		{ 1, 360 }
	};
	private static readonly Dictionary<double, double> _rotationAngleKeyframes = new()
	{
		{ 0, 0 },
		{ 1, 360 }
	};

	public CircularProgress()
	{
		InitializeComponent();
		CompositionTarget.Rendering += CompositionTarget_Rendering;
		_lastTick = Stopwatch.GetTimestamp();
	}

	private void CompositionTarget_Rendering(object? sender, EventArgs e)
	{
		var now = Stopwatch.GetTimestamp();
		var delta = Stopwatch.GetTimePassed(_lastTick, now);
		_lastTick = now;

		_progress += delta.TotalSeconds / _speed;
		_progress %= 1.0;

		double startAngle = Animations.MapValues(_progress, Animations.EaseInOutSine, _startAngleKeyframes);
		double endAngle = Animations.MapValues(_progress, Animations.EaseInOutSine, _endAngleKeyframes);
		double rotationAngle = Animations.MapValues(_progress, Animations.Linear, _rotationAngleKeyframes);

		arc.StartAngle = startAngle + rotationAngle;
		arc.SweepAngle = endAngle - startAngle;
	}
}
