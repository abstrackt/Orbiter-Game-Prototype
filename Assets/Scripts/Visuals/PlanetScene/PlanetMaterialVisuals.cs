using Data.Map;
using Systems.Global;
using UnityEngine;

namespace Visuals.PlanetScene
{
    public class PlanetMaterialVisuals : SingletonMonoBehaviour<PlanetMaterialVisuals>
    {
        public Material planetMaterial;
        public Material atmosphereMaterial;
        public Material ringsMaterial;

        public MeshRenderer planet;
        public MeshRenderer atmosphere;
        public MeshRenderer rings;

        private SpaceshipManager _spaceship;

        public void Start()
        {
            planet.material = Instantiate(planetMaterial);
            atmosphere.material = Instantiate(atmosphereMaterial);
            rings.material = Instantiate(ringsMaterial);

            _spaceship = SpaceshipManager.Instance;

            if (_spaceship != null && _spaceship.OrbitingPlanet.HasValue)
            {
                GenerateVisuals(_spaceship.OrbitingPlanet.Value);
            } 
        }

        public void GenerateVisuals(PlanetData data)
        {
            planet.gameObject.transform.localScale = Vector3.one * 10f * data.radius;
            
            // Surface colors

            var col1 = data.surfaceColor;
            Color.RGBToHSV(col1, out var h, out var s, out var v);
            var col2 = Color.HSVToRGB(h, s, 0.85f * v);
            var seaCol = GetSeaColor(data.seaType, col1);
            
            planet.material.SetColor("_LandColor1", col1);
            planet.material.SetColor("_LandColor2", col2);
            planet.material.SetColor("_SeaColor", seaCol);

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
            
            this.planet.material.SetColor("_CloudColor", cloudCol);
            
            // Ice caps
            
            var iceCol = new Color(0.85f, 0.9f, 1f);
            
            if (data.seaLevel > 0.5f)
            {
                iceCol = Color.Lerp(seaCol, iceCol, 0.9f);
            }
            
            var mult = -(data.atmoData.temperature - 30f) / 100f;
            var iceSize = data.atmoData.temperature < 30f && !gas ? Mathf.Clamp01(data.atmoData.Humidity + 0.4f * mult) : 0f;
            
            this.planet.material.SetColor("_IceColor", iceCol);
            this.planet.material.SetFloat("_IceCapSize", iceSize);
            
            // Craters

            var craterAmount = 0f;

            if (data.atmoData.pressure < AtmosphereData.MAX_PRESSURE * 0.1f)
            {
                craterAmount = Random.Range(0.01f, 0.05f);
            }
            
            this.planet.material.SetFloat("_CraterSize", craterAmount);
        }

        private Color GetSeaColor(SeaType type, Color surface)
        {
            var hMult = Random.Range(0.9f, 1.1f);
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
                    rgb = new Color(0.55f, 0.65f, 0.2f);
                    break;
                case SeaType.Lava:
                    rgb = new Color(1f, 0.6f, 0.2f);
                    break;
                case SeaType.Gas:
                    return surface;
            }
            
            Color.RGBToHSV(rgb, out var h, out var s, out var v);
            hMult = Random.Range(0.9f, 1.1f);
            vMult = Random.Range(0.9f, 1.1f);
            return Color.HSVToRGB(hMult * h, s, vMult * v);
        }
    }
}