using Data.Map;
using Systems.Global;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visuals.PlanetScene
{
    public class PlanetSceneVisuals : SingletonMonoBehaviour<PlanetSceneVisuals>
    {
        public Transform starEcliptic;
        public Transform starAxis;
        public SpriteRenderer starSprite;
        public UnityEngine.Rendering.Universal.Light2D starLight;
        
        public Material planetMaterial;
        public Material atmosphereMaterial;
        public Material ringsMaterial;

        public MeshRenderer planet;
        public MeshRenderer atmosphere;
        public MeshRenderer rings;

        private SpaceshipManager _spaceship;
        private LocationManager _location;

        private Vector3 _axis;

        public void Start()
        {
            planet.material = Instantiate(planetMaterial);
            atmosphere.material = Instantiate(atmosphereMaterial);
            rings.material = Instantiate(ringsMaterial);

            _spaceship = SpaceshipManager.Instance;
            _location = LocationManager.Instance;

            if (_spaceship != null && _location.OrbitingPlanet.HasValue)
            {
                GeneratePlanetVisuals(_location.OrbitingPlanet.Value);
            }
            
            GenerateStarVisuals(_location.OrbitingStar);
        }

        public void FixedUpdate()
        {
            starAxis.Rotate(0, -0.004f, 0);

            var lightDir = starSprite.transform.position;
            
            planet.material.SetVector("_LightDir", lightDir);
            atmosphere.material.SetVector("_LightDir", lightDir);
        }

        public void GenerateStarVisuals(StarData? data)
        {
            if (data.HasValue)
            {
                var star = data.Value;
                
                starSprite.gameObject.SetActive(true);
                starAxis.Rotate(0, -_location.StarPhase * Mathf.Rad2Deg, 0);

                var starTemp = star.temperature;
                var starColor = new Color(
                    Mathf.Clamp01((250 - starTemp * 4f) / 255f + Mathf.Max(starTemp - 80f, 0)),
                    Mathf.Clamp01((90 - starTemp * .4f) / 255f + Mathf.Max(starTemp - 80f, 0)),
                    Mathf.Clamp01((20 + starTemp * 2f) / 255f + Mathf.Max(starTemp - 80f, 0)));
                var starSize = star.radius * 2;

                starSprite.color = starColor;
                starLight.color = starColor;
                starSprite.transform.localScale = new Vector3(starSize, starSize, 1) / (_location.StarDistance * 0.05f);
                starLight.intensity = starSize * 0.75f / (_location.StarDistance * 0.01f);
                starLight.pointLightOuterRadius *= Mathf.Sqrt(starSize) / (_location.StarDistance * 0.1f);
            }
            else
            {
                starSprite.gameObject.SetActive(false);
            }
        }

        public void GeneratePlanetVisuals(PlanetData data)
        {
            planet.gameObject.transform.localScale = Vector3.one * 15f * data.radius;
            
            starEcliptic.Rotate(data.inclination, 0, 0);

            // Surface colors

            var iceCol = new Color(0.85f, 0.9f, 1f);
            var col1 = data.surfaceColor;
            Color.RGBToHSV(col1, out var h, out var s, out var v);
            var col2 = Color.HSVToRGB(h, s, 0.85f * v);
            var seaCol = GetSeaColor(data.seaType, col1);
            
            planet.material.SetColor("_LandColor1", col1);
            planet.material.SetColor("_LandColor2", col2);

            var gas = data.planetType is PlanetType.Giant or PlanetType.Subgiant;

            // Sea level
            
            planet.material.SetFloat("_SeaLevel", gas ? 1 : data.seaLevel);
            
            // Population

            planet.material.SetFloat("_Population", data.population);
            
            // Rings

            rings.gameObject.SetActive(data.hasRings);
            
            if (data.hasRings)
            {
                rings.material.SetFloat("_InnerRadius", data.ringData.innerRadius);
                rings.material.SetFloat("_OuterRadius", data.ringData.outerRadius);
                rings.material.SetFloat("_Opacity", data.ringData.opacity);
            }
           
            // Atmosphere

            var t = Mathf.Lerp(1, 0, 2 * data.atmoData.pressure / AtmosphereData.MAX_PRESSURE);

            atmosphere.material.SetFloat("_Transparency", Mathf.Lerp(0.7f, 2f, t));

            var atmoCol = data.seaLevel > 0.5f ? seaCol : col1;
            Color.RGBToHSV(atmoCol, out h, out s, out v);
            atmoCol = Color.HSVToRGB(h, s * 0.8f, 1f);

            atmosphere.material.SetColor("_Color", atmoCol);
            
            // Clouds

            var cloudCol = Color.white * 0.95f;

            if (data.seaLevel > 0.5f)
            {
                cloudCol = Color.Lerp(seaCol, cloudCol, 0.9f);
            }
            
            planet.material.SetColor("_CloudColor", cloudCol);

            if (!data.atmoData.CanHaveClouds)
            {
                planet.material.SetFloat("_CloudOpacity", 0f);
            }
            
            // Ice caps

            if (data.seaLevel > 0.5f)
            {
                iceCol = Color.Lerp(seaCol, iceCol, 0.9f);
            }
            
            var mult = -(data.atmoData.temperature - 30f) / 100f;
            var iceSize = data.atmoData.temperature < 30f && !gas ? Mathf.Clamp01(data.atmoData.Humidity + 0.5f * mult) * 0.8f : 0f;
            
            planet.material.SetColor("_IceColor", iceCol);
            planet.material.SetFloat("_IceCapSize", iceSize);
            
            // Ocean color

            if (data.atmoData.temperature < 0f)
            {
                seaCol = iceCol;
            }
            
            planet.material.SetColor("_SeaColor", seaCol);
            
            // Craters

            var craterAmount = 0f;

            if (data.atmoData.pressure < AtmosphereData.MAX_PRESSURE * 0.2f)
            {
                craterAmount = Random.Range(0.01f, 0.05f);
            }
            
            planet.material.SetFloat("_CraterSize", craterAmount);
        }

        private Color GetSeaColor(SeaType type, Color surface)
        {
            var sMult = Random.Range(0.9f, 1.1f);
            var vMult = Random.Range(0.9f, 1.1f);
            var rgb = Color.magenta;
            
            switch (type)
            {
                case SeaType.Water:
                    rgb = new Color(0.2f, 0.5f, 0.8f);
                    break;
                case SeaType.Carbohydrate:
                    rgb = new Color(0.7f, 0.6f, 0.3f);
                    break;
                case SeaType.Sulfur:
                    rgb = new Color(0.7f, 0.65f, 0.5f);
                    break;
                case SeaType.Lava:
                    rgb = new Color(1f, 0.6f, 0.2f);
                    break;
                case SeaType.Gas:
                    return surface;
            }
            
            Color.RGBToHSV(rgb, out var h, out var s, out var v);
            return Color.HSVToRGB( h, sMult * s, vMult * v);
        }
    }
}