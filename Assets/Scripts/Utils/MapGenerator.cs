using System;
using System.Collections.Generic;
using Physics;
using UnityEngine;
using Visuals;
using Random = UnityEngine.Random;

namespace Utils
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Randomization")] 
        public int seed = 2137;

        [Header("Parameters")] 
        public int starCount;
        public int maxPlanetCount;
        public float mapSize;

        [Header("Data")] 
        public PhysicsSystem physicsSystem;
        public Star starPrefab;
        public Planet planetPrefab;

        public void Start()
        {
            GenerateMap(seed);
        }

        private void GenerateMap(int seed)
        {
            Random.InitState(seed);

            // Star generation
            for (int i = 0; i < starCount; i++)
            {
                var starGo = Instantiate(starPrefab.gameObject);
                var starTr = starGo.transform;
                var starVis = starGo.GetComponent<Star>();
                var att = starGo.GetComponent<Attractor>();

                starTr.position = new Vector3(
                    (Random.value - .5f) * mapSize, 
                    (Random.value - .5f) * mapSize, 
                    0);
                starVis.attractor.mass = Random.value * 200f + 50f;

                var starTemp = Random.Range(0, 100);
                var starColor = new Color(
                    (180 - starTemp * 0.75f) / 255f, 
                    (180 - starTemp) / 255f, 
                    (80 + starTemp * 1.5f) / 255f);
                var starSize = Random.Range(1f, 4f);

                starVis.sprite.color = starColor;
                starVis.starLight.color = starColor;
                starVis.transform.localScale = new Vector3(starSize, starSize, 1);
                starVis.starLight.intensity = starSize * 0.75f;
                starVis.starLight.pointLightOuterRadius *= (float)Math.Sqrt(starSize) / 2;

                physicsSystem.attractors.Add(att);

                var planets = Random.Range(0, maxPlanetCount);

                for (int j = 0; j < planets; j++)
                {
                    var planetGo = Instantiate(planetPrefab.gameObject);
                    var planetTr = planetGo.transform;
                    var planetVis = planetGo.GetComponent<Planet>();
                    var planetPhysics = planetGo.GetComponent<PhysicsBody>();

                    var orbitHeight = 5f + Random.value * 40f;
                    var starPos = starTr.position;
                    var point = new Vector2(starPos.x, starPos.y) + 
                                Random.insideUnitCircle.normalized * orbitHeight;
                    var accel = physicsSystem.Gravity(point);
                    var v = new Vector2(accel.normalized.y, -accel.normalized.x);
                    var scale = (float)Math.Sqrt(PhysicsSystem.G * att.mass * physicsSystem.physicsScale / orbitHeight);
                    v *= scale * (0.8f + Random.value * 0.4f);

                    planetTr.position = point;
                    planetPhysics.mass = Random.value * 4f + 1f;
                    planetPhysics.initialVelocity = v;

                    var planetColor = new Color(
                        Random.value, 
                        Random.value * .5f + .5f, 
                        Random.value * .5f + .5f);

                    var planetSize = Random.Range(0.3f, 1.2f);

                    planetTr.localScale = new Vector3(planetSize, planetSize, 1);
                    planetVis.sprite.color = planetColor;
                    var colorKeys = new GradientColorKey[2]
                    {
                        new GradientColorKey(planetColor, 0),
                        new GradientColorKey(planetColor, 1)
                    };
                    var alphaKeys = new GradientAlphaKey[2]
                    {
                        new GradientAlphaKey(15, 0),
                        new GradientAlphaKey(0, 1)
                    };
                    planetVis.trail.colorGradient.SetKeys(colorKeys, alphaKeys);
                }
            }
        }
    }
}
