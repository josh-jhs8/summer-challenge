using ChallengeModel.Player;
using System.Collections.Concurrent;
using System.Linq;

namespace Challenger.Model
{
    public class ChallengeEmpire
    {
        public string Name { get; }
        public ConcurrentDictionary<string, Ship> Ships { get; set; } = new ConcurrentDictionary<string, Ship>();

        public ChallengeEmpire(Empire empire)
        {
            Name = empire.Name;
            foreach (var ship in empire.Ships) Ships.AddOrUpdate(ship.Name, ship, (n, s) => ship);
        }

        public Empire GetEmpire()
        {
            var empire = new Empire();
            empire.Name = Name;
            foreach (var ship in Ships.Select(s => s.Value)) empire.Ships.Add(ship);
            return empire;
        }
    }
}
