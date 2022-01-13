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

    [Table(Name = "rune_page", Schema = "public")]
    public class RunePagePoco
    {
        [Column(IsPrimaryKey = true, Name = "rune_page_id")]
        public int RunePageId { get; set; }
        [Column(Name = "name")]
        public string Name { get; set; }
        [Column(Name = "perk_ids")]
        public Array PerkIds { get; set; }
        [Column(Name = "primary_style_id")]
        public int PrimaryStyleId { get; set; }
        [Column(Name = "sub_style_id")]
        public int SubStyleId { get; set; }
    }
}
