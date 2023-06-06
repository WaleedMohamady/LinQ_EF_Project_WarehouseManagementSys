namespace LinQ_EF_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_MeasuringUnit
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_Code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Measuring_Unit { get; set; }

        public virtual Product Product { get; set; }
    }
}
