using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Data.Entities;
using SchoolManagementNTAPI.Professor.Interfaces;
using SchoolManagementNTAPI.Professor.Models;
using SchoolManagementNTAPI.Professor.Requests;
using SchoolManagementNTAPI.Professor.Responses;

namespace SchoolManagementNTAPI.Professor.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly SchoolManagementNTDBContext _context;

        public ProfessorService(SchoolManagementNTDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreateProfessor(CreateProfessorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                Data.Entities.Professor professorDto = new() { Name = request.Name, SchoolId = request.SchoolId };

                _context.Add(professorDto);
                await _context.SaveChangesAsync();

                return new OperationStatusResponse { Message = "Success. Professor created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<OperationStatusResponse> DeleteProfessor(IdRequest request)
        {
            try
            {
                var professorDto = await _context.Professor.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (professorDto is null)
                    return new OperationStatusResponse { Message = $"Professor with ID {request.Id} not found." };

                _context.Remove(professorDto);
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Professor with ID {professorDto.Id} deleted successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<GetAllProfessorsResponse> GetAllProfessors()
        {
            var response = await _context.Professor
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            return new GetAllProfessorsResponse { Lista = response };
        }

        public async Task<ProfessorModel?> GetProfessorById(IdRequest request)
        {
            var response = await _context.Professor
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<GetAllProfessorsResponse> GetProfessorsBySchoolId(IdRequest request)
        {
            var response = await _context.Professor
                .Where(x => x.SchoolId == request.Id)
                .AsNoTracking()
                .Select(x => new ProfessorModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            return new GetAllProfessorsResponse { Lista = response };
        }

        public async Task<OperationStatusResponse> UpdateProfessor(UpdateProfessorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                var professorDto = await _context.Professor.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (professorDto is null)
                    return new OperationStatusResponse { Message = $"Professor with ID {request.Id} not found." };

                professorDto.Name = request.Name;
                professorDto.SchoolId = request.SchoolId;
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Professor with ID {professorDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
