﻿using SonicRealms.Core.Triggers;
using UnityEngine;

namespace SonicRealms.Level.Platforms.Movers
{
    /// <summary>
    /// Moves down when a controller stands on it and back up when it leaves.
    /// </summary>
    public class WeightedPlatform : BasePlatformMover
    {
        /// <summary>
        /// The time to wait before a platform depresses after a controller stands on it.
        /// </summary>
        [SerializeField] public float DepressDelay;

        /// <summary>
        /// The amount by which to depress the platform when a controller stands on it.
        /// </summary>
        [SerializeField] public Vector2 DepressionAmount;

        /// <summary>
        /// Whether to return when a controller leaves it.
        /// </summary>
        [SerializeField] public bool Return;

        /// <summary>
        /// How long to wait after a controller leaves it to begin returning.
        /// </summary>
        [SerializeField] public float ReturnDelay;

        private float _returnTimer;
        private bool _returning;
        private bool _colliding;

        private float _tprev;

        public override void Reset()
        {
            base.Reset();

            Duration = 0.5f;
            DepressDelay = 0.0f;

            EasingCurve = new AnimationCurve(
                new Keyframe(0, 0, 0, 2),
                new Keyframe(1, 1, 0, 0));

            DepressionAmount = Vector2.up*0.1f;
            Return = true;
        }

        public override void Awake()
        {
            base.Awake();
            _returnTimer = 0.0f;
            _returning = false;
            _tprev = CurrentTime/Duration;
        }

        /// <summary>
        /// Prevents the platform from returning for the duration of ReturnDelay.
        /// </summary>
        public void DelayReturn()
        {
            _returning = true;
            _returnTimer = 0.0f;
        }

        public override void UpdateTimer(float timestep)
        {
            if (_colliding)
            {
                CurrentTime += timestep;
                if (CurrentTime > Duration) CurrentTime = Duration;
            }
            else if(Return)
            {
                if (_returning)
                {
                    _returnTimer += timestep;
                    if (_returnTimer > ReturnDelay)
                    {
                        _returnTimer = ReturnDelay;
                        _returning = false;
                    }
                }
                else
                {
                    CurrentTime -= timestep;
                    if (CurrentTime < 0.0f) CurrentTime = 0.0f;
                }
            }
        }

        public override void To(float t)
        {
            transform.localPosition -= (Vector3)DepressionAmount*(t - _tprev);
            _tprev = t;
        }

        // Check if a controller is on the platform
        public override void OnSurfaceEnter(SurfaceCollision collision)
        {
            _colliding = true;
        }

        // Check if a controller is on the platform
        public override void OnSurfaceStay(SurfaceCollision collision)
        {
            _colliding = true;
        }
            
        // Check if a controller leaves the platform
        public override void OnSurfaceExit(SurfaceCollision collision)
        {
            if (!_colliding) return;
            _colliding = false;
            if (Return) DelayReturn();
        }
    }
}