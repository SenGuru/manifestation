using System;
using System.Collections.Generic;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.Account;

namespace Manifest.Common.Interfaces.Service.App
{
    public interface ITokenGeneratorService
    {
        LoggedInUserModel GetUserToken(UserModel user, long predefinedCurrencyTypeId);

        (string authValue, DateTime validUntil, Exception ex) GetUserInfoFromToken(string token);

        LoggedInUserModel CreateExpiringUserToken(UserModel user, long expirationMinutes, List<string> permissions = null);

        string GetUserTokenAsString(UserModel user, long predefinedCurrencyTypeId);
    }
}