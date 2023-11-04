using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Data.Entities;
using SchoolManagementNTAPI.School.Interfaces;
using SchoolManagementNTAPI.School.Models;
using SchoolManagementNTAPI.School.Requests;
using SchoolManagementNTAPI.School.Responses;

namespace SchoolManagementNTAPI.School.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolManagementNTDBContext _context;

        public SchoolService(SchoolManagementNTDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreateSchool(CreateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                Data.Entities.School schoolDto = new() { Name = request.Name };

                _context.Add(schoolDto);
                await _context.SaveChangesAsync();

                return new OperationStatusResponse { Message = "Success. School created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<OperationStatusResponse> DeleteSchool(IdRequest request)
        {
            try
            {
                var schoolDto = await _context.School.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (schoolDto is null)
                    return new OperationStatusResponse { Message = $"School with ID {request.Id} not found." };

                _context.Remove(schoolDto);
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. School with ID {schoolDto.Id} deleted successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<GetAllSchoolsResponse> GetAllSchools()
        {
            var response = await _context.School
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return new GetAllSchoolsResponse { Lista = response };
        }

        public async Task<SchoolModel?> GetSchoolById(IdRequest request)
        {
            var response = await _context.School
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<SchoolModel?> GetSchoolByPrincipalId(IdRequest request)
        {
            var response = await _context.Principal
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.School.Id,
                    Name = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<SchoolModel?> GetSchoolByProfessorId(IdRequest request)
        {
            var response = await _context.Professor
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new SchoolModel
                {
                    Id = x.School.Id,
                    Name = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<OperationStatusResponse> UpdateSchool(UpdateSchoolRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                var schoolDto = await _context.School.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (schoolDto is null)
                    return new OperationStatusResponse { Message = $"School with ID {request.Id} not found." };

                schoolDto.Name = request.Name;
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. School with ID {schoolDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
