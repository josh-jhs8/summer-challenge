using Challenger.Model;
using Challenger.Model.Map;
using Challenger.Model.Player;
using Challenger.Model.PlayerAction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenger.Logic
{
    public class ShipManager
    {
        private ChallengeConfiguration _config;

        public ShipManager(ChallengeConfiguration config)
        {
            _config = config;
        }

        public CommandResultDto ProcessCommand(Command command)
        {
            try
            {
                if (command.Action == "List") return List();

                if (command.Type != "Ship") throw new Exception("This is not a valid command for a ship.");
                if (!_config.Ships.Any(s => s.Name == command.Subject)) throw new Exception("This ship does not exist.");

                switch (command.Action)
                {
                    case "Move": return Move(command.Subject, command.Arguments);
                    case "Observe": return Observe(command.Subject);
                    default: throw new Exception("This is not a valid action for a ship.");
                }
            }
            catch (Exception e)
            {
                return new CommandResultDto
                {
                    Success = false,
                    Message = e.Message,
                    ResultObjectJson = null,
                    ResultObjectType = typeof(object)
                };
            }
        }

        private CommandResultDto Move(string shipName, List<string> args)
        {
            if (args.Count < 1) throw new Exception("A destination must be provided for a move command.");

            var ship = _config.Ships.First(s => s.Name == shipName);
            var currentSystem = _config.SolarSystems.First(ss => ss.Name == ship.Location);
            if (!currentSystem.Hyperlanes.Contains(args[0])) throw new Exception($"{args[0]} is not a valid destination for a move command.");

            ship.Location = args[0];
            return new CommandResultDto
            {
                Success = true,
                Message = $"Sucessfully moved {ship.Name} to {ship.Location}.",
                ResultObjectJson = JsonConvert.SerializeObject(ship),
                ResultObjectType = typeof(Ship)
            };
        }

        private CommandResultDto Observe(string shipName)
        {
            var ship = _config.Ships.First(s => s.Name == shipName);
            var system = _config.SolarSystems.First(ss => ss.Name == ship.Location);

            return new CommandResultDto
            {
                Success = true,
                Message = $"Sucessfully observed {system.Name}",
                ResultObjectJson = JsonConvert.SerializeObject(system),
                ResultObjectType = typeof(SolarSystem)
            };
        }

        private CommandResultDto List()
        {
            return new CommandResultDto
            {
                Success = true,
                Message = "Listing all current ships",
                ResultObjectJson = JsonConvert.SerializeObject(_config.Ships),
                ResultObjectType = typeof(List<Ship>)
            };
        }
    }
}
