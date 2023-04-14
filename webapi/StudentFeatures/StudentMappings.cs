namespace webapi.StudentFeatures;

public static class StudentMappings
{
    public static Student FromDTO(this StudentDTO request)
    {
        return new Student
        {
            StudentId = request.StudentId == Guid.Empty ? Guid.NewGuid() : request.StudentId, 
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Nickname = request.Nickname
        };
    }

    public static IList<StudentDTO> ToDTO(this IEnumerable<Student> request)
    {
        return request
            .Select(ToDTO)
            .ToList();
    }

    public static StudentDTO ToDTO(this Student request)
    {
        return new StudentDTO
        {
            StudentId = request.StudentId,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Nickname = request.Nickname
        };
    }
}
