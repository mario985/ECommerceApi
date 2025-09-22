using System.ComponentModel.DataAnnotations;

public class RemoveFromCartDto
{
    [Required]
    public string UserId { set; get; }
    [Required]
    public string ProductId { set; get; }
}