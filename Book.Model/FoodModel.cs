namespace Book.Model
{
    public class FoodRequest
    {

        /// <summary>
        /// 
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Intro { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Img { get; set; }
        
        public int Status { get; set; }
    }

    public class FoodTypeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
