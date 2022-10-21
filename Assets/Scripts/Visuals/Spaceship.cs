using System.Collections.Generic;
using UnityEngine;
using Utils;
using Vectrosity;

namespace Visuals
{
    public class Spaceship : MonoBehaviour
    {
        public SpaceshipController controller;
        public ParticleSystem particles;
        public MapDefinition map;
        public float lineWidth = 2f;
        public float trajectoryDrawRange = 200f;
        public Color32 lineColor;

        private VectorLine _trajectory;

        public void Start()
        {
            _trajectory = new VectorLine("Trajectory", new List<Vector3>(), lineWidth, LineType.Discrete);
            _trajectory.joins = Joins.Fill;
        }
        
        public void Update()
        {
            if (controller.Maneuvering)
            {
                if (!particles.isPlaying)
                    particles.Play();
                var emission = particles.emission;
                emission.rateOverTime = 20;
            }
            else if (!controller.Maneuvering)
            {
                var emission = particles.emission;
                emission.rateOverTime = 0;
            }

            if (map.ClosestStar.star != null && map.ClosestStar.dist < trajectoryDrawRange)
            {
                var prediction = controller.Trajectory;
                _trajectory.points3.Clear();
                for (int i = 1; i < prediction.Count; i++)
                {
                    _trajectory.points3.Add(new Vector3(prediction[i-1].x, prediction[i-1].y, 1));
                    _trajectory.points3.Add(new Vector3(prediction[i].x, prediction[i].y, 1));
                }
                var color = lineColor;
                color.a = (byte)((trajectoryDrawRange - map.ClosestStar.dist) / trajectoryDrawRange * color.a);
                _trajectory.Draw3D();
                _trajectory.SetColor(color);
            }
        }
    }
}