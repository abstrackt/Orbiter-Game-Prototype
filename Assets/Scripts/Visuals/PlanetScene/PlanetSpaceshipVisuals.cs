using System.Collections.Generic;
using Systems.StarsScene;
using UnityEngine;
using Vectrosity;

namespace Visuals.PlanetScene
{
    public class PlanetSpaceshipVisuals : MonoBehaviour
    {
        public ParticleSystem particles;
        public float lineWidth = 2f;
        public Color32 lineColor;

        private PlanetSpaceshipController _controller;
        private VectorLine _trajectory;

        public void Start()
        {
            _trajectory = new VectorLine("Trajectory", new List<Vector3>(), lineWidth, LineType.Discrete);
            _trajectory.joins = Joins.Fill;
            _controller = PlanetSpaceshipController.Instance;
        }
        
        public void Update()
        {
            var prediction = _controller.Trajectory;
            _trajectory.points3.Clear();
            for (int i = 1; i < prediction.Count; i++)
            {
                _trajectory.points3.Add(new Vector3(prediction[i-1].x, prediction[i-1].y, 1));
                _trajectory.points3.Add(new Vector3(prediction[i].x, prediction[i].y, 1));
            }
            var color = lineColor;
            _trajectory.Draw3D();
            _trajectory.SetColor(color);
        }
        
        public void OnDisable()
        {
            VectorLine.Destroy(ref _trajectory);
        }
    }
}