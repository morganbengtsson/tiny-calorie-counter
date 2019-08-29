using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Diet.Contexts;
using Diet.Models;

namespace Diet.Seed
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetService<MainContext>())
            {
                if (!context.MassUnits.Any()) 
                {
                    context.MassUnits.AddRange(new List<MassUnit> 
                    {
                        //new MassUnit("Milligram", "mg", 1000000),
                        //new MassUnit("Centigram", "cg", 100000),
                        //new MassUnit("Decigram", "dg", 10000),
                        new MassUnit{Name = "Gram", Symbol = "g", Factor = 1},
                        new MassUnit{Name = "Decagram", Symbol = "dag", Factor = 10},
                        new MassUnit{Name = "Hectogram", Symbol = "hg", Factor = 100},
                        new MassUnit{Name = "Kilogram", Symbol = "kg", Factor = 1000},
                        new MassUnit{Name = "Pound", Symbol = "lb", Factor = 453.59},
                        new MassUnit{Name = "Ounce", Symbol = "oz", Factor = 28.34},
                    });
                    context.SaveChanges();
                }

                if (!context.VolumeUnits.Any()) 
                {               
                    context.VolumeUnits.AddRange(new List<VolumeUnit> 
                    {
                        new VolumeUnit{Name = "Millitre", Symbol = "ml", Factor = 1},
                        new VolumeUnit{Name = "Centilitre", Symbol = "cl", Factor = 10},
                        new VolumeUnit{Name = "Decilitre", Symbol = "dl", Factor = 100},
                        new VolumeUnit{Name = "Litre", Symbol = "l", Factor = 1000},
                    });
                    context.SaveChanges();
                }

                if (!context.Foods.Any())
                {
                    var file = new FileInfo(@"Seed/8b. AUSNUT 2011-13 AHS Food Nutrient Database.xlsx");

                    var foods = new List<Food>();                    
                    
                    using (ExcelPackage xlPackage = new ExcelPackage(file))
                    {
                        var myWorksheet = xlPackage.Workbook.Worksheets.First();
                        var totalRows = myWorksheet.Dimension.End.Row;
                        var totalColumns = myWorksheet.Dimension.End.Column;

                        for (int rowNum = 2; rowNum <= totalRows; rowNum++)
                        {
                            var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns];
                            foods.Add(new Food
                            {
                                Code = row.ElementAt(0).GetValue<string>(),
                                Title = row.ElementAt(2).GetValue<string>(),
                                EnergyKJ = row.ElementAt(4).GetValue<int>(),
                                Water = row.ElementAt(6).GetValue<double>(),
                                Protein = row.ElementAt(7).GetValue<double>(),
                                TotalFat = row.ElementAt(8).GetValue<double>(),
                                Carbohydrates = row.ElementAt(9).GetValue<double>(),
                                Starch = row.ElementAt(11).GetValue<double>(),
                                TotalSugars = row.ElementAt(12).GetValue<double>(),
                                DietaryFibre = row.ElementAt(15).GetValue<double>(),
                                Alcohol = row.ElementAt(16).GetValue<double>(),
                                Ash = row.ElementAt(17).GetValue<double>(),
                                Retinol = row.ElementAt(18).GetValue<double>(),
                                BetaCarotene = row.ElementAt(19).GetValue<double>(),
                                ProvitaminA = row.ElementAt(20).GetValue<double>(),
                                VitaminA = row.ElementAt(21).GetValue<double>(),
                                Thiamin = row.ElementAt(22).GetValue<double>(),
                                Riboflavin = row.ElementAt(23).GetValue<double>(),
                                Niacin = row.ElementAt(24).GetValue<double>(),
                                NiacinEquivalents = row.ElementAt(25).GetValue<double>(),
                                TotalFolates = row.ElementAt(28).GetValue<double>(),
                                VitaminB6 = row.ElementAt(30).GetValue<double>(),
                                VitaminB12 = row.ElementAt(31).GetValue<double>(),
                                VitaminC = row.ElementAt(32).GetValue<double>(),
                                AlphaTocopherol = row.ElementAt(33).GetValue<double>(),
                                VitaminE = row.ElementAt(34).GetValue<double>(),
                                Calcium = row.ElementAt(35).GetValue<double>(),
                                Iodine = row.ElementAt(36).GetValue<double>(),
                                Iron = row.ElementAt(37).GetValue<double>(),
                                Magnesium = row.ElementAt(38).GetValue<double>(),
                                Phosphorus = row.ElementAt(39).GetValue<double>(),
                                Potassium = row.ElementAt(40).GetValue<double>(),
                                Selenium = row.ElementAt(41).GetValue<double>(),
                                Sodium = row.ElementAt(42).GetValue<double>(),
                                Zinc = row.ElementAt(43).GetValue<double>(),
                                Caffeine = row.ElementAt(44).GetValue<double>(),
                                Cholesterol = row.ElementAt(45).GetValue<double>(),
                                Tryptophan = row.ElementAt(46).GetValue<double>(),
                                TotalSaturatedFat = row.ElementAt(47).GetValue<double>(),
                                TotalMonounsaturatedFat = row.ElementAt(48).GetValue<double>(),
                                TotalPolyunsaturatedFat = row.ElementAt(49).GetValue<double>(),
                                LinoleicAcid = row.ElementAt(50).GetValue<double>(),
                                AlphaLinolenicAcid = row.ElementAt(51).GetValue<double>(),
                                TotalOmega3FattyAcids = row.ElementAt(55).GetValue<double>(),
                                TotalTransFattyAcids = row.ElementAt(56).GetValue<double>(),
                                Meassures = new List<Meassure>(),
                            });
                        }
                    }
                
                    var fileMeassure = new FileInfo(@"Seed/8e. AUSNUT 2011-13 AHS Food Measures File.xlsx");

                    using (ExcelPackage xlPackage = new ExcelPackage(fileMeassure))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.First();
                        var totalRows = worksheet.Dimension.End.Row;
                        var totalColumns = worksheet.Dimension.End.Column;

                        var start = worksheet.Dimension.Start;
                        var end = worksheet.Dimension.End;

                        //Only ten meassures for now, its slow.
                        for (int row = start.Row + 1; row <= end.Row; row++)
                        {
                            var code = worksheet.Cells[row, 1].GetValue<string>();
                            var name0 = worksheet.Cells[row, 7].GetValue<string>();
                            var name1 = worksheet.Cells[row, 8].GetValue<string>();
                            var name2 = worksheet.Cells[row, 9].GetValue<string>();
                            var name = name0 + " " + name1 + " " + name2;
                            name = name.Trim();

                            // Uppercase first letter
                            if (!string.IsNullOrEmpty(name))
                            {
                                name = char.ToUpper(name[0]) + name.Substring(1);
                            }

                            var weightGrams = worksheet.Cells[row, 11].GetValue<double>(); // Grams
                            var volumeMillilitres = worksheet.Cells[row, 12].GetValue<double>(); // Millilire

                            var food = foods.FirstOrDefault(f => f.Code == code);
                            if (volumeMillilitres > 0)
                            {
                                food.Dencity = weightGrams / volumeMillilitres;
                            }
                            else if (name0 == "density") 
                            {
                                food.Dencity = weightGrams;
                            }
                            else
                            {
                                food.Dencity = 0;
                                food.Meassures.Add(new Meassure{Name = name, Symbol = "", Factor = weightGrams});
                                Console.WriteLine(name);                                
                            }                            
                        }
                    }
                    context.Foods.AddRange(foods);
                    context.SaveChanges();                    
                }


                if (!context.Genders.Any()) {
                    var genders = new List<Gender>();
                    genders.Add(new Gender{Name = "Male", 
                    Factor0 = 10.0, Factor1 = 6.25, Factor2  = 5, Term = 5});
                    
                    genders.Add(new Gender{Name = "Female", 
                    Factor0 = 10.0, Factor1 = 6.25, Factor2  = 5, Term = -161});
                    context.Genders.AddRange(genders);
                    context.SaveChanges();
                }
            }
            return;
        }
    }
}