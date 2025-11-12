namespace InputLayer.Settings.Mappers
{
    public interface IViewModelMapper<TData, TViewModel>
        where TData : class
        where TViewModel : class
    {
        TData FromViewModel(TViewModel viewModel);

        TViewModel ToViewModel(TData data);
    }
}