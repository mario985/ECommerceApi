public enum StockTransition
{
    Reserve,   // Reserved += qty   (requires Available >= qty)
    Commit,    // OnHand -= qty, Reserved -= qty (requires Reserved >= qty)
    Release    // Reserved -= qty   (requires Reserved >= qty)
}
