using ChallengeModel.Map;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Challenger.Model
{
    public class ChallengeState
    {
        public ConcurrentDictionary<string, ChallengeEmpire> Players { get; set; } = new ConcurrentDictionary<string, ChallengeEmpire>();

        public ConcurrentDictionary<string, SolarSystem> SolarSystems { get; set; } = new ConcurrentDictionary<string, SolarSystem>();

        public ConcurrentDictionary<string, List<string>> ObservedSystems { get; set; } = new ConcurrentDictionary<string, List<string>>();

        public ConcurrentDictionary<string, bool> Flags { get; set; } = new ConcurrentDictionary<string, bool>();

        public ChallengeState()
        {
            Flags.AddOrUpdate("Ready", false, (x, y) => false);
        }

        public List<string> GetPlayerNames()
        {
            return Players.Select(p => p.Key).ToList();
        }

        public static ChallengeState GetStateFromConfiguration(ChallengeConfiguration config)
        {
            var state = new ChallengeState();
            foreach (var ss in config.SolarSystems) state.SolarSystems.AddOrUpdate(ss.Name, ss, (name, sys) => sys);
            foreach (var p in config.Players)
            {
                state.Players.AddOrUpdate(p.Name, new ChallengeEmpire(p), (name, player) => player);
                state.ObservedSystems.AddOrUpdate(p.Name, new List<string>(), (n, l) => new List<string>());
            }
            return state;
        }

    }
}
