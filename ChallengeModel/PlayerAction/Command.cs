using System.Collections.Generic;

namespace ChallengeModel.PlayerAction
{
    public class Command
    {
        public string Type { get; set; }

        public string Subject { get; set; }

        public string Action { get; set; }

        public List<string> Arguments { get; set; }
    }
}
