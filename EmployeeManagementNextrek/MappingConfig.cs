﻿using AutoMapper;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Models.Dtos;

namespace EmployeeManagementNextrek
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<EmployeCreateDto, Employee>().ReverseMap();
            CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, DepartmentCreateDto>().ReverseMap();
            CreateMap<Department, EmployeeUpdateDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleCreateDto>().ReverseMap();
            CreateMap<Role, RoleUpDateDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Permission, PermissionCreateDto>().ReverseMap();
            CreateMap<Permission, PermissionUpDateDto>().ReverseMap();
            CreateMap<EmployeeAddress, EmployeeAddressDto>().ReverseMap();
            CreateMap<EmployeeAddress, EmployeeAddressCreateDto>().ReverseMap();
            CreateMap<EmployeeAddress, EmployeeAddressUpDateDto>().ReverseMap();
            CreateMap<EmployeeDocument, EmployeeDocumentDto>().ReverseMap();
            CreateMap<EmployeeDocument, EmployeeDocumentCreateDto>().ReverseMap();
            CreateMap<EmployeeDocumentDto, EmployeeDocumentUpdateDto>().ReverseMap();
            CreateMap<EmployeeRole, EmployeeRoleCreateDto>().ReverseMap();
            CreateMap<RolePermission, RolePermissionCreateDto>().ReverseMap();
            CreateMap<WorkSchedule, WorkScheduleDto>().ReverseMap();
            CreateMap<WorkSchedule, WorkScheduleCreateDto>().ReverseMap();
            CreateMap<WorkSchedule, WorkScheduleUpdateDto>().ReverseMap();
            CreateMap<WorkShift, WorkShiftCreateDto>().ReverseMap();    
            CreateMap<WorkShift, WorkShiftDto>().ReverseMap();
            CreateMap<WorkShift, WorkShiftUpdateDto>().ReverseMap();
            CreateMap<ScheduleException, ScheduleExceptionCreateDto>().ReverseMap();
            CreateMap<ScheduleException, ScheduleExceptionDto>().ReverseMap();
            CreateMap<ScheduleException, ScheduleExceptionUpdateDto>().ReverseMap();

        }
    }
}
