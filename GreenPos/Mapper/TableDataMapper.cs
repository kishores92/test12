using GreenPOS.Entity;
using GreenPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Mapper
{
    public static class TableDataMapper
    {
        public static List<TableData> ToTableData(this List<InclusionDetailViewModel> inclusion, long basePrice, long facadeCost, List<InclusionDetailViewModel> additionalInclusions)
        {
            var data = new List<TableData>();
            var categoryCss = "header-text-bold";
            var subTotalCss = "header-text-bold-right ";
            var categories = inclusion.OrderBy(y => y.SequenceNumber).Select(x => x.Category).Distinct();
            var categoryNum = 1;

            foreach (var category in categories)
            {
                var categoryLineNum = 1;
                data.Add(new TableData { RowNumber = $"{categoryNum}.00", Description = category, Price = "", CssClass = categoryCss });

                var subCategories = inclusion.Where(z => z.Category == category).Select(x => x.SubCategory).Distinct();
                foreach (var subCategory in subCategories)
                {
                    data.Add(new TableData { RowNumber = "", Description = subCategory, Price = "", CssClass = categoryCss });

                    var lineItems = inclusion.Where(z => z.Category == category && z.SubCategory == subCategory).ToList();
                    foreach (var line in lineItems)
                    {
                        var numbering = categoryLineNum < 10 ? $"{categoryNum}.0{categoryLineNum}" : $"{categoryNum}.{categoryLineNum}";
                        var lineDisplayPrice = "";
                        if (line.Cost > 0)
                            lineDisplayPrice = line.Cost.ToString("C0");
                        else if (line.IsIncluded)
                            lineDisplayPrice = "Included";
                        else if (line.IsNoteOnly)
                            lineDisplayPrice = "Note Only";

                        data.Add(new TableData { RowNumber = numbering, Description = line.Description, Price = lineDisplayPrice });
                        data.Add(new TableData());
                        categoryLineNum++;
                    }
                }
                var categoryTotal = inclusion.Where(z => z.Category == category).Sum(s => s.Cost);
                var displayPrice = categoryTotal > 0 ? categoryTotal.ToString("C0") : "-";

               // data.Add(new TableData());
                data.Add(new TableData { RowNumber = "", Description = "Sub Total", Price = displayPrice, CssClass = categoryCss, SubTotalCss = subTotalCss });
                data.Add(new TableData());

                categoryNum++;

            }
            // Add Ons
            if (additionalInclusions != null && additionalInclusions.Count > 0)
            {

                var addons = additionalInclusions.Select(x => x.Category).Distinct();
                foreach (var addOn in addons)
                {
                    var categoryLineNum = 1;
                    data.Add(new TableData { RowNumber = $"{categoryNum}.00", Description = addOn, Price = "", CssClass = categoryCss });
                    var subCategories = additionalInclusions.Where(z => z.Category == addOn).Select(x => x.SubCategory).Distinct();

                    foreach (var subCategory in subCategories)
                    {

                        if (subCategory.ToLower().Trim() != "select")
                        {
                            data.Add(new TableData { RowNumber = "", Description = subCategory, Price = "", CssClass = categoryCss });
                        }
                        var lineItems = additionalInclusions.Where(z => z.Category == addOn && z.SubCategory == subCategory).ToList();
                        foreach (var line in lineItems)
                        {
                            var numbering = categoryLineNum < 10 ? $"{categoryNum}.0{categoryLineNum}" : $"{categoryNum}.{categoryLineNum}";
                            var lineDisplayPrice = "";
                            if (line.Cost > 0)
                                lineDisplayPrice = line.Cost.ToString("C0");
                            else if (line.IsIncluded)
                                lineDisplayPrice = "Included";
                            else if (line.IsNoteOnly)
                                lineDisplayPrice = "Note Only";

                            //if (line.Description.Contains("-"))
                            //{
                            //    var rows = line.Description.Split("-");
                            //    for(int i=0;i<rows.Length;i++)
                            //    {
                            //        var row = rows[i];
                            //        if (i > 0)
                            //            row = "-" + row;
                            //        var rowPrice = i == 0 ? lineDisplayPrice : "";
                            //        var rowNumber = i == 0 ? numbering : "";
                            //        data.Add(new TableData { RowNumber = rowNumber, Description = row, Price = rowPrice });
                            //    }
                            //}
                            //else
                            {
                                data.Add(new TableData { RowNumber = numbering, Description = line.Description, Price = lineDisplayPrice });
                            }
                            data.Add(new TableData());
                            categoryLineNum++;
                        }
                    }
                    var categoryTotal = additionalInclusions.Where(z => z.Category == addOn).Sum(s => s.Cost);
                    var displayPrice = categoryTotal > 0 ? categoryTotal.ToString("C0") : "-";

                    data.Add(new TableData());
                    data.Add(new TableData { RowNumber = "", Description = "Sub Total", Price = displayPrice, CssClass = categoryCss, SubTotalCss = subTotalCss });
                    data.Add(new TableData());
                    categoryNum++;
                }

            }



            var total = basePrice + facadeCost + inclusion.Sum(s => s.Cost) + (additionalInclusions ?? new List<InclusionDetailViewModel>()).Sum(y => y.Cost);

            //data.Add(new TableData());
            //data.Add(new TableData());
            data.Add(new TableData { RowNumber = "", Description = "Grand Total", Price = total.ToString("C0"), CssClass = categoryCss, SubTotalCss = subTotalCss });
//            data.Add(new TableData());

            return data;
        }
    }
}
