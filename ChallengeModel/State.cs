using ChallengeModel.Map;
using ChallengeModel.Player;
using System.Collections.Generic;

namespace ChallengeModel
{
    public class State
    {
        public List<SolarSystem> SolarSystems { get; set; } = new List<SolarSystem>();

        public List<Empire> Players { get; set; } = new List<Empire>();
    }
}
