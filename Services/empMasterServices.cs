﻿using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using static QuestPDF.Helpers.Colors;

namespace iMARSARLIMS.Services
{
    public class empMasterServices : IempMasterServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public empMasterServices(ContextClass context, ILogger<BaseController<empMaster>> logger, IConfiguration configuration, HttpClient httpClient)
        {

            db = context;
            this._configuration = configuration;
            this._httpClient = httpClient;
        }
        async Task<ServiceStatusResponseModel> IempMasterServices.EmpLogin(LoginRequestModel loginRequestModel)
        {
            var employee = await (from em in db.empMaster
                      join emr in db.empRoleAccess on em.empId equals emr.empId
                      join emc in db.empCenterAccess on em.empId equals emc.empId
                      where em.userName == loginRequestModel.userName 
                            && (em.password == loginRequestModel.password || em.tempPassword == loginRequestModel.password) 
                            && em.defaultcentre == emc.centreId
                      select new LoginResponseModel
                      {
                          employeeId = em.empId.ToString(),
                          Name = string.Concat(em.fName, " ", em.lName),
                          DefaultRole = em.defaultrole.ToString(),
                          DefaultCenter = em.defaultcentre.ToString(),
                            allowTicket = em.allowTicket,
                          allowTicketRole= em.allowTicketRole,
                          tempPassword = string.IsNullOrEmpty(em.tempPassword) ? "" : em.tempPassword,
                          image = string.IsNullOrEmpty(em.fileName) ? _configuration["FileBase64:profilePic"] : em.fileName
                          // Optional: remove if unnecessary
                      }).FirstOrDefaultAsync();

            if (employee != null)
            {
                var massage = "Login Successful#0";
               if (loginRequestModel.password== employee.tempPassword)
                {
                    massage = "Login Successful#1";
                }

                var token = JwtTokenGenrator.GenerateToken(
                    userid: employee.employeeId,
                    Role: employee.DefaultRole,
                    Centreid: employee.DefaultCenter,
                    key: _configuration["JwtSettings:Key"],
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    expiryMinutes: int.Parse(_configuration["JwtSettings:ExpiryMinutes"])
                    );

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = massage,
                    Data = employee,
                    Token = token
                };
            }
            else
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "Invalid UserName Or Password"
                };
            }

        }


        async Task<ActionResult<ServiceStatusResponseModel>> IempMasterServices.SaveEmployee(empMaster empmaster)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (empmaster.empId == 0)
                    {
                        
                        var count= db.empMaster.Where(e=>e.userName == empmaster.userName).Count();
                        if (count > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "User Name Already Exist, Please Enter Unique Name"
                            };
                        }
                        var count1 = 0;
                        count1 = db.centreMaster.Where(c => c.mobileNo == empmaster.mobileNo).Count();
                        if (count1 > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Duplicate Mobile NO"
                            };
                        }
                        count1 = db.empMaster.Where(c => c.mobileNo == empmaster.mobileNo).Count();
                        if (count1 > 0)
                        {
                            return new ServiceStatusResponseModel
                            {
                                Success = false,
                                Message = "Duplicate Mobile NO"
                            };
                        }
                        var EmployeeRegData = CreateEmployee(empmaster);
                        var EmployeeData = db.empMaster.Add(EmployeeRegData);
                        await db.SaveChangesAsync();
                        var employeeId = EmployeeData.Entity.empId;
                        await SaveEmpRoleAccess(empmaster.addEmpRoleAccess, employeeId);
                        await SaveEmpCentreAccess(empmaster.addEmpCentreAccess, employeeId);
                        await SaveEmpDepartmentAccess(empmaster.addEmpDepartmentAccess, employeeId);
                        var roleids = empmaster.addEmpRoleAccess.Select(l => l.roleId).ToList();
                        var menuAcccessdata = db.RolePageAccess.Where(r => roleids.Contains(r.roleid)).ToList();
                        await saveRolemenuAccess(menuAcccessdata, employeeId);
                        await transaction.CommitAsync();


                        var SmsText = _configuration["SMSText:UserPassword"].Replace("{User}", empmaster.fName).Replace("{UserName}", empmaster.userName).Replace("{TempPassword}", empmaster.password);
                        var apiUrl = _configuration["SMSText:ApiUrl"];
                        var finalUrl = apiUrl.Replace("{MobileNo}", empmaster.mobileNo);
                        finalUrl = finalUrl.Replace("{Msg}", SmsText);
                        finalUrl = finalUrl.Replace("{Sender}", "Wellness Diagnostic");
                        try
                        {
                            var response = await _httpClient.GetAsync(finalUrl);
                        }
                        catch (Exception ex)
                        {

                        }

                        var result = db.empMaster.Where(e => e.empId == employeeId).ToList();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        var EmpMaster = db.empMaster.FirstOrDefault(em => em.empId == empmaster.empId);
                        if (EmpMaster != null)
                        {
                            UpdateEmployee(EmpMaster, empmaster);
                        }
                        var EmployeeMaster = db.empMaster.Update(EmpMaster);
                        await db.SaveChangesAsync();
                        var employeeId = EmployeeMaster.Entity.empId;
                        await UpdateEmpRoleAccess(empmaster.addEmpRoleAccess, employeeId);
                        await UpdateEmpCentreAccess(empmaster.addEmpCentreAccess, employeeId);
                        await UpdateEmpDepartmentAccess(empmaster.addEmpDepartmentAccess, employeeId);
                        await transaction.CommitAsync();
                        var result = db.empMaster.Where(e => e.empId == employeeId).ToList();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };


                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };

                }
            }
        }

        private empMaster CreateEmployee(empMaster empmaster)
        {

            return new empMaster
            {
                empId = empmaster.empId,
                empCode = empmaster.empCode,
                title = empmaster.title,
                fName = empmaster.fName,
                lName = empmaster.lName,
                address = empmaster.address,
                pinCode = empmaster.pinCode,
                email = empmaster.email,
                mobileNo = empmaster.mobileNo,
                dob = empmaster.dob,
                qualification = empmaster.qualification,
                bloodGroup = empmaster.bloodGroup,
                designationId = empmaster.designationId,
                userName = empmaster.userName,
                password = empmaster.password,
                zone = empmaster.zone,
                state = empmaster.state,
                city = empmaster.city,
                area = empmaster.area,
                district=empmaster.district,
                defaultcentre = empmaster.defaultcentre,
                pro = empmaster.pro,
                defaultrole = empmaster.defaultrole,
                rate = empmaster.rate,
                fileName = empmaster.fileName,
                autoCreated = empmaster.autoCreated,
                centreId = empmaster.centreId,
                allowDueReport = empmaster.allowDueReport,
                employeeType = empmaster.employeeType,
                isSalesTeamMember = empmaster.isSalesTeamMember,
                isDiscountAppRights = empmaster.isDiscountAppRights,
                isPwdchange = empmaster.isPwdchange,
                isemailotp = empmaster.isemailotp,
                adminPassword = empmaster.adminPassword,
                isActive = empmaster.isActive,
                createdById = empmaster.createdById,
                createdDateTime = empmaster.createdDateTime,
                fromIP = empmaster.fromIP,
                toIP = empmaster.toIP,
                isdeviceAuthentication = empmaster.isdeviceAuthentication,
                tempPassword = empmaster.tempPassword,
                indentIssue = empmaster.indentIssue,
                IndentApprove = empmaster.IndentApprove,
                allowTicket= empmaster.allowTicket,
                allowTicketRole= empmaster.allowTicketRole,
                employeeCentretype= empmaster.employeeCentretype

            };
        }

        private async Task<ServiceStatusResponseModel> saveRolemenuAccess(IEnumerable<RolePageAccess> pageacess, int employeeid)
        {
            if (pageacess != null)
            {
                var pageaccessData = pageacess.Select(page => CreateRolemenuAcess(page, employeeid)).ToList();
                if (pageaccessData.Any())
                {
                    db.roleMenuAccess.AddRange(pageaccessData);
                    await db.SaveChangesAsync();
                }
            }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    
                };
        }
        private roleMenuAccess CreateRolemenuAcess(RolePageAccess pageaccess, int employeeid)
        {
            return new roleMenuAccess
            {
                id=0,
                roleId=pageaccess.roleid,
                menuId= pageaccess.parentmenuid,
                subMenuId= pageaccess.submenuId,
                employeeId=employeeid,
                isActive=true
            };
        }
        private async Task<ServiceStatusResponseModel> SaveEmpRoleAccess(IEnumerable<empRoleAccess> emproleaccess, int employeeId)
        {
            if (emproleaccess != null)
            {
                var empRoleDataList = emproleaccess.Select(emprole => CreateEmpRoleData(emprole, employeeId)).ToList();
                if (empRoleDataList.Any())
                {
                    db.empRoleAccess.AddRange(empRoleDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = emproleaccess
            };

        }

        private empRoleAccess CreateEmpRoleData(empRoleAccess emproleaccess, int employeeId)
        {
            return new empRoleAccess
            {
                id = 0,
                empId = employeeId,
                roleId = emproleaccess.roleId,
                isActive = emproleaccess.isActive,
                createdById = emproleaccess.createdById,
                createdDateTime = emproleaccess.createdDateTime,
            };

        }
        private async Task<ServiceStatusResponseModel> SaveEmpCentreAccess(IEnumerable<empCenterAccess> empcentreaccess, int employeeId)
        {
            if (empcentreaccess != null)
            {
                var empCentreDataList = empcentreaccess.Select(empcentre => CreateEmpCentreData(empcentre, employeeId)).ToList();
                if (empCentreDataList.Any())
                {
                    db.empCenterAccess.AddRange(empCentreDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empcentreaccess
            };

        }

        private empCenterAccess CreateEmpCentreData(empCenterAccess empcentreaccess, int employeeId)
        {
            return new empCenterAccess
            {
                id = 0,
                empId = employeeId,
                centreId = empcentreaccess.centreId,
                isActive = empcentreaccess.isActive,
                createdById = empcentreaccess.createdById,
                createdDateTime = empcentreaccess.createdDateTime
            };

        }
        private async Task<ServiceStatusResponseModel> SaveEmpDepartmentAccess(IEnumerable<empDepartmentAccess> empDepartment, int employeeId)
        {
            if (empDepartment != null)
            {
                var empDepatrtmentDataList =  empDepartment.Select(empDept => CreateEmpDepartmentData(empDept, employeeId)).ToList();
                if (empDepatrtmentDataList.Any())
                {
                    db.empDepartmentAccess.AddRange(empDepatrtmentDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empDepartment
            };

        }

        private empDepartmentAccess CreateEmpDepartmentData(empDepartmentAccess empDepartment, int employeeId)
        {
            return new empDepartmentAccess
            {
                id = 0,
                empId = employeeId,
                departmentId = empDepartment.departmentId,
                isActive = empDepartment.isActive,
                createdById = empDepartment.createdById,
                createdDateTime = empDepartment.createdDateTime
            };

        }

        private void UpdateEmployee(empMaster EmpMaster, empMaster empmaster)
        {
            EmpMaster.empCode = empmaster.empCode;
            EmpMaster.title = empmaster.title;
            EmpMaster.fName = empmaster.fName;
            EmpMaster.lName = empmaster.lName;
            EmpMaster.address = empmaster.address;
            EmpMaster.pinCode = empmaster.pinCode;
            EmpMaster.email = empmaster.email;
            EmpMaster.mobileNo = empmaster.mobileNo;
            EmpMaster.dob = empmaster.dob;
            EmpMaster.qualification = empmaster.qualification;
            EmpMaster.bloodGroup = empmaster.bloodGroup;
            EmpMaster.designationId = empmaster.designationId;
            EmpMaster.userName = empmaster.userName;
            EmpMaster.password = empmaster.password;
            EmpMaster.zone = empmaster.zone;
            EmpMaster.state = empmaster.state;
            EmpMaster.city = empmaster.city;
            EmpMaster.area = empmaster.area;
            EmpMaster.district= empmaster.district;
            EmpMaster.defaultcentre = empmaster.defaultcentre;
            EmpMaster.pro = empmaster.pro;
            EmpMaster.defaultrole = empmaster.defaultrole;
            EmpMaster.rate = empmaster.rate;
            EmpMaster.fileName = empmaster.fileName;
            EmpMaster.autoCreated = empmaster.autoCreated;
            EmpMaster.centreId = empmaster.centreId;
            EmpMaster.allowDueReport = empmaster.allowDueReport;
            EmpMaster.employeeType = empmaster.employeeType;
            EmpMaster.isSalesTeamMember = empmaster.isSalesTeamMember;
            EmpMaster.isDiscountAppRights = empmaster.isDiscountAppRights;
            EmpMaster.isPwdchange = empmaster.isPwdchange;
            EmpMaster.isemailotp = empmaster.isemailotp;
            EmpMaster.adminPassword = empmaster.adminPassword;
            EmpMaster.isActive = empmaster.isActive;
            EmpMaster.updateById = empmaster.updateById;
            EmpMaster.updateDateTime = empmaster.updateDateTime;
            EmpMaster.fromIP = empmaster.fromIP;
            EmpMaster.toIP = empmaster.toIP;
            EmpMaster.isdeviceAuthentication = empmaster.isdeviceAuthentication;
            EmpMaster.tempPassword = empmaster.tempPassword;
            EmpMaster.indentIssue = empmaster.indentIssue;
            EmpMaster.IndentApprove = empmaster.IndentApprove;
            EmpMaster.allowTicket= empmaster.allowTicket;
            EmpMaster.allowTicketRole= empmaster.allowTicketRole;
            EmpMaster.employeeCentretype= empmaster.employeeCentretype;
        }

        private async Task<ServiceStatusResponseModel> UpdateEmpCentreAccess(IEnumerable<empCenterAccess> empcenteraccess, int employeeId)
        {
            var Empid= empcenteraccess.Select(e=> e.empId).FirstOrDefault();
            var centreIds= empcenteraccess.Select(e => e.centreId).ToList();
            var centreaccess = db.empCenterAccess.Where(e => e.empId == Empid).ToList();
            db.empCenterAccess.RemoveRange(centreaccess);
            await db.SaveChangesAsync();

            if (empcenteraccess != null)
            {
                var empCentreDataList = empcenteraccess.Select(empCentre => CreateEmpCentreData(empCentre, employeeId)).ToList();
                if (empCentreDataList.Any())
                {
                    db.empCenterAccess.AddRange(empCentreDataList);
                    await db.SaveChangesAsync();
                }
            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empcenteraccess
            };
        }
        private async Task<ServiceStatusResponseModel> UpdateEmpDepartmentAccess(IEnumerable<empDepartmentAccess> empDepartment, int employeeId)
        {
            var Empid = empDepartment.Select(e => e.empId).FirstOrDefault();
            var DepartmentAccess = db.empDepartmentAccess.Where(e => e.empId == Empid).ToList();
            db.empDepartmentAccess.RemoveRange(DepartmentAccess);
            await db.SaveChangesAsync();

            if (empDepartment != null)
            {
                var empDepartmentDataList = empDepartment.Select(empDept => CreateEmpDepartmentData(empDept, employeeId)).ToList();
                if (empDepartmentDataList.Any())
                {
                    db.empDepartmentAccess.AddRange(empDepartmentDataList);
                    await db.SaveChangesAsync();
                }
            }

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empDepartment
            };
        }
        

        private async Task<ServiceStatusResponseModel> UpdateEmpRoleAccess(IEnumerable<empRoleAccess> emproleaccess, int employeeId)
        {
            var Empid = emproleaccess.Select(e => e.empId).FirstOrDefault();
            var RoleAccess = db.empRoleAccess.Where(e => e.empId == Empid).ToList();
            db.empRoleAccess.RemoveRange(RoleAccess);
            await db.SaveChangesAsync();

            if (emproleaccess != null)
            {
                var EmproleList = emproleaccess.Select(empRole => CreateEmpRoleData(empRole, employeeId)).ToList();
                if (EmproleList.Any())
                {
                    db.empRoleAccess.AddRange(EmproleList);
                    await db.SaveChangesAsync();
                }
            }

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = emproleaccess
            };
        }
       
        async Task<ServiceStatusResponseModel> IempMasterServices.UploadDocument(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (extension != ".pdf" && extension != ".Pdf" && extension != ".PDF" && extension != ".jpg" && extension != ".png" && extension != ".JPG" && extension != ".PNG" && extension != ".JPEG" && extension != ".jpeg")
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = "No valid file extension Found"
                };
            }
            try
            {
                string primaryFolder = _configuration["DocumentPath:PrimaryFolder"];
                if (!Directory.Exists(primaryFolder))
                {
                    Directory.CreateDirectory(primaryFolder);
                }
                string uploadPath = Path.Combine(primaryFolder, "UploadedDocuments");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Message = "File uploaded and saved successfully.",
                    Data = new { FilePath = filePath }
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.DownloadImage(int emplpyeeid)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (emplpyeeid != 0)
                    {
                        var imagepath = db.empMaster.Where(e => e.empId == emplpyeeid).Select(e => e.fileName).FirstOrDefault();
                        byte[] imageBytes = File.ReadAllBytes(imagepath);
                        string image = Convert.ToBase64String(imageBytes);
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = image
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "wrong employee Id"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.EmployeeWiseCentre(int EmplyeeId)
        {
            var Centres = await (from eca in db.empCenterAccess
                                 join cm in db.centreMaster on eca.centreId equals cm.centreId
                                 where eca.empId == EmplyeeId
                                 orderby eca.centreId
                                 select new
                                 {
                                     CentreId = cm.centreId,
                                     CentreName = cm.companyName,
                                     cm.paymentMode,
                                     cm.paymentModeId
                                 }).ToListAsync();

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = Centres,
                Message = ""
            };
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.EmployeeWiseRole(int EmplyeeId)
        {
            var Roles = await (from era in db.empRoleAccess
                               join rm in db.roleMaster on era.roleId equals rm.id
                               where era.empId == EmplyeeId
                               orderby rm.roleName
                               select new
                               {
                                   RoleId = rm.id,
                                   RoleName = rm.roleName
                               }).ToListAsync();

            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = Roles,
                Message = ""
            };
        }

        public async Task<ServiceStatusResponseModel> EmployeeWiseMenu(string employeeId, string roleId, string centreId,int MenuType)
        {
            // Fetch menu data
            var menuData = from rma in db.roleMenuAccess
                           join mm in db.menuMaster on rma.menuId equals mm.id
                           join mm1 in db.menuMaster on rma.subMenuId equals mm1.id
                           join mi in db.menuIconMaster on 1 equals mi.id into parentIcons
                           from parentIcon in parentIcons.DefaultIfEmpty()
                           join mi1 in db.menuIconMaster on 4 equals mi1.id into childIcons
                           from childIcon in childIcons.DefaultIfEmpty()
                           where employeeId == rma.employeeId.ToString() && roleId == rma.roleId.ToString() && mm.isActive==1
                 
                           select new
                           {
                               ParentMenuId = mm.id,
                               ParentMenuName = mm.menuName,
                               ParentDisplayOrder = mm.displaySequence,
                               ParentIcon = parentIcon,
                               ChildMenuId = mm1.id,
                               ChildMenuName = mm1.menuName,
                               NavigationURL = mm1.navigationUrl,
                               ChildDisplayOrder = mm1.displaySequence,
                               ChildIcon = childIcon,
                               mm1.isHide
                           };

            if (MenuType == 2)
            {
                menuData = menuData.Where(m => m.ParentMenuId == m.ParentMenuId && m.isHide == true);
            }
            // Materialize data to allow client-side operations
            var menuDataList = await menuData.ToListAsync();

            // Perform string formatting on the client side
            var groupedMenuData = menuDataList
                .GroupBy(m => new
                {
                    m.ParentMenuId,
                    m.ParentMenuName,
                    m.ParentDisplayOrder,
                    m.ParentIcon
                })
                .Select(group => new
                {
                    parentMenuId = group.Key.ParentMenuId,
                    parentMenuName = group.Key.ParentMenuName,
                    parentDisplayOrder= group.Key.ParentDisplayOrder,
                    parentIcons = group.Key.ParentIcon != null
                        ? group.Key.ParentIcon.icon : null,
                    children = group
                        .Where(child => child.ChildMenuId != null)
                        .Select(child => new
                        {
                            childMenuId = child.ChildMenuId,
                            childMenuName = child.ChildMenuName,
                            navigationURL = child.NavigationURL,
                            childDisplayOrder= child.ChildDisplayOrder,
                            childIcons = child.ChildIcon != null
                                ? child.ChildIcon.icon  : null
                        })
                        .OrderBy(child => child.childDisplayOrder)
                        .ToList()
                })
                .OrderBy(parent => parent.parentDisplayOrder)
                .ToList();

            // Generate JWT token
            var token = JwtTokenGenrator.GenerateToken(
                userid: employeeId,
                Role: roleId,
                Centreid: centreId,
                key: _configuration["JwtSettings:Key"],
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expiryMinutes: int.Parse(_configuration["JwtSettings:ExpiryMinutes"])
            );

            // Return response
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = groupedMenuData,
                Message = "Menu retrieved successfully.",
                Token = token
            };
        }


        async Task<ServiceStatusResponseModel> IempMasterServices.forgetPassword(string Username)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                var empdata = await db.empMaster.Where(e => e.userName == Username).FirstOrDefaultAsync();

                if (empdata == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var TempPassword = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                if (empdata != null)
                {
                    empdata.tempPassword = TempPassword;
                }
                var SmsText = _configuration["SMSText:ForGetPassword"].Replace("{User}", empdata.fName).Replace("{TempPassword}", TempPassword);

                var apiUrl = _configuration["SMSText:ApiUrl"];
                var finalUrl = apiUrl.Replace("{MobileNo}", empdata.mobileNo);
                finalUrl = finalUrl.Replace("{Msg}", SmsText);
                finalUrl = finalUrl.Replace("{Sender}", "Testing");
                try
                {
                    var response = await _httpClient.GetAsync(finalUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        db.empMaster.Update(empdata);
                        await db.SaveChangesAsync();
                       await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "SMS sent successfully."
                        };
                    }
                    else
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = $"Failed to send SMS. Status code: {response.StatusCode}"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}"
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.UpdatePassword(int Employeeid, string Password)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var empdata =await  db.empMaster.Where(e => e.empId == Employeeid).FirstOrDefaultAsync();
                    empdata.password = Password;
                    empdata.tempPassword = "";
                    db.empMaster.Update(empdata);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Password Reset Successful"

                    };
                }
                catch (Exception ex)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message

                    };

                }
            }

        }

        async Task<ServiceStatusResponseModel> IempMasterServices.GetAllMenu()
        {
            var menunevigationUrl=  await db.menuMaster.Where(m=>m.parentId!=0).Select(m=> m.navigationUrl).Distinct().ToListAsync();
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = menunevigationUrl,
                Message = "Menu retrieved successfully.",
                
            };
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.UpdateEmployeeStatus(int EmplyeeId, byte status,int UserId)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var result =  db.empMaster.Where(e => e.empId == EmplyeeId).FirstOrDefault();
                    if(result == null)
                    {
                        return new ServiceStatusResponseModel
                        {
                            Success = false,
                            Message = "Please enter Correct EmpId"
                        };
                    }
                    else
                    {
                        result.isActive = status;
                        result.updateById = UserId;
                        result.updateDateTime= DateTime.Now;
                        db.empMaster.Update(result);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Status Updated Successful"
                        };
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }

        async Task<ServiceStatusResponseModel> IempMasterServices.BillingTypeWiseCentre(int EmplyeeId, int Billingtype)
        {
            try
            {
                var Centres = await(from eca in db.empCenterAccess
                                    join cm in db.centreMaster on eca.centreId equals cm.centreId
                                    where eca.empId == EmplyeeId && cm.billingType == Billingtype
                                    orderby eca.centreId
                                    select new
                                    {
                                        CentreId = cm.centreId,
                                        CentreName = cm.companyName,
                                        cm.paymentMode,
                                        cm.paymentModeId,cm.isPrePrintedBarcode,cm.barcodeType
                                    }).ToListAsync();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = Centres                 
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
