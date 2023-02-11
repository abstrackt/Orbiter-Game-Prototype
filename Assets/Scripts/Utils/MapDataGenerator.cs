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

            var world = new WorldData
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

                var starType =(StarType)starTypes.GetValue(rd.NextInt(starTypes.Length - 1));
                var temperature = rd.NextFloat(0.5f, 50f);
                var radius = rd.NextFloat(1f, 2f);

                if (starType is StarType.GiantBranch)
                {
                    temperature *= 0.1f;
                    radius *= 4f;
                }
                
                if (starType is StarType.SuperBranch)
                {
                    radius *= 5f;
                }

                if (starType is StarType.CarbonStar or StarType.IronStar)
                {
                    temperature *= 0.02f;
                    radius *= 0.5f;
                }
                
                if (starType is StarType.WhiteDwarf or StarType.NeutronStar)
                {
                    temperature *= 5f;
                    radius *= 0.25f;
                }

                var star = new StarData
                {
                    initPosition = rd.NextFloat2(-mapSize / 2, mapSize /2),
                    age = rd.NextFloat(1, 1450),
                    magneticField = rd.NextFloat(0, 100),
                    starType = starType,
                    mass = rd.NextFloat(10f, 100f),
                    radius = radius,
                    temperature = temperature,
                    starName = NameGenerator.GetRandomStarName()
                };

                system.star = star;
                var planets = rd.NextUInt(0, (uint) (avgPlanets * 2 * Mathf.Max(1, Mathf.Log(radius, 5))));

                for (int j = 0; j < planets; j++)
                {
                    var comp = new List<float>(AtmosphereData.ELEMENT_COUNT);
                    var sum = 0f;
                    
                    for (int k = 0; k < AtmosphereData.ELEMENT_COUNT; k++)
                    {
                        var val = rd.NextFloat(0, 100);
                        comp.Add(val);
                        sum += val;
                    }

                    if (sum != 0)
                    {
                        for (int k = 0; k < AtmosphereData.ELEMENT_COUNT; k++)
                        {
                            comp[k] /= sum;
                        }
                    }
                    
                    var dist = rd.NextFloat(radius + 10, radius + 100);
                    var planetPos = star.initPosition + (Vector2) rd.NextFloat2Direction() * dist;

                    var surfType = (PlanetType) planetTypes.GetValue(rd.NextInt(planetTypes.Length - 1));
                    var isGas = surfType is PlanetType.Giant or PlanetType.Subgiant;

                    var atmo = new AtmosphereData
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
                        pressure = rd.NextFloat(
                            AtmosphereData.MAX_PRESSURE * (isGas ? 0.75f : 0), 
                            AtmosphereData.MAX_PRESSURE * (isGas ? 1f : 0.35f)),
                        temperature = Mathf.Lerp(
                            200f * Mathf.Sqrt(star.temperature), 
                            -260f + star.temperature, 
                            Mathf.Sqrt((dist - radius) / 150f))
                    };
                    
                    var isIce = surfType is PlanetType.Icy;
                    if (isIce && atmo.temperature > -10f)
                    {
                        isIce = false;
                        surfType = PlanetType.Rocky;
                    }

                    bool hasRings = rd.NextFloat() < 0.1f;

                    RingData ring = default;

                    if (hasRings)
                    {
                        var inner = rd.NextFloat(0.013f, 0.025f);

                        ring = new RingData
                        {
                            innerRadius = rd.NextFloat(0.013f, 0.014f),
                            outerRadius = inner + rd.NextFloat(0.01f, 0.04f),
                            opacity = rd.NextFloat(0.1f, 0.6f)
                        };
                    }

                    var seaType = SeaType.Gas;

                    if (!isGas)
                    {
                        if (atmo.temperature < 100f)
                        {
                            seaType = rd.NextBool() ? SeaType.Water : SeaType.Carbohydrate;
                        }
                        else if (atmo.temperature >= 100f && atmo.temperature < 500f)
                        {
                            seaType = SeaType.Sulfur;
                        }
                        else
                        {
                            seaType = SeaType.Lava;
                        }
                    }
                    
                    var minV = isGas || isIce ? 0.85f : 0.6f;
                    var rad = isGas ? rd.NextFloat(1.5f, 3f) : rd.NextFloat(0.5f, 1.5f);
                    
                    var planet = new PlanetData
                    {
                        initPosition = planetPos,
                        age = rd.NextFloat(0, star.age - 0.5f),
                        angularVelocity = rd.NextFloat(-5, 5),
                        atmoData = atmo,
                        ringData = ring,
                        hasRings = hasRings,
                        mass = (isGas ? 0.3f : 0.6f) * MathF.PI * MathF.Pow(rad, 3),
                        planetType = surfType,
                        seaType = seaType,
                        population = (isGas || rd.NextFloat() > 0.1f) ? 0f : rd.NextFloat(0.5f),
                        radius = rad,
                        seaLevel = atmo.CanHaveSeas ? rd.NextFloat(0.25f, 0.7f) : 0,
                        surfaceRadiation = rd.NextFloat(0, 100),
                        surfaceColor = Color.HSVToRGB(
                            rd.NextFloat(),
                            rd.NextFloat(0.05f, 0.3f),
                            rd.NextFloat(minV, 1f)),
                        inclination = rd.NextFloat(-5, 5),
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