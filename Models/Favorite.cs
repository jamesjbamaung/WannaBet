using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class Favorite
{
    // No other fields!
    public int FavoriteId { get; set; }
    public int UserId { get; set;}
    public int BetId { get; set;}
    public User User { get; set; }
    public Bet Bet { get; set; }

} 