using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TryExtended.Components
{
    public partial class CameraController : DrawableGameComponent
    {
        public float scrollValue;

        private int _zoomDirection;
        private float _zoomSpeed = 0.5f;
        private float _targetZoom;
        private float _zoomAmount;

        private double elapsed;

        private void UpdateZoom(GameTime gt, MouseState mouse)
        {
            float zoomAmount;
            int compared = scrollValue.CompareTo(mouse.ScrollWheelValue);

            if (compared != 0)
            {
                _zoomDirection = compared;
                _zoomAmount = _zoomSpeed;
                _targetZoom = Camera.Zoom + ((mouse.ScrollWheelValue - scrollValue) / 120);
                scrollValue = mouse.ScrollWheelValue;
                elapsed = gt.ElapsedGameTime.TotalSeconds;
            }

            if (_zoomDirection != 0)
            {
                if (_zoomDirection == -1)
                    Camera.ZoomOut(MathHelper.SmoothStep(_zoomAmount, _targetZoom, (float)elapsed));
                else if (_zoomDirection == 1)
                    Camera.ZoomIn(MathHelper.SmoothStep(_targetZoom, _zoomAmount, (float)elapsed));

                if (Camera.Zoom == _targetZoom)
                {
                    _zoomDirection = 0;
                }

                elapsed += gt.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
}