using System.Drawing;
using MonoTouch.UIKit;

namespace SplitPanelContainer.SplitPanels
{
    public abstract class PanelContainer : UIViewController
    {
        /// <summary>
        /// Gets the panel position.
        /// </summary>
        /// <value>The panel position.</value>
        protected abstract RectangleF PanelPosition { get; }

        protected virtual RectangleF ChildViewPosition
        {
            get { return new RectangleF(new PointF(1, 1), new SizeF(PanelPosition.Width - 2, PanelPosition.Height - 2)); }
        }

        /// <summary>
        /// Gets the view controller contained inside this panel
        /// </summary>
        /// <value>The panel V.</value>
        public UIViewController PanelView
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the panel is currently showing
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
        public virtual bool IsVisible
        {
            get
            {
                return !View.Hidden;
            }
        }

        /// <summary>
        /// Gets the size of the panel
        /// </summary>
        /// <value>The size.</value>
        public virtual SizeF Size
        {
            get;
            private set;
        }

        #region Construction / Destruction

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelContainer"/> class.
        /// </summary>
        protected PanelContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelContainer"/> class.
        /// </summary>
        /// <param name="panel">Panel.</param>
        protected PanelContainer(UIViewController panel)
        {
            PanelView = panel;
            //View.Frame = PanelPosition;
            //PanelView.View.Frame = ChildViewPosition;
            Size = panel.View.Frame.Size;
        }
        #endregion

        #region View Lifecycle

        /// <summary>
        /// Called when the panel view is first loaded
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.Frame = PanelPosition;
            PanelView.View.Frame = ChildViewPosition;

            AddChildViewController(PanelView);
            View.AddSubview(PanelView.View);

            //Show();
        }

        /// <summary>
        /// Called every time the Panel is about to be shown
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            //View.Frame = PanelPosition;
            //PanelView.View.Frame = ChildViewPosition;
            PanelView.ViewWillAppear(animated);
            base.ViewWillAppear(animated);
        }

        /// <summary>
        /// Called every time after the Panel is shown
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewDidAppear(bool animated)
        {
            //View.Frame = PanelPosition;
            //PanelView.View.Frame = ChildViewPosition;
            PanelView.ViewDidAppear(animated);
            base.ViewDidAppear(animated);
        }

        /// <summary>
        /// Called whenever the Panel is about to be hidden
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillDisappear(bool animated)
        {
            PanelView.ViewWillDisappear(animated);
            base.ViewWillDisappear(animated);
        }

        /// <summary>
        /// Called every time after the Panel is hidden
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewDidDisappear(bool animated)
        {
            PanelView.ViewDidDisappear(animated);
            base.ViewDidDisappear(animated);
        }
        #endregion

        #region Visibility Control

        /// <summary>
        /// Toggle the visibility of this panel
        /// </summary>
        public virtual void Toggle()
        {
            if (View.Hidden)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// Makes this Panel visible
        /// </summary>
        public virtual void Show()
        {
            View.Layer.ZPosition = -1;
            View.Hidden = false;
        }

        /// <summary>
        /// Hides this Panel
        /// </summary>
        public virtual void Hide()
        {
            View.Hidden = true;
        }

        #endregion

        public void SwapChildView(UIViewController newChildView)
        {
            if (newChildView == null) return;

            newChildView.View.Frame = ChildViewPosition;
            Size = newChildView.View.Frame.Size;

            if (PanelView == null)
            {
                AddChildViewController(newChildView);
                View.AddSubview(newChildView.View);
            }
            else
            {
                AddChildViewController(newChildView);
                View.AddSubview(newChildView.View);
                //newChildView.WillMoveToParentViewController(null);
                PanelView.WillMoveToParentViewController(null);
                //                Transition(PanelView, newChildView, 1.0, UIViewAnimationOptions.CurveEaseOut, () => { },
                //                    (finished) =>
                //                    {
                //                        PanelView.RemoveFromParentViewController();
                //                        newChildView.DidMoveToParentViewController(this);
                //                        PanelView = newChildView;
                //                    });
                PanelView.RemoveFromParentViewController();
                newChildView.DidMoveToParentViewController(this);
                PanelView = newChildView;
            }
        }
    }
}