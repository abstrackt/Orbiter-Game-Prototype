namespace Data.Map
{
    public enum StarType
    {
        // "Normal" 1-10
        ProtoStar = 0,
        MainSequence = 1,
        GiantBranch = 2,
        SuperBranch = 3,
        WolfRayetStar = 4,
        CarbonStar = 5,

        // "Normal" stellar remnants 11-20
        WhiteDwarf = 11,
        
        // Neutron stars 21-30
        NeutronStar = 21,
        Magnetar = 22,
        Pulsar = 23,
        
        // Crazy (relativistic) stellar remnants 31-40
        // (neutron stars are also semi-relativistic, but I mean VERY relativistic)
        QuarkStar = 31,
        BlackHole = 32,
        
        // Anomalies 41+
        IronStar = 41,
        TZO = 42
    }
}