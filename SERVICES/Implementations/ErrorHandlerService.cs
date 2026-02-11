using System;
using System.Data.SqlClient;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class ErrorHandlerService : IErrorHandlerService
    {
        private readonly ILogService _logService;
        private readonly ILocalizationService _localizationService;

        public ErrorHandlerService(ILogService logService, ILocalizationService localizationService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        public void HandleError(Exception ex, string context = null)
        {
            var contextMessage = string.IsNullOrEmpty(context) ? "An error occurred" : context;
            _logService.Error(contextMessage, ex);
        }

        public string GetFriendlyMessage(Exception ex)
        {
            if (ex == null)
                return "An unknown error occurred.";

            // Specific error types
            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: // Unique constraint violation
                    case 2601:
                        return _localizationService.GetString("Error.DuplicateEntry") ?? 
                               "This record already exists. Please use a different value.";
                    case 547: // Foreign key constraint violation
                        return _localizationService.GetString("Error.ForeignKeyViolation") ?? 
                               "Cannot delete this record because it is referenced by other records.";
                    case -1: // Connection timeout
                    case -2:
                        return _localizationService.GetString("Error.DatabaseTimeout") ?? 
                               "Database connection timeout. Please try again.";
                    default:
                        return _localizationService.GetString("Error.DatabaseError") ?? 
                               "A database error occurred. Please contact support.";
                }
            }

            if (ex is InvalidOperationException)
            {
                return _localizationService.GetString("Error.InvalidOperation") ?? 
                       "The operation cannot be completed at this time.";
            }

            if (ex is ArgumentException || ex is ArgumentNullException)
            {
                return _localizationService.GetString("Error.InvalidData") ?? 
                       "Invalid data provided. Please check your input.";
            }

            if (ex is UnauthorizedAccessException)
            {
                return _localizationService.GetString("Error.Unauthorized") ?? 
                       "You do not have permission to perform this action.";
            }

            // Generic error
            return _localizationService.GetString("Error.Generic") ?? 
                   "An unexpected error occurred. Please try again or contact support.";
        }

        public void ShowError(Exception ex, string context = null)
        {
            HandleError(ex, context);
            var friendlyMessage = GetFriendlyMessage(ex);
            
            // This will be called from UI layer
            System.Windows.Forms.MessageBox.Show(
                friendlyMessage,
                _localizationService.GetString("Common.Error") ?? "Error",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );
        }
    }
}
