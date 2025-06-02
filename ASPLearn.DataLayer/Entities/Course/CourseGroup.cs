using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLearn.DataLayer.Entities.Course
{
    public class CourseGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "حداکثر {0}،{1} کاراکتر مجاز است")]
        public required string GroupName { get; set; }

        [Display(Name = "حذف شده")]
        public bool IsDelete { get; set; } = false;

        [Display(Name = "نام سرگروه")]
        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        [ValidateNever]
        public List<CourseGroup>? CourseGroups { get; set; }

        [InverseProperty("CourseGroup")]
        [ValidateNever]
        public List<Course> Courses { get; set; }

        [InverseProperty("Group")]
        [ValidateNever]
        public List<Course> SubGroups { get; set; }
    }
}
