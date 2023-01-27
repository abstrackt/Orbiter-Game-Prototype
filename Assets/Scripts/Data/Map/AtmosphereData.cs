using System;

namespace Data.Map
{
    [Serializable]
    public struct AtmosphereData
    {
        public const float BREATHABLE = 800;
        public const float CO2_TOXICITY = 0.05f;
        public const float METHANE_TOXICITY = 0.075f;
        public const float SULPHUR_TOXICITY = 0.05f;
        public const float AMMONIA_TOXICITY = 0.01f;
        public const float OXYGEN_MIN = 0.15f;

        // TODO: Rewrite to something more correct
        public bool Breathable =>
            pressure > BREATHABLE &&
            oxygen > OXYGEN_MIN &&
            co2 < CO2_TOXICITY &&
            methane < METHANE_TOXICITY &&
            sulphur < SULPHUR_TOXICITY &&
            ammonia < AMMONIA_TOXICITY;

        // TODO: Rewrite to correct vapor capacity formula
        public float Humidity => oxygen == 0 ? 0f : waterVapor * 10f;

        // hcuj
        public bool CanHaveLiquids => temperature > -20 && temperature < 100 && pressure > 200;

        // gwuno
        public bool Inhabitable => temperature > -50 && temperature < 100;
        
        public float pressure;
        
        public const float ELEMENT_COUNT = 9; // CHANGE IF NECESSARY
        
        // Element abundances in percents
        public float oxygen;
        public float nitrogen;
        public float co2;
        public float waterVapor;
        public float methane;
        public float sulphur;
        public float ammonia;
        public float hydrogen;
        public float nobleGases;
        public float temperature;
    }
}