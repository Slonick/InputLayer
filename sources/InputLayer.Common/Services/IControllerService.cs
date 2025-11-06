using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Common.Services
{
    public interface IControllerService : IDisposable
    {
        event Action<IReadOnlyList<ControllerInput>> ButtonCombinationPressed;
        event Action<IReadOnlyList<ControllerInput>> ButtonCombinationReleased;
        event Action<ControllerInput> ButtonPressed;
        event Action<ControllerInput> ButtonReleased;

        void Initialize();
        Task InitializeAsync();

        void Rumble(int durationMs, float intensity);
    }
}