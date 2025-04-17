﻿using Microsoft.AspNetCore.Authorization;

namespace Warehouse.Common.Security
{
    public static class Policies
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Supplier = "Supplier";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }
        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
        }

        public static AuthorizationPolicy SupplierPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Supplier).Build();
        }
    }
}
