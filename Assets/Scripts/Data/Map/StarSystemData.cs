using System;
using System.Collections.Generic;

namespace Data.Map
{
    [Serializable]
    public struct StarSystemData
    {
        public StarData star;
        public List<PlanetData> planets;
    }
}