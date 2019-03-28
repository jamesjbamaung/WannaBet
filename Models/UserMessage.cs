using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class UserMessage
{
    // No other fields!
    public int UserMessageId { get; set; }
    public string Info { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set;}
    public bool SenderDelete { get; set; }
    public bool ReceiverDelete { get; set; }
    public User Sender { get; set; }
    public User Receiver { get; set; }
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


} 