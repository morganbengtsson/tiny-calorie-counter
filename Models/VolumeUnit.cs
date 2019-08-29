using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Diet.Models
{
    public class VolumeUnit : Unit
    {
        public override double GetDencityFactor(double dencity) {
            return dencity;
        }
    }
}