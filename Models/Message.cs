using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class Message
{
    // No other fields!
    public int MessageId { get; set; }
    public string Contents { get; set; }
    public int BetId { get; set; }
    public int UserId { get; set;}
    public Bet Bet { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


} 