using System.ComponentModel.DataAnnotations;
using Zephyr.Infrastructure;

namespace Zephyr.Notes
{
    public class NoteViewModel : ViewModel
    {
        public int NoteId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field Title is required!")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field Content is required!")]
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
