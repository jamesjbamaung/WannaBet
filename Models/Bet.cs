using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class Bet
{
    // No other fields!
    public int BetId {get; set;}
    public string Title { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }
    public DateTime ClosingDate { get; set; }
    public int UserId { get; set;}
    public virtual User User { get; set; }
    public List<Reserve> listOfParticipants { get; set; }
    public List<Message> listOfMessages { get; set; }
    public List<Favorite> listOfFavorites { get; set; }
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


}  