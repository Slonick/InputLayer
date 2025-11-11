using System;
using InputLayer.Common.Models.Actions;
using Newtonsoft.Json.Serialization;

namespace InputLayer.Common.Serialization
{
    public class BackwardCompatibleSerializationBinder : DefaultSerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "InputLayer.Common.Models.Actions.ControllerAction":
                    return typeof(GameControllerAction);
                default:
                    return base.BindToType(assemblyName, typeName);
            }
        }
    }
}