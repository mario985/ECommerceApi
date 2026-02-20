using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

public class Inventory
{
    public int Id{ set; get; }
    public string ProductId { set; get; } = string.Empty;
    public int OnHand{set ; get ;}
    public int Reserved {set ; get ;}
    [NotMapped]
    public int Availaible => OnHand - Reserved;
}