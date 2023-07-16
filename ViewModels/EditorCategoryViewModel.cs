using System.ComponentModel.DataAnnotations;

namespace AspNet_Core6.Fundamentals.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "The 'Name' field is required.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "This field must contain between three and forty characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The 'Slug' field is required.")]
        public string Slug { get; set; }
    }
}
