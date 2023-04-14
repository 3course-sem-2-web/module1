namespace webapi.StudentFeatures;

public class Student
{
    public Guid StudentId { get; set; }
    public string Nickname { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
}