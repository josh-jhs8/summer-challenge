using ChallengeModel.Map;
using ChallengeModel.Player;
using System.Collections.Generic;

namespace ChallengeModel
{
    public class State
    {
        public List<SolarSystem> SolarSystems { get; set; } = new List<SolarSystem>();

        public List<Ship> Ships { get; set; } = new List<Ship>();
    }
}
