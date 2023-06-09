﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.SettingModels;

public class SettingViewModel
{
    public byte? Id { get; }
    
    [DisplayName("Description")]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(25, ErrorMessage = "Description must be between 2 and 25 characters", MinimumLength = 2 )]
    [RegularExpression(@"^[a-zA-Z0-9 ,&]+$", ErrorMessage = "Description can only contain letters, spaces, commas, and ampersands.")]
    public string Description { get; set; }
    
    public IEnumerable<SettingModel> SettingModels { get; set; } = new List<SettingModel>();
    
    public SettingViewModel(IEnumerable<SettingModel> settingModels) => SettingModels = settingModels;

    public SettingViewModel() { }
}