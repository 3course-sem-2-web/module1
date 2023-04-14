using System.ComponentModel.DataAnnotations;

namespace webapi.StudentFeatures;

public record struct StudentDTO(
    Guid StudentId,
    [Required] 
    string Nickname,
    [Required] 
    string Firstname,
    [Required]
    string Lastname);
