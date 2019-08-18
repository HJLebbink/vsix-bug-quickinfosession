using Microsoft.VisualStudio.Language.Intellisense;
using System.Windows;

namespace VsixBug.QuickInfo
{
    public partial class BugWindow : IInteractiveQuickInfoContent
    {
        public BugWindow()
        {
            this.InitializeComponent();
            MyTools.Output_INFO(string.Format("{0}:constructor", this.ToString()));
            
            this.MyExpander.MouseLeftButtonDown += (o, i) =>
            {
                //MyTools.Output_INFO(string.Format("{0}:MouseLeftButtonDown", this.ToString()));
                //i.Handled = true; // dont let the mouse event from inside this window bubble up to VS
            };

            this.MyExpander.PreviewMouseLeftButtonDown += (o, i) =>
            {
                //i.Handled = true; // if true then no event is able to bubble to the gui
                if (false)
                {
                    MyTools.Output_INFO(string.Format("{0}:PreviewMouseLeftButtonDown", this.ToString()));
                    Point locationFromScreen = this.MainWindow.PointToScreen(new Point(0, 0));
                    PresentationSource source = PresentationSource.FromVisual(this);
                    System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                    MyTools.Output_INFO(string.Format("{0}:PreviewMouseLeftButtonDown; x={1}; y={2}", this.ToString(), targetPoints.X, targetPoints.Y));
                }
            };

            this.MyExpander.MouseLeftButtonUp += (o, i) =>
            {
               //MyTools.Output_INFO(string.Format("{0}:MouseLeftButtonUp", this.ToString()));
            };

            this.MyExpander.GotMouseCapture += (o, i) =>
            {
                //i.Handled = true; // if true then no event is able to bubble to the gui
                if (false)
                {
                    MyTools.Output_INFO(string.Format("{0}:GotMouseCapture", this.ToString()));
                    Point locationFromScreen = this.MainWindow.PointToScreen(new Point(0, 0));
                    PresentationSource source = PresentationSource.FromVisual(this);
                    System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                    MyTools.Output_INFO(string.Format("{0}:GotMouseCapture; x={1}; y={2}", this.ToString(), targetPoints.X, targetPoints.Y));
                }
            };

            this.MyExpander.SizeChanged += (o, i) =>
            {
                if (false)
                {
                    MyTools.Output_INFO(string.Format("{0}:SizeChanged; desired.height={1}; actual.height={2}", this.ToString(), this.MyExpander.DesiredSize.Height, this.MyExpander.ActualHeight));
                    Point locationFromScreen = this.MainWindow.PointToScreen(new Point(0, 0));
                    PresentationSource source = PresentationSource.FromVisual(this);
                    System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                    MyTools.Output_INFO(string.Format("{0}:MyExpander_SizeChanged; x={1}; y={2}", this.ToString(), targetPoints.X, targetPoints.Y));

                    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement.onchilddesiredsizechanged
                    //this.MainWindow.OnChildDesiredSizeChanged(this.MyExpander);
                    //this.MainWindow.InvalidateMeasure();
                    //this.MainWindow.InvalidateVisual();
                    //this.MainWindow.ParentLayoutInvalidated(this.MyExpander);
                    //this.MainWindow.OnRenderSizeChanged();
                }
            };

            //this.MyExpander.LayoutUpdated += (o, i) =>
            //{
            //    MyTools.Output_INFO(string.Format("{0}:LayoutUpdated", this.ToString()));
            //};

            this.MyExpander.Collapsed += (o, i) =>
            {
                //MyTools.Output_INFO(string.Format("{0}:Collapsed", this.ToString()));
            };

            this.MainWindow.SizeChanged += (o, i) =>
            {
                if (false)
                {
                    MyTools.Output_INFO(string.Format("{0}:Collapsed", this.ToString()));
                    Point locationFromScreen = this.MainWindow.PointToScreen(new Point(0, 0));
                    PresentationSource source = PresentationSource.FromVisual(this);
                    System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                    MyTools.Output_INFO(string.Format("{0}:MainWindow_SizeChanged; x={1}; y={2}", this.ToString(), targetPoints.X, targetPoints.Y));
                }
            };
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var result = base.MeasureOverride(constraint);
            if (false)
            {
                Point locationFromScreen = this.MainWindow.PointToScreen(new Point(0, 0));
                PresentationSource source = PresentationSource.FromVisual(this);
                System.Windows.Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                MyTools.Output_INFO(string.Format("{0}:MeasureOverride; x={1}; y={2}; height={3}; width={4}", this.ToString(), targetPoints.X, targetPoints.Y, result.Height, result.Width));

                this.InvalidateVisual();
            }
            return result;
        }

        public bool KeepQuickInfoOpen => this.IsMouseOverAggregated || this.IsKeyboardFocusWithin || this.IsKeyboardFocused || this.IsFocused;

        public bool IsMouseOverAggregated => this.IsMouseOver || this.IsMouseDirectlyOver;

    }
}
