using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Student.Models;
using SchoolManagementNTAPI.Student.Requests;
using SchoolManagementNTAPI.Student.Responses;

namespace SchoolManagementNTAPI.Student.Interfaces
{
    public interface IStudentService
    {
        public Task<GetAllStudentsResponse> GetAllStudents();
        public Task<StudentModel?> GetStudentById(IdRequest request);
        public Task<GetStudentWithHisMajorResponse?> GetStudentWithHisMajor(IdRequest request);
        public Task<OperationStatusResponse> SetNewMajorForStudent(SetNewMajorForStudentRequest request);
        public Task<OperationStatusResponse> CreateStudent(CreateStudentRequest request);
        public Task<OperationStatusResponse> UpdateStudent(UpdateStudentRequest request);
        public Task<OperationStatusResponse> DeleteStudent(IdRequest request);
    }
}
