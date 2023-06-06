namespace LinQ_EF_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supply_Quantity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Supply_Num { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_Code { get; set; }

        public int Supplied_Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime Prod_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Expiration_Date { get; set; }

        public virtual Product Product { get; set; }

        public virtual Supply Supply { get; set; }
    }
}
