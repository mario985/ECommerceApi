public class ProdcutFilterDto
{
    public string Name { set; get; } = string.Empty;
    public ProductCategory? Category { set; get; } = null;
    public decimal? MinPrice { set; get; }
    public decimal? MaxPrice { set; get; }
    public SortField? SortBy { set; get; }
    public bool SortDec { set; get; }
    public string Brand { set; get; } = string.Empty;
    public int Page { set; get; } = 1;
    public int Limit { set; get; } = 20;

}
public enum SortField
{
    name,
    category,
    price,
    brand
}