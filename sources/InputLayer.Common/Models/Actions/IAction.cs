namespace InputLayer.Common.Models.Actions
{
    public interface IAction
    {
        void Execute(object obj = null);
    }

    public interface IExecutableAction : IAction
    {
        bool IsHidden { get; set; }
    }
}