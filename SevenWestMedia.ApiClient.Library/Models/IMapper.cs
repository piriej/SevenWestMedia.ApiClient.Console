namespace SevenWestMedia.ApiClient.Library.Models
{
    public interface IMapper<in T, TV>
        where T : IModel
    {
        TV Map( TV viewModel, T person);
    }
}