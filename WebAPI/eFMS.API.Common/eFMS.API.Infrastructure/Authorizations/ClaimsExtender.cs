﻿using eFMS.IdentityServer.DL.IService;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace eFMS.API.Infrastructure.Authorizations
{
    public class ClaimsExtender : IClaimsTransformation
    {
        private IUserPermissionService _userpermissionService;
        public ClaimsExtender(IUserPermissionService userpermissionService)
        {
            _userpermissionService = userpermissionService;
        }

        async Task<ClaimsPrincipal> IClaimsTransformation.TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                string userID = principal.FindFirstValue("id");

                Guid officeId = new Guid(principal.Claims.Where(claim => claim.Type.Equals("officeId")).FirstOrDefault().Value);
                List<Claim> lstClaim = new List<Claim>();
                var lstPermissions = await _userpermissionService.GetPermission(userID, officeId);
                if (lstPermissions != null)
                {
                    lstPermissions.ForEach(x => lstClaim.Add(new Claim(JwtClaimTypes.Role, x)));
                }
                principal.AddIdentity(new ClaimsIdentity(lstClaim, JwtBearerDefaults.AuthenticationScheme, "name", "role"));
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            //Add additional claims here.
            return await Task.FromResult(principal);
        }
    }
}
