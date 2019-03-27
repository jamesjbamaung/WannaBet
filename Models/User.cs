using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class User
{
    [Key]
    public int UserId {get;set;}
    [Required]
    public string FirstName {get;set;}
    [Required]
    public string LastName {get;set;}
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string Password {get;set;}
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
    public string ProfilePic { get; set; }
    [InverseProperty("Better")]
    public List<Reserve> BetterBets { get; set; }
    [InverseProperty("Taker")]
    public List<Reserve> TakerTakes { get; set; }
    [InverseProperty("Follower")]
    public List<Follow> listOfFollowers { get; set; }
    [InverseProperty("Followed")]
    public List<Follow> listOfFollows { get; set; }
    public List<Message> listOfMessages { get; set; }
    public List<Favorite> listOfFavorites { get; set; }
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // Will not be mapped to your users table!

}