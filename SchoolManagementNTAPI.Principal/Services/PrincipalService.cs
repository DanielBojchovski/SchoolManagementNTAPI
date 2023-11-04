﻿using Microsoft.EntityFrameworkCore;
using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Data.Entities;
using SchoolManagementNTAPI.Principal.Interfaces;
using SchoolManagementNTAPI.Principal.Models;
using SchoolManagementNTAPI.Principal.Requests;
using SchoolManagementNTAPI.Principal.Responses;

namespace SchoolManagementNTAPI.Principal.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly SchoolManagementNTDBContext _context;

        public PrincipalService(SchoolManagementNTDBContext context)
        {
            _context = context;
        }

        public async Task<OperationStatusResponse> CreatePrincipal(CreatePrincipalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                Data.Entities.Principal principalDto = new() { Name = request.Name, SchoolId = request.SchoolId };

                _context.Add(principalDto);
                await _context.SaveChangesAsync();

                return new OperationStatusResponse { Message = "Success. Principal created successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<OperationStatusResponse> DeletePrincipal(IdRequest request)
        {
            try
            {
                var principalDto = await _context.Principal.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (principalDto is null)
                    return new OperationStatusResponse { Message = $"Principal with ID {request.Id} not found." };

                _context.Remove(principalDto);
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Principal with ID {principalDto.Id} deleted successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<GetAllPrincipalsResponse> GetAllPrincipals()
        {
            var response = await _context.Principal
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).ToListAsync();

            return new GetAllPrincipalsResponse { Lista = response };
        }

        public async Task<PrincipalModel?> GetPrincipalById(IdRequest request)
        {
            var response = await _context.Principal
                .Where(x => x.Id == request.Id)
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<PrincipalModel?> GetPrincipalBySchoolId(IdRequest request)
        {
            var response = await _context.Principal
                .Where(x => x.SchoolId == request.Id)
                .AsNoTracking()
                .Select(x => new PrincipalModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SchoolName = x.School.Name
                }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<OperationStatusResponse> UpdatePrincipal(UpdatePrincipalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new OperationStatusResponse { Message = "Invalid name." };

            try
            {
                var principalDto = await _context.Principal.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                if (principalDto is null)
                    return new OperationStatusResponse { Message = $"Principal with ID {request.Id} not found." };

                principalDto.Name = request.Name;
                principalDto.SchoolId = request.SchoolId;
                await _context.SaveChangesAsync();
                return new OperationStatusResponse { Message = $"Success. Principal with ID {principalDto.Id} updated successfully." };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
