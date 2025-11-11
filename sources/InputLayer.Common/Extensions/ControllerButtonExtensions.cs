using System.Collections.Generic;
using System.Linq;
using InputLayer.Common.Infrastructures;

namespace InputLayer.Common.Extensions
{
    public static class ControllerButtonExtensions
    {
        public static bool ContainsButtons(this IEnumerable<ControllerInput> combination, params ControllerInput[] buttons)
            => combination.Any(buttons.Contains);

        public static IEnumerable<ControllerInput> ExceptButtons(this IEnumerable<ControllerInput> combination, params ControllerInput[] buttons)
            => combination.Where(button => !buttons.Contains(button));
    }
}