using System.Collections.Generic;

namespace ChallengeModel.Player
{
    public class Empire
    {
        public string Name { get; set; }

        public List<Ship> Ships { get; set; } = new List<Ship>();
    }
}
