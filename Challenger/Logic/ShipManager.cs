using ChallengeModel.Map;
using ChallengeModel.Player;
using ChallengeModel.PlayerAction;
using Challenger.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Challenger.Logic
{
    public class ShipManager
    {
        private ChallengeState _state;
        private readonly string _playerName;

        public ShipManager(ChallengeState state, string playerName)
        {
            _state = state;
            _playerName = playerName;
            if (!_state.Players.ContainsKey(_playerName)) throw new Exception("This has gone really wrong!");
        }

        public CommandResultDto ProcessCommand(Command command)
        {
            try
            {
                if (command.Action == "List") return List();

                if (command.Type != "Ship") throw new Exception("This is not a valid command for a ship.");
                if (!_state.Players[_playerName].Ships.ContainsKey(command.Subject)) throw new Exception("This ship does not exist.");

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
                    ResultObjectJson = "",
                    ResultObjectType = typeof(object)
                };
            }
        }

        private CommandResultDto Move(string shipName, List<string> args)
        {
            if (args.Count < 1) throw new Exception("A destination must be provided for a move command.");

            var ship = _state.Players[_playerName].Ships[shipName];
            if (ship.Status != "Awaiting Command") throw new Exception("Cannot move ship until it has finished current action.");

            var currentSystem = _state.SolarSystems[ship.Location];
            if (!currentSystem.Hyperlanes.Contains(args[0])) throw new Exception($"{args[0]} is not a valid destination for a move command.");

            ship.Status = $"Moving to {args[0]}";
            _state.Players[_playerName].Ships.AddOrUpdate(ship.Name, ship, (n, s) => ship);

            var completeMove = new Task(() =>
            {
                Thread.Sleep(500);
                ship.Status = "Awaiting Command";
                ship.Location = args[0];
                _state.Players[_playerName].Ships.AddOrUpdate(ship.Name, ship, (n, s) => ship);
            });

            completeMove.Start();
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
            var ship = _state.Players[_playerName].Ships[shipName];
            if (ship.Status != "Awaiting Command") throw new Exception("Cannot accept observe command until previous action is finished.");

            var system = _state.SolarSystems[ship.Location];
            _state.ObservedSystems.AddOrUpdate(system.Name, new List<string>() { system.Name }, (n, l) => 
            {
                l.Add(n);
                return l;
            });
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
            var ships = new List<Ship>();
            foreach (var shipEntry in _state.Players[_playerName].Ships) ships.Add(shipEntry.Value);
            return new CommandResultDto
            {
                Success = true,
                Message = "Listing all current ships",
                ResultObjectJson = JsonConvert.SerializeObject(ships),
                ResultObjectType = typeof(List<Ship>)
            };
        }
    }
}
