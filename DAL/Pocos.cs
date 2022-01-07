namespace Zephyr.DAL
{
    [Table(Name = "note", Schema = "public")]
    public class NotePoco 
    {
        [Column(IsPrimaryKey = true, Name = "note_id")]
        public int NoteId { get; set; }
        [Column(Name = "title")]
        public string Title { get; set; }
        [Column(Name = "content")]
        public string Content { get; set; }
        [Column(Name = "created")]
        public DateTime Created { get; set; }
        [Column(Name = "last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}
