using System.ComponentModel.DataAnnotations;

public record AdjustSTockDto([Range(0,100000)] int delta);