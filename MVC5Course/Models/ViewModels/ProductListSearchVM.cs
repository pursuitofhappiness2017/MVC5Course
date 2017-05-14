using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    public class ProductListSearchVM : IValidatableObject
    {
        public string q { get; set; }

        public int stock_min { get; set; }

        public int stock_max { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.stock_min > this.stock_max)
            {
                yield return new ValidationResult("庫存資料篩選條件錯誤", new string[] { "stock_min", "stock_max" });
            }
        }
    }
}