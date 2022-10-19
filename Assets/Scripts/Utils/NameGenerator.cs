using UnityEngine;

namespace Utils
{
    public static class NameGenerator
    {
        public static readonly string[] PLANET_FIRST_NAMES = new[]
        {
            "Gliese",
            "Sagan",
            "Cristoferetti",
            "Gagarin",
            "XY",
            "Gaia",
            "Alpha",
            "Terra",
            "Juno",
            "Altair",
            "Rutherford",
            "Canis",
            "Ursae",
            "Pegasi",
            "Epsilon",
            "Errai",
            "Fomalhaut",
            "HAT",
            "HD",
            "WASP",
            "Penrose",
            "Hawking",
            "Lem",
            "Giger",
            "Tycho",
            "Hubble",
            "Kepler",
            "Halley",
            "Herschel",
            "Huygens",
            "Leavitt",
            "Cassini"
        };

        public static readonly string[] PLANET_SECOND_NAMES = new[]
        {
            "431b",
            "a",
            "b",
            "c",
            "d",
            "Centauro",
            "Majoris",
            "Minoris",
            "Orbitar",
            "Gamma",
            "Tau",
            "2c",
            "3"
        };

        public static string GetRandomPlanetName()
        {
            var idx = Random.Range(0, PLANET_FIRST_NAMES.Length);

            var name = PLANET_FIRST_NAMES[idx];

            if (Random.value > 0.3f)
            {
                idx = Random.Range(0, PLANET_SECOND_NAMES.Length);
                name += " " + PLANET_SECOND_NAMES[idx];
            }

            return name;
        }
        
        public static readonly string[] STAR_FIRST_NAMES = new[]
        {
            "Proxima",
            "Alpha",
            "Betelgeuse",
            "VY",
            "Cephei",
            "VV",
            "Eridanus",
            "Gamma",
            "Taurus",
            "Lyra",
            "Corvus",
            "Draco",
            "Auriga",
            "Andromeda",
            "Canis",
            "Pisces",
            "Ursa",
            "Cygnus",
            "Perseus",
            "Aries",
            "Orion",
            "Gemini",
            "Cetus",
            "Fornax",
            "Cassiopea",
            "Pegasus",
            "Leo",
            "Corona"
        };
        
        public static readonly string[] STAR_SECOND_NAMES = new[]
        {
            "Centauri",
            "Major",
            "Minor",
            "Borealis",
            "Australis",
            "Austrinus",
            "Berenices",
            "Venatici",
            "Gamma"
        };
        
        public static string GetRandomStarName()
        {
            var idx = Random.Range(0, STAR_FIRST_NAMES.Length);

            var name = STAR_FIRST_NAMES[idx];

            if (Random.value > 0.75f)
            {
                idx = Random.Range(0, STAR_SECOND_NAMES.Length);
                name += " " + STAR_SECOND_NAMES[idx];
            }

            return name;
        }
    }
}