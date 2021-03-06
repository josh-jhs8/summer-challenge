﻿using ChallengeModel.Map;
using ChallengeModel.Player;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Challenger.Model
{
    public class ChallengeConfiguration
    {
        public List<SolarSystem> SolarSystems { get; set; } = new List<SolarSystem>();

        public List<Empire> Players { get; set; } = new List<Empire>();

        public static ChallengeConfiguration GetChallengeConfiguration(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ChallengeConfiguration>(json);
        }
    }
}
