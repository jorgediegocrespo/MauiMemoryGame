using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace MauiMemoryGame.Controls;

public class CircleProgressBar : SKCanvasView
{
    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircleProgressBar), 10f, propertyChanged: OnPropertyChanged);
    public static readonly BindableProperty LineBackgroundColorProperty = BindableProperty.Create(nameof(LineBackgroundColor), typeof(Color), typeof(CircleProgressBar), Colors.Gray, propertyChanged: OnPropertyChanged);
    public static readonly BindableProperty ProgressPercentageProperty = BindableProperty.Create(nameof(ProgressPercentage), typeof(float), typeof(CircleProgressBar), 0f, propertyChanged: OnPropertyChanged);
    public static readonly BindableProperty ProgressBarColorProperty = BindableProperty.Create(nameof(ProgressBarColor), typeof(Color), typeof(CircleProgressBar), Colors.Red, propertyChanged: OnPropertyChanged);

    public float StrokeWidth
    {
        get { return (float)GetValue(StrokeWidthProperty); }
        set { SetValue(StrokeWidthProperty, value); }
    }

    public Color LineBackgroundColor
    {
        get { return (Color)GetValue(LineBackgroundColorProperty); }
        set { SetValue(LineBackgroundColorProperty, value); }
    }

    public float ProgressPercentage
    {
        get { return (float)GetValue(ProgressPercentageProperty); }
        set { SetValue(ProgressPercentageProperty, value); }
    }

    public Color ProgressBarColor
    {
        get { return (Color)GetValue(ProgressBarColorProperty); }
        set { SetValue(ProgressBarColorProperty, value); }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        InvalidateSurface();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
    {
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        int size = Math.Min(info.Width, info.Height);

        canvas.Translate((info.Width - size) / 2, (info.Height - size) / 2);

        canvas.Clear();
        canvas.Save();
        canvas.RotateDegrees(270, size / 2, size / 2);

        DrawBackgroundCircle(info, canvas);
        DrawProgressProgressCircle(info, canvas);

        canvas.Restore();
    }

    private void DrawBackgroundCircle(SKImageInfo info, SKCanvas canvas)
    {
        var paint = new SKPaint
        {
            Color = LineBackgroundColor.ToSKColor(),
            StrokeWidth = StrokeWidth,
            IsStroke = true,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };

        DrawCircle(info, canvas, paint, 360f);
    }

    private void DrawProgressProgressCircle(SKImageInfo info, SKCanvas canvas)
    {
        float progressAngle = 360f * ProgressPercentage / 100;

        var paint = new SKPaint
        {
            Color = ProgressBarColor.ToSKColor(),
            StrokeWidth = StrokeWidth,
            IsStroke = true,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };

        DrawCircle(info, canvas, paint, progressAngle);
    }

    private void DrawCircle(SKImageInfo info, SKCanvas canvas, SKPaint paint, float angle)
    {
        int size = Math.Min(info.Width, info.Height);
        using (SKPath path = new SKPath())
        {

            SKRect rect = new SKRect(
                StrokeWidth,
                StrokeWidth,
                size - StrokeWidth,
                size - StrokeWidth);
            path.AddArc(rect, 0f, angle);

            canvas.DrawPath(path, paint);
        }
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldVal, object newVal)
    {
        var circleProgress = bindable as CircleProgressBar;
        circleProgress?.InvalidateSurface();
    }
}
