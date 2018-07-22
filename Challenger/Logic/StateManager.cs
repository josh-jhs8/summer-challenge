using ChallengeModel;
using ChallengeModel.PlayerAction;
using Challenger.Model;
using Newtonsoft.Json;
using System;

namespace Challenger.Logic
{
    public class StateManager
    {
        private ChallengeState _state;
        private readonly string _playerName;

        public StateManager(ChallengeState state, string playerName)
        {
            _state = state;
            _playerName = playerName;
        }

        public CommandResultDto ProcessCommand(Command command)
        {
            try
            {
                if (command.Action == "Poll") return GetCurrentState();
                else throw new Exception("Invalid action for command type state.");
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

        private CommandResultDto GetCurrentState()
        {
            var state = new State();
            state.Players.Add(_state.Players[_playerName].GetEmpire());
            foreach (var observation in _state.ObservedSystems[_playerName])
                state.SolarSystems.Add(_state.SolarSystems[observation]);

            return new CommandResultDto
            {
                Success = true,
                Message = "Got current state",
                ResultObjectJson = JsonConvert.SerializeObject(state),
                ResultObjectType = typeof(State)
            };
        }
    }
}
