namespace Challenger.Model
{
    using Map;
    using Newtonsoft.Json;
    using Player;
    using System.Collections.Generic;
    using System.IO;

    public class ChallengeConfiguration
    {
        public List<System> Systems { get; set; }

        public List<Ship> Ships { get; set; }

        public static ChallengeConfiguration GetChallengeConfiguration(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ChallengeConfiguration>(json);
        }
    }
}
