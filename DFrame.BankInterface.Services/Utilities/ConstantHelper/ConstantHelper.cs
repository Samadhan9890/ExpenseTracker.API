using System.Reflection.Metadata;

namespace ExpenseTracker.Services.Utilities.ConstantHelper
{
    public static class ConstantHelper
    {
        // To get the keys from configurations
        #region CONFIGURATION_KEYS

        #region JWT_TOKENS
        public const string JWT_SECRET =  "JWT:SECRET";
        public const string JWT_ISSUER = "JWT:ISSUER";
        #endregion JWT_TOKENS

        public const string cors_react = "CORS_REACT";

        #endregion CONFIGURATION_KEYS

        public enum payoutMethodsEnum
        {
            Bank,
            Upi
        }

        public enum SubscriptionStatusEnum
        {
            Active =1,
			ForceClosed = 2,
            Matured_Closed = 3,
            Matured_Renewed =4,
            Deleted = 5

		}

        public enum NCAuditModulesEnum
        {
            CLIENT_MASTER,
            SUBSCRIPTION,
            PAYMENTS,
            SPL_INVESTMENTS,
            SPL_PAYMENTS
        }

        public enum PaymentStatusEnum
        {
            Pending = 0,
            SentForProcessing=1,
            HoldByMaker=2,
            HoldByProcessor=3,
            Complete=4
        }


        public enum  SplPaymentTypesEnum
        {
            JoiningBonus=0,
            ClientProfit= 1,
            ReferralBonus=2,
            MaturityBonus=3,
            CapitalReturn=4
            
        }
    }
}
