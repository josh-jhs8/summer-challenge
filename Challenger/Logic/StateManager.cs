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

        public StateManager(ChallengeState state)
        {
            _state = state;
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
            foreach (var shipEntry in _state.Ships)
                state.Ships.Add(shipEntry.Value);
            foreach (var obsEntry in _state.ObservedSystems)
                state.SolarSystems.Add(_state.SolarSystems[obsEntry.Key]);

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
