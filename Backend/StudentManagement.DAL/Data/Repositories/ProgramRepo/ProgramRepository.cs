﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ProgramRepo
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Program>> AddProgramAsync(Program program)
        {
            try
            {
                program.Id = Guid.NewGuid();
                //program.CreatedAt = DateTime.Now;
                //program.UpdatedAt = DateTime.Now;
                await _context.Programs.AddAsync(program);
                await _context.SaveChangesAsync();
                //var addedProgram = await _context.Programs.FirstOrDefaultAsync(p => p.Code == program.Code);
                var addedProgram = await _context.Programs.FirstOrDefaultAsync(p => p.Id == program.Id);
                return Result<Program>.Ok(addedProgram);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_programs_name") == true)
            {
                return Result<Program>.Fail("PROGRAM_NAME_EXIST", "Program name already exists");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Program>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Program>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Program>.Fail("ADD_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Program>>> GetAllProgramsAsync()
        {
            try
            {
                var programs = await _context.Programs.ToListAsync();
                return Result<IEnumerable<Program>>.Ok(programs);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<IEnumerable<Program>>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<IEnumerable<Program>>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Program>>.Fail("GET_PROGRAMS_FAILED", ex.Message);
            }
        }

        public async Task<Result<Program?>> GetProgramByIdAsync(Guid id)
        {
            try
            {
                var program = await _context.Programs.FindAsync(id);
                return Result<Program?>.Ok(program);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Program?>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Program?>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Program?>.Fail("GET_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<Program?>> GetProgramByIdAsync(string id) => await GetProgramByIdAsync(id.ToGuid());

        public async Task<Result<Program?>> GetProgramByNameAsync(string name)
        {
            try
            {
                var program = await _context.Programs.FirstOrDefaultAsync(p => p.Name == name);
                return Result<Program?>.Ok(program);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Program?>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Program?>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Program?>.Fail("GET_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<Program>> UpdateProgramAsync(Program program)
        {
            try
            {
                var existingProgram = await _context.Programs.FindAsync(program.Id);
                if (existingProgram == null)
                    return Result<Program>.Fail("PROGRAM_NOT_EXIST", "Program does not exist");

                foreach (var prop in typeof(Program).GetProperties())
                {
                    var value = prop.GetValue(program);
                    if (value is null) continue;
                    if (prop.GetValue(existingProgram) == value) continue;
                    prop.SetValue(existingProgram, value);
                }

                //existingProgram.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return Result<Program>.Ok(existingProgram);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_programs_name") == true)
            {
                return Result<Program>.Fail("PROGRAM_NAME_EXIST", "Program name already exists");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Program>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Program>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Program>.Fail("UPDATE_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<Program>> DeleteProgramAsync(Program program)
        {
            try
            {
                var existingProgram = await _context.Programs.Where(p => p.Id == program.Id || p.Name == program.Name).FirstOrDefaultAsync();
                if (existingProgram is null) return Result<Program>.Fail("FACULTY_NOT_EXIST", "Faculty does not exist");
                _context.Programs.Remove(existingProgram);
                await _context.SaveChangesAsync();
                return Result<Program>.Ok(existingProgram);
            }
            catch (DbUpdateException ex)
            {
                return Result<Program>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Program>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Program>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Program>.Fail("DELETE_FACULTY_FAILED", ex.Message);
            }
        }

        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }
    }
}