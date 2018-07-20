using System.Collections.Generic;

namespace ChallengeModel.Map
{
    public class SolarSystem
    {
        public string Name { get; set; }

        public List<Star> Stars { get; set; } = new List<Star>();

        public List<Planet> Planets { get; set; } = new List<Planet>();

        public List<string> Hyperlanes { get; set; } = new List<string>();

        public Location Location { get; set; } = new Location();
    }
}
