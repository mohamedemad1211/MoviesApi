namespace MoviesApi.Models
{
    public class Genre
    {
        //byte يمكن استخدامها بدل int لان اخرها ف العد 255
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //عشان اخليه يعد لوحده ال id
        public byte Id { get; set; }
        [Required]
        [MaxLength(100)]

        public string Name { get; set; } = string.Empty;
    }
}
