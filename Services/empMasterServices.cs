using Google.Protobuf;
using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace iMARSARLIMS.Services
{
    public class empMasterServices : IempMasterServices
    {
        private readonly ContextClass db;
        private readonly IConfiguration _configuration;
        public empMasterServices(ContextClass context, ILogger<BaseController<empMaster>> logger, IConfiguration configuration)
        {

            db = context;
            this._configuration = configuration;
        }
        async Task<ActionResult<List<LoginResponseModel>>> IempMasterServices.EmpLogin(LoginRequestModel loginRequestModel)
        {
            var employee = await (from em in db.empMaster
                                  join emr in db.empRoleAccess on em.id equals emr.empId
                                  join emc in db.empCenterAccess on em.id equals emc.empId
                                  where em.userName == loginRequestModel.userName && em.password == loginRequestModel.password
                                  select new LoginResponseModel
                                  {
                                      employeeId = em.id,
                                      Name = string.Concat(em.fName, ' ', em.lName),
                                      DefaultRole = em.defaultrole,
                                      DefaultCenter = em.defaultcentre,
                                      Centres = emc.centreId,
                                      Roles = emr.roleId
                                  }).ToListAsync();

            if (employee != null)
            {
                return employee;
            }

            return new ActionResult<List<LoginResponseModel>>(new List<LoginResponseModel>()); // Return an empty model for invalid login

        }


        async Task<ActionResult<ServiceStatusResponseModel>> IempMasterServices.SaveEmployee(empMaster empmaster)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    string image = _configuration["FileBase64:profilePic"];
                    var filename = "";
                    if (empmaster.id == 0)
                    {
                        if (empmaster.fileName != "")
                        {
                            filename = await Uploademployeeimage(empmaster.fileName);
                            if (filename == "Invalid image data." || filename == "Error uploading image")
                            {
                                return new ServiceStatusResponseModel
                                {
                                    Success = false,
                                    Message = filename
                                };
                            }
                        }
                        empmaster.fileName = filename;
                        var EmployeeRegData = CreateEmployee(empmaster);
                        var EmployeeData = db.empMaster.Add(EmployeeRegData);
                        await db.SaveChangesAsync();
                        var employeeId = EmployeeData.Entity.id;
                        await SaveEmpRoleAccess(empmaster.addEmpRoleAccess, employeeId);
                        await SaveEmpCentreAccess(empmaster.addEmpCentreAccess, employeeId);
                        await transaction.CommitAsync();
                        var result = db.empMaster.Where(e => e.id == employeeId).ToList();
                        result.ForEach(e => e.fileName = image);
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Data = result
                        };
                    }
                    else
                    {
                        var EmpMaster = db.empMaster.FirstOrDefault(em => em.id == empmaster.id);
                        if (EmpMaster != null)
                        {
                            UpdateEmployee(EmpMaster, empmaster);
                        }
                        var EmployeeMaster = db.empMaster.Update(EmpMaster);
                        await db.SaveChangesAsync();
                        var employeeId = EmployeeMaster.Entity.id;
                        await UpdateEmpRoleAccess(empmaster.addEmpRoleAccess, employeeId);
                        await UpdateEmpCentreAccess(empmaster.addEmpCentreAccess, employeeId);
                        await transaction.CommitAsync();
                        var result = db.empMaster.Where(e => e.id == employeeId).ToList();
                        result.ForEach(e => e.fileName = image);
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Data = result
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
                id = empmaster.id,
                empCode = empmaster.empCode,
                title = empmaster.title,
                fName = empmaster.fName,
                lName = empmaster.lName,
                address = empmaster.address,
                pinCode = empmaster.pinCode,
                email = empmaster.email,
                mobileNo = empmaster.mobileNo,
                landline = empmaster.landline,
                deptAccess = empmaster.deptAccess,
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
                isdeviceAuthentication = empmaster.isdeviceAuthentication

            };
        }

        private async Task<ServiceStatusResponseModel> SaveEmpRoleAccess(IEnumerable<empRoleAccess> emproleaccess, int employeeId)
        {
            if (emproleaccess != null)
            {
                //foreach (var emprole in emproleaccess)
                //{
                //    var EmpRoleData = CreateEmpRoleData(emprole, employeeId);
                //    var EmpRole = db.empRoleAccess.Add(EmpRoleData);
                //    await db.SaveChangesAsync();
                //}
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
                id = emproleaccess.id,
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
                //foreach (var empcentre in empcentreaccess)
                //{
                //    var EmpCentreData = CreateEmpCentreData(empcentre, employeeId);
                //    var EmpCentre = db.empCenterAccess.Add(EmpCentreData);
                //    await db.SaveChangesAsync();
                //}
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
                id = empcentreaccess.id,
                empId = employeeId,
                centreId = empcentreaccess.centreId,
                isActive = empcentreaccess.isActive,
                createdById = empcentreaccess.createdById,
                createdDateTime = empcentreaccess.createdDateTime
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
            EmpMaster.landline = empmaster.landline;
            EmpMaster.deptAccess = empmaster.deptAccess;
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
        }

        private async Task<ServiceStatusResponseModel> UpdateEmpCentreAccess(IEnumerable<empCenterAccess> empcenteraccess, int employeeId)
        {
            foreach (var empCentre in empcenteraccess)
            {
                if (empCentre.id != 0)
                {
                    var empCentreData = await db.empCenterAccess.FirstOrDefaultAsync(em => em.id == empCentre.id);
                    if (empCentreData != null)
                    {
                        UpdateEmpCentre(empCentreData, empCentre, employeeId);
                        var EmpCentreData = db.empCenterAccess.Update(empCentreData);
                        await db.SaveChangesAsync();
                        var id = EmpCentreData.Entity.id;
                    }
                }
                else
                {

                    var empCenterData = CreateEmpCentreData(empCentre, employeeId);
                    var empCenter = db.empCenterAccess.Add(empCenterData);
                    await db.SaveChangesAsync();
                }

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = empcenteraccess
            };
        }
        private void UpdateEmpCentre(empCenterAccess EmpCentreAccess, empCenterAccess empcentreaccess, int employeeId)
        {
            EmpCentreAccess.empId = employeeId;
            EmpCentreAccess.centreId = empcentreaccess.centreId;
            EmpCentreAccess.isActive = empcentreaccess.isActive;
            EmpCentreAccess.updateById = empcentreaccess.updateById;
            EmpCentreAccess.updateDateTime = empcentreaccess.updateDateTime;
        }

        private async Task<ServiceStatusResponseModel> UpdateEmpRoleAccess(IEnumerable<empRoleAccess> emproleaccess, int employeeId)
        {
            foreach (var emprole in emproleaccess)
            {
                if (emprole.id != 0)
                {
                    var empRoleData = await db.empRoleAccess.FirstOrDefaultAsync(em => em.id == emprole.id);
                    if (empRoleData != null)
                    {
                        UpdateEmprole(empRoleData, emprole, employeeId);
                        var EmpRoleData = db.empRoleAccess.Update(empRoleData);
                        await db.SaveChangesAsync();
                        //   var id = empRoleData.Entity.id;
                    }
                }
                else
                {

                    var empRoleData = CreateEmpRoleData(emprole, employeeId);
                    var empRole = db.empRoleAccess.Add(empRoleData);
                    await db.SaveChangesAsync();
                }

            }
            return new ServiceStatusResponseModel
            {
                Success = true,
                Data = emproleaccess
            };
        }
        private void UpdateEmprole(empRoleAccess EmpRoleAccess, empRoleAccess emproleaccess, int employeeId)
        {
            EmpRoleAccess.empId = employeeId;
            EmpRoleAccess.roleId = emproleaccess.roleId;
            EmpRoleAccess.isActive = emproleaccess.isActive;
            EmpRoleAccess.updateById = emproleaccess.updateById;
            EmpRoleAccess.updateDateTime = emproleaccess.updateDateTime;
        }

        private async Task<string> Uploademployeeimage(string fileBase64data)
        {
            try
            {
                if (string.IsNullOrEmpty(fileBase64data))
                {
                    return "Invalid image data.";
                }
                string primaryFolder = _configuration["DocumentPath:PrimaryFolder"];
                string mainFolder = _configuration["DocumentPath:EmployeeImage"];
                if (string.IsNullOrEmpty(primaryFolder) || string.IsNullOrEmpty(mainFolder))
                {
                    return "Invalid folder configuration.";
                }
                if (!Directory.Exists(mainFolder))
                {
                    Directory.CreateDirectory(mainFolder);
                }
                string uploadPath = Path.Combine(primaryFolder, mainFolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string extension = ".jpg";
                if (fileBase64data.StartsWith("data:image/png;base64,"))
                {
                    extension = ".png";
                    fileBase64data = fileBase64data.Substring("data:image/png;base64,".Length);  // Remove data URL prefix
                }
                else if (fileBase64data.StartsWith("data:image/jpeg;base64,"))
                {
                    extension = ".jpeg";
                    fileBase64data = fileBase64data.Substring("data:image/jpeg;base64,".Length);  // Remove data URL prefix
                }

                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadPath, fileName);
                byte[] fileBytes = Convert.FromBase64String(fileBase64data);
                await File.WriteAllBytesAsync(filePath, fileBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                return "Error uploading image";
            }
        }


        async Task<ServiceStatusResponseModel> IempMasterServices.UploadDocument(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (extension != ".pdf" && extension != ".Pdf" && extension != ".PDF")
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
                         var imagepath = db.empMaster.Where(e => e.id == emplpyeeid).Select(e => e.fileName).FirstOrDefault();
                      //  var imagepath = "D:\\UploadDocument\\EmpImage\\f9ed7ffd-efd0-452d-9785-d908e4890853.jpeg";
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
    }
}
