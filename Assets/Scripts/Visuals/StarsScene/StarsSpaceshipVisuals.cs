using System.Collections.Generic;
using Systems.StarsScene;
using UnityEngine;
using Vectrosity;

namespace Visuals.StarsScene
{
    public class StarsSpaceshipVisuals : MonoBehaviour
    {
        public ParticleSystem particles;
        public float lineWidth = 2f;
        public float trajectoryDrawRange = 200f;
        public Color32 lineColor;

        private StarsSpaceshipController _controller;
        private StarsMapManager _map;
        private VectorLine _trajectory;

        public void Start()
        {
            _trajectory = new VectorLine("Trajectory", new List<Vector3>(), lineWidth, LineType.Discrete);
            _trajectory.joins = Joins.Fill;

            _controller = StarsSpaceshipController.Instance;
            _map = StarsMapManager.Instance;
        }
        
        public void Update()
        {
            if (_controller.Maneuvering)
            {
                if (!particles.isPlaying)
                    particles.Play();
                var emission = particles.emission;
                emission.rateOverTime = 20;
            }
            else if (!_controller.Maneuvering)
            {
                var emission = particles.emission;
                emission.rateOverTime = 0;
            }

            if (_map.ClosestStarVisuals.dist < trajectoryDrawRange)
            {
                var prediction = _controller.Trajectory;
                _trajectory.points3.Clear();
                for (int i = 1; i < prediction.Count; i++)
                {
                    _trajectory.points3.Add(new Vector3(prediction[i-1].x, prediction[i-1].y, 1));
                    _trajectory.points3.Add(new Vector3(prediction[i].x, prediction[i].y, 1));
                }
                var color = lineColor;
                color.a = (byte)((trajectoryDrawRange - _map.ClosestStarVisuals.dist) / trajectoryDrawRange * color.a);
                _trajectory.Draw3D();
                _trajectory.SetColor(color);
            }
            else
            {
                _trajectory.points3.Clear();
                _trajectory.Draw3D();
            }
        }

        public void OnDisable()
        {
            _trajectory.points3.Clear();
            _trajectory.Draw3D();
        }
    }
}