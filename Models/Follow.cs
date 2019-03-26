using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    
public class Follow
{
    // No other fields!
    public int FollowId { get; set; }
    public int FollowerId { get; set;}
    public int FollowedId { get; set;}
    public User Follower { get; set; }
    public User Followed { get; set; }

}  