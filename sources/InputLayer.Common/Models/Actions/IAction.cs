namespace InputLayer.Common.Models.Actions
{
    public interface IAction
    {
        void Execute(object obj = null);
    }

    public interface IActionWithParams : IAction
    {
        bool HasOptionalSettings { get; }

        bool IsOpenOptionalSettings { get; set; }
    }

    public interface IExecutableAction : IAction
    {
        bool IsHidden { get; set; }
    }

    public interface IExecutableActionWithParams : IExecutableAction, IActionWithParams { }
}