using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EntryGroupView {
    public ICollection<Entry> Entries{get; set;}
    public int GetEnergySum(){
        return Entries.Sum(e => e.EnergyKcal);
    }
}