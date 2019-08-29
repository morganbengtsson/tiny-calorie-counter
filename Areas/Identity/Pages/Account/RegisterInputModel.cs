using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

public class RegisterInputModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
  
    [Required]
    [Display(Name = "Gender")]
    public int GenderId {get; set;}

    [Required]
    [Display(Name = "Birth date")]
    [DataType(DataType.Date)]
    public DateTime  BirthDate {get; set;}

    [Required]
    [Display(Name = "Height (cm)")]
    [Range(0, 300, ErrorMessage= "Not a valid height.")]
    public double Height {get; set;}

    [Required]
    [Display(Name = "Weight (kg)")]
    [Range(0,700, ErrorMessage = "Not a valid weight")]    
    public double Weight {get; set;}

    [Required]
    [Display(Name = "Physical activity level")]
    [Range(1, 5, ErrorMessage = "Not an activity level")]    
    public decimal PhysicalActivityLevel {get; set;}

    [Required]
    [Display(Name = "Weight goal per week")]
    [Range(-1.0, 1.0, ErrorMessage = "Out of range")]    
    public decimal Goal {get; set;}
            
}