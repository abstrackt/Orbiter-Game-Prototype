using System;
using System.Collections.Generic;
using Data.Map;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Utils
{
    public static class MapGenerator
    {
        // Overload this when you will be creating more physically-correct star generation my beloved <3
        public static WorldData GenerateWorldData(uint seed, uint stars, uint avgPlanets, int mapSize)
        {
            var rd = new Random(seed);

            var world = new WorldData()
            {
                systems = new()
            };

            var starTypes = Enum.GetValues(typeof(StarType));
            var planetTypes = Enum.GetValues(typeof(PlanetType));
            
            for (int i = 0; i < stars; i++)
            {
                var system = new StarSystemData
                {
                    planets = new()
                };
                
                // Generate some random star

                var star = new StarData
                {
                    initPosition = rd.NextFloat2(-mapSize / 2, mapSize /2),
                    age = rd.NextFloat(1, 1450),
                    magneticField = rd.NextFloat(0, 100),
                    starType = (StarType)starTypes.GetValue(rd.NextInt(starTypes.Length - 1)),
                    mass = rd.NextFloat(10f, 100f),
                    radius = rd.NextFloat(0.5f, 2f),
                    temperature = rd.NextFloat(0.5f, 100f),
                    starName = NameGenerator.GetRandomPlanetName()
                };

                system.star = star;

                var planets = rd.NextUInt(0, avgPlanets * 2);

                for (int j = 0; j < planets; j++)
                {
                    var comp = new List<float>(9);
                    var sum = 0f;
                    
                    for (int k = 0; k < 9; k++)
                    {
                        var val = rd.NextFloat(0, 100);
                        comp.Add(val);
                        sum += val;
                    }

                    if (sum != 0)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            comp[k] /= sum;
                        }
                    }
                    
                    var atmo = new AtmosphereData()
                    {
                        ammonia = comp[0],
                        co2 = comp[1],
                        hydrogen = comp[2],
                        methane = comp[3],
                        nitrogen = comp[4],
                        nobleGases = comp[5],
                        oxygen = comp[6],
                        sulphur = comp[7],
                        waterVapor = comp[8],
                        pressure = rd.NextFloat(0, 10000f)
                    };

                    var planet = new PlanetData()
                    {
                        initPosition = star.initPosition + (Vector2)rd.NextFloat2Direction() * rd.NextFloat(20, 80),
                        age = rd.NextFloat(0, star.age - 0.5f),
                        angularVelocity = rd.NextFloat(-5, 5),
                        atmoData = atmo,
                        mass = rd.NextFloat(0.1f, 10f),
                        planetType = (PlanetType)planetTypes.GetValue(rd.NextInt(planetTypes.Length - 1)),
                        population = rd.NextFloat() > 0.1f ? 0f : rd.NextFloat(0.5f),
                        radius = rd.NextFloat(0.5f, 2f),
                        seaLevel = atmo.CanHaveLiquids ? rd.NextFloat() : null,
                        surfaceRadiation = rd.NextFloat(0, 100),
                        planetName = NameGenerator.GetRandomPlanetName()
                    };
                    
                    system.planets.Add(planet);
                }
                
                world.systems.Add(system);
            }

            return world;
        }
    }
}