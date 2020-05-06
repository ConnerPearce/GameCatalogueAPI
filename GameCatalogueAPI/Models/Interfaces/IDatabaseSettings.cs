namespace GameCatalogueAPI.Models
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string GameCollectionName { get; set; }
        string PlayedCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string WishlistCollectionName { get; set; }
    }
}