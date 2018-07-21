using ChallengeModel.Map;
using ChallengeModel.Player;
using System.Collections.Concurrent;

namespace Challenger.Model
{
    public class ChallengeState
    {
        public ConcurrentDictionary<string, Ship> Ships { get; set; } = new ConcurrentDictionary<string, Ship>();

        public ConcurrentDictionary<string, SolarSystem> SolarSystems { get; set; } = new ConcurrentDictionary<string, SolarSystem>();

        public ConcurrentDictionary<string, bool> ObservedSystems { get; set; } = new ConcurrentDictionary<string, bool>();

        public static ChallengeState GetStateFromConfiguration(ChallengeConfiguration config)
        {
            var state = new ChallengeState();
            foreach (var ss in config.SolarSystems) state.SolarSystems.AddOrUpdate(ss.Name, ss, (name, sys) => sys);
            foreach (var s in config.Ships) state.Ships.AddOrUpdate(s.Name, s, (name, ship) => ship);
            return state;
        }
    }
}
