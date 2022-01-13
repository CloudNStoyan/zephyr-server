using System.ComponentModel.DataAnnotations;
using Zephyr.Infrastructure;

namespace Zephyr.Areas.LoL.RunePage
{
    public class RunePageViewModel : ViewModel
    {
        public int RunePageId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Primary Style Id is required")]
        public int PrimaryStyleId { get; set; }
        [Required(ErrorMessage = "Sub Style Id is required!")]
        public int SubStyleId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Perk Ids are required!")]
        public string PerkIds { get; set; }
    }
}
