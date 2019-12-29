using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;
using System.Data;

namespace CustomFramework.BaseWebApi.Identity.Utils
{
    public static class ClaimChecker
    {
        public static Claim CheckClaimStatus(Claim claim, IList<Claim> userOrRoleclaims, IList<Claim> existingClaims)
        {
            var claimIsExist = (from p in existingClaims where p.Type == claim.Type select p).Count() > 0;
            if (!claimIsExist) throw new KeyNotFoundException(nameof(Claim));

            bool claimHasAlreadyAssigned;

            //Eğer claim değeri true ya da false değilse, linq sorgusunun where koşulunda claim değeri de çekiliyor.
            //Örnek olarak bir role bir claim tipi için true değerinde yetki atandıysa sistemde aynı claim tipinden false değeri olamaz
            //Fakat ÇalıştığıSektör gibi bir claim tipi yetkisi için sistemde birden fazla claim değeri olabilir.
            if (Convert.ToBoolean(claim.Value))
            {
                claimHasAlreadyAssigned = (from p in userOrRoleclaims where p.Type == claim.Type select p).Count() > 0;
            }
            else
            {
                claimHasAlreadyAssigned = (from p in userOrRoleclaims where p.Type == claim.Type &&
                    p.Value == claim.Value select p).Count() > 0;
            }

            if (claimHasAlreadyAssigned) throw new DuplicateNameException(nameof(Claim));
            return claim;
        }
    }
}