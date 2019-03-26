using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class Reserve
{
    // No other fields!
    public int ReserveId { get; set; }
    public int BetId { get; set; }
    public int BetterId { get; set;}
    public int TakerId { get; set;}
    public Bet Bet { get; set; }
    public User Better { get; set; }
    public User Taker { get; set; }
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


}  