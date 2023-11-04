using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Common.Models;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Data.Entities;
using SchoolManagementNTAPI.Student.Interfaces;
using SchoolManagementNTAPI.Student.Models;
using SchoolManagementNTAPI.Student.Requests;
using SchoolManagementNTAPI.Student.Responses;
using SchoolManagementNTAPI.Subject.Models;

namespace SchoolManagementNTAPI.Student.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolManagementNTDBContext _context;

        public StudentService(SchoolManagementNTDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreateStudent(CreateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                Data.Entities.Student studentDto = new() { Name = request.Name };

                _context.Add(studentDto);

                foreach (var subject in request.Subjects)
                {
                    Data.Entities.StudentSubject studentSubjectDto = new()
                    {
                        SubjectId = subject.SubjectId,
                        IsMajor = subject.IsMajor
                    };
                    studentDto.StudentSubject.Add(studentSubjectDto);
                }

                await _context.SaveChangesAsync();

                return new OperationStatusResponse { Message = "Success. Student created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<OperationStatusResponse> DeleteStudent(IdRequest request)
        {
            try
            {
                var studentDto = await _context.Student.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (studentDto is null)
                    return new OperationStatusResponse { Message = $"Student with ID {request.Id} not found." };

                _context.Remove(studentDto);
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Student with ID {studentDto.Id} deleted successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<GetAllStudentsResponse> GetAllStudents()
        {
            var response = await _context.Student
                .AsNoTracking()
                .Select(x => new StudentModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subjects = x.StudentSubject.Where(y => y.StudentId == x.Id).Select(y => new SubjectDto
                    {
                        Id = y.SubjectId,
                        Name = y.Subject.Name
                    }).ToList()
                }).ToListAsync();

            return new GetAllStudentsResponse { Lista = response };
        }

        public async Task<StudentModel?> GetStudentById(IdRequest request)
        {
            var response = await _context.Student
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new StudentModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subjects = x.StudentSubject.Where(y => y.StudentId == x.Id).Select(y => new SubjectDto
                    {
                        Id = y.SubjectId,
                        Name = y.Subject.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<GetStudentWithHisMajorResponse?> GetStudentWithHisMajor(IdRequest request)
        {
            var response = await _context.Student
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new GetStudentWithHisMajorResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Major = string.Join(", ", x.StudentSubject.Where(y => y.StudentId == x.Id && y.IsMajor).Select(y => y.Subject.Name))
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<OperationStatusResponse> SetNewMajorForStudent(SetNewMajorForStudentRequest request)
        {
            var student = await _context.Student.FindAsync(request.StudentId);
            if (student == null)
                return new OperationStatusResponse { Message = "Student not found." };

            var subject = await _context.Subject.FindAsync(request.NewMajorId);
            if (subject == null)
                return new OperationStatusResponse { Message = "Subject not found." };

            var studentSubjectRecords = await _context.StudentSubject
                .Where(x => x.StudentId == request.StudentId)
                .ToListAsync();

            foreach (var item in studentSubjectRecords)
            {
                item.IsMajor = item.SubjectId == request.NewMajorId;
            }

            int rowsChanged = await _context.SaveChangesAsync();

            return rowsChanged > 0
                ? new OperationStatusResponse { Message = $"Success {rowsChanged} rows changed." }
                : new OperationStatusResponse { Message = "No changes. Student already has that major." };
        }

        public async Task<OperationStatusResponse> UpdateStudent(UpdateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                var studentDto = await _context.Student.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (studentDto is null)
                    return new OperationStatusResponse { Message = $"Student with ID {request.Id} not found." };

                studentDto.Name = request.Name;

                var rowsFromDatabase = await _context.StudentSubject.Where(x => x.StudentId == request.Id).ToListAsync();

                var rowsToDelete = getRowsToDelete(request.Subjects, rowsFromDatabase);
                var rowsToAdd = getRowsToAdd(request.Subjects, rowsFromDatabase);
                var rowsToUpdate = getRowsToUpdate(request.Subjects, rowsFromDatabase);

                if (rowsToDelete.Count > 0)
                {
                    foreach (StudentSubject row in rowsToDelete)
                    {
                        var item = rowsFromDatabase.Find(x => x.Id == row.Id);

                        if (item != null)
                            _context.Remove(item);
                    }
                }

                if (rowsToAdd.Count > 0)
                {
                    foreach (var row in rowsToAdd)
                    {
                        StudentSubject studentSubjectDTO = new()
                        {
                            StudentId = request.Id,
                            SubjectId = row.SubjectId,
                            IsMajor = row.IsMajor
                        };
                        _context.Add(studentSubjectDTO);
                    }
                }

                if (rowsToUpdate.Count > 0)
                {
                    foreach (SubjectInfo row in rowsToUpdate)
                    {
                        var item = rowsFromDatabase.Find(x => x.StudentId == request.Id && x.SubjectId == row.SubjectId);

                        if (item != null)
                        {
                            item.IsMajor = row.IsMajor;
                            _context.Update(item);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Student with ID {studentDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        private List<StudentSubject> getRowsToDelete(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<StudentSubject> listToReturn = new();

            foreach (StudentSubject item in databaseItems)
            {

                if (!request.Any(x => x.SubjectId == item.SubjectId))
                {
                    listToReturn.Add(item);
                }

            }

            return listToReturn;
        }

        private List<SubjectInfo> getRowsToAdd(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<SubjectInfo> listToReturn = new();

            foreach (SubjectInfo item in request)
            {
                if (!databaseItems.Any(x => x.SubjectId == item.SubjectId))
                {
                    listToReturn.Add(item);
                }
            }

            return listToReturn;
        }

        private List<SubjectInfo> getRowsToUpdate(List<SubjectInfo> request, List<StudentSubject> databaseItems)
        {
            List<SubjectInfo> listToReturn = new();

            foreach (SubjectInfo item in request)
            {
                if (databaseItems.Any(x => x.SubjectId == item.SubjectId && x.IsMajor != item.IsMajor))
                {
                    listToReturn.Add(item);
                }
            }

            return listToReturn;
        }
    }
}
